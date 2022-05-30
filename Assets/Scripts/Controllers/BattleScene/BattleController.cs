using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using Other.Enums;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Controllers.BattleScene
{
    public class BattleController : MonoBehaviour
    {
        #region Properties

        #region Iterate-through-only lists

        // Could be used here depending on need: IReadOnlyCollection/IReadOnlyList/IEnumerable
        public IEnumerable<Character> SoPlayerCharacters => soPlayerCharacters;
        public IEnumerable<Character> SoEnemyCharacters => soEnemyCharacters;
        public IEnumerable<Character> BattleQueue => battleQueue;
        public IEnumerable<GameObject> AllCharactersGameObjects => allCharactersGameObjects;

        #endregion

        #region Other

        public bool PlayerWon { get; private set; }
        public Character PlayerSelectedTarget { get; set; }
        public Character PlayerHoveredOverTarget { get; set; }
        private Character ThisTurnCharacter { get; set; }
        public Ability PlayerSelectedAbility { get; set; }
        private GameObject CurrentCharGameObject { get; set; }

        #endregion

        #endregion

        #region Fields

        #region References set automatically in Start()

        private GameManager gm;
        private StateController stateController;
        private PostProcessingController postProcessingController;
        private BattleUIController battleUIController;
        private readonly StatusHandler statusHandler = new();

        #endregion

        #region GameObject Lists

        public List<GameObject> playerCharactersGameObjects;
        public List<GameObject> enemyCharactersGameObjects;
        private List<GameObject> allCharactersGameObjects;

        #endregion

        #region Static class references

        private readonly BattleActions battleActions = new();

        #endregion

        #region Scriptable Objects

        // so - scriptable object
        private readonly List<Character> soPlayerCharacters = new();
        private readonly List<Character> soEnemyCharacters = new();
        private List<Character> battleQueue;
        private List<Character> soAllCharacters;
        private readonly List<Character> targetsForEnemyPool = new();
        private readonly List<Character> targetsForPlayerPool = new();
        
        #endregion

        #region Delays, offsets etc.

        [SerializeField] private float delayBetweenActions;
        [SerializeField] private float timeBeforeBattleStart;

        #endregion

        #region Other

        public Color deadCharacterTint = new Color(166f, 0.33f, 0.33f, 1f);

        #endregion

        #region Events

        public delegate void BattleEvent();
        public static event BattleEvent OnActionMade;
        public static event BattleEvent OnStatusHandled;

        #endregion
        
        #endregion
        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            stateController = FindObjectOfType<StateController>();
            postProcessingController = FindObjectOfType<PostProcessingController>();
            battleUIController = FindObjectOfType<BattleUIController>();
            
            #region Creating variables related to characters

            allCharactersGameObjects = playerCharactersGameObjects.Concat(enemyCharactersGameObjects).ToList();
            ExtractCharactersData();
            CreateTargetPools();
            CreateQueue();
            InitializeAllCharactersDefaultStats();

            #endregion

            #region UI elements

            battleUIController.LetPlayerChooseTarget(false);
            battleUIController.ToggleAbilityButtonsVisibility(false);

            #endregion

            #region Events config

            DisplayCharacterData.OnTurnEnd += EndPlayerTurn;
            Character.OnCharacterDeath += DisableInteractabilityForCharacter;

            #endregion
            StartCoroutine(PlayBattle());
        }

        /// <summary>
        ///     Extract character scriptable object data from GameManager to their respective lists
        /// </summary>
        private void ExtractCharactersData()
        {
            var index = 0;
            foreach (Character character in gm.selectedDefenders)
            {
                soPlayerCharacters.Add(character);
                var displayCharacterDataComponent = playerCharactersGameObjects[index].GetComponent<DisplayCharacterData>();
                displayCharacterDataComponent.character = character;
                displayCharacterDataComponent.Initialize();
                index++;
            }

            index = 0;
            foreach (Character character in gm.thisEncounterEnemies)
            {
                soEnemyCharacters.Add(character);
                var displayCharacterDataComponent = enemyCharactersGameObjects[index].GetComponent<DisplayCharacterData>();
                displayCharacterDataComponent.character = character;
                displayCharacterDataComponent.Initialize();
                index++;
            }

            soAllCharacters = soPlayerCharacters.Concat(soEnemyCharacters).ToList();
        }

        /// <summary>
        ///     Creates a list of characters which are targets for player, and another one for the enemy
        /// </summary>
        private void CreateTargetPools()
        {
            targetsForPlayerPool.AddRange(soEnemyCharacters);
            targetsForEnemyPool.AddRange(soPlayerCharacters);
        }

        /// <summary>
        ///     Create a queue in form of List of all the characters to be used in battle
        /// </summary>
        private void CreateQueue()
        {
            // Merge playerCharacters and enemyCharacters into one array
            battleQueue = soPlayerCharacters.Concat(soEnemyCharacters).ToList();
            // Sort the battle queue by initiative
            battleQueue = battleQueue.OrderByDescending(character => character.initiative).ToList();
        }
        
        /// <summary>
        ///     Initialize all characters' default stats (currentHealth = maxHealth etc.)
        /// </summary>
        private void InitializeAllCharactersDefaultStats()
        {
            foreach (Character character in battleQueue) character.Initialize();
        }

        /// <summary>
        ///     Checks if either of teams has won (all characters are dead while the other team still stands)
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAnySideWon()
        {
            PlayerWon = soPlayerCharacters.Any(character => character.Health > 0) &&
                        soEnemyCharacters.All(character => character.Health <= 0);
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return (soPlayerCharacters.Any(character => character.Health > 0) &&
                    soEnemyCharacters.All(character => character.Health <= 0))
                   || (soPlayerCharacters.All(character => character.Health <= 0) &&
                       soEnemyCharacters.Any(character => character.Health > 0));
        }
        
        /// <summary>
        ///     Starts a whole battle consisting of multiple turns
        /// </summary>
        private IEnumerator PlayBattle()
        {
            yield return new WaitForSecondsRealtime(timeBeforeBattleStart);
            while (!CheckIfAnySideWon())
            {
                StartCoroutine(MakeTurn());
                yield return new WaitUntil(
                    () => stateController.fsm.State == StateController.States.ReadyForNextTurn);
            }

            StartCoroutine(battleUIController.GameEnd());
        }
        
        /// <summary>
        ///     Makes a turn consisting of one action for each character in the battle queue
        /// </summary>
        private IEnumerator MakeTurn()
        {
            battleUIController.UpdateTurnCounter();

            for (var index = 0; index < battleQueue.Count; index++)
            {
                Character character = battleQueue[index];
                ThisTurnCharacter = character;
                Debug.Log($"{character.characterName} turn!");
                CurrentCharGameObject = FindCharactersGameObjectByName(character);
                PlayerHoveredOverTarget = character;
                DisplayCharacterData.ActivateOnHoveredEvent();
                statusHandler.HandleStatuses(character, out bool skipThisTurn, CurrentCharGameObject);
                OnStatusHandled?.Invoke();
                if (CheckIfAnySideWon()) break;
                if (character.CheckIfDead())
                {
                    // currentCharGameObject.GetComponent<DisplayCharacterData>().VisualizeDeathOnDeadCharacters();
                    // currentCharGameObject.GetComponent<Button>().interactable = false;
                    if (character.isOwnedByPlayer)
                        targetsForEnemyPool.Remove(character);
                    else
                        targetsForPlayerPool.Remove(character);
                    battleQueue.Remove(character);
                    continue;
                }
                
                if (skipThisTurn)
                {
                    Debug.Log($"{character.name} is stunned, thus the turn will be skipped. {character.StunDurationLeft} stun turns left.");
                    continue;
                }

                // AD HOC, TO BE CHANGED ASAP
                CurrentCharGameObject.GetComponent<MoveActiveCharacterToCenter>().MoveToCenter(character.isOwnedByPlayer ? 1 : 2);
                if (character.isOwnedByPlayer)
                {
                    stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                    postProcessingController.ToggleEnemyTurnPostEffects(gm);
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    battleUIController.ToggleAbilityButtonsVisibility(true);
                    battleUIController.UpdateSkillButtons(character);
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() =>
                        stateController.fsm.State == StateController.States.PlayerFinalizedHisMove);
                    Debug.Log("Player finalized his move!");
                    battleActions.AssignNotificationHandlerReference(FindCharactersGameObjectByName(PlayerSelectedTarget));
                    battleActions.MakeAction(PlayerSelectedTarget, PlayerSelectedAbility,
                        soAllCharacters, true);
                    StartCoroutine(postProcessingController.MakeAttackPostEffects());
                    battleUIController.ToggleAbilityButtonsVisibility(false);
                    PlayerSelectedAbility = null;
                    PlayerSelectedTarget = null;
                }
                else
                {
                    stateController.fsm.ChangeState(StateController.States.EnemyTurn);
                    postProcessingController.ToggleEnemyTurnPostEffects(gm);
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    
                    var modifiedTargetsOfEnemyPool = targetsForEnemyPool.Where(target =>
                        !target.DodgeEverythingUntilNextTurn).ToList();
                    
                    var randomEnemyAbilityIndex = Random.Range(0, character.abilities.Length);
                    Ability enemySelectedAbility = character.abilities[randomEnemyAbilityIndex];
                    
                    var randomTargetIndex = Random.Range(0, modifiedTargetsOfEnemyPool.Count);
                    Character enemySelectedTarget;
                    
                    switch (enemySelectedAbility.abilityTarget)
                    {
                        case TargetType.SelfOnly or TargetType.MultipleTeammates:
                            enemySelectedTarget = character;
                            break;
                        case TargetType.SingleTeammate:
                            enemySelectedTarget = soEnemyCharacters[randomTargetIndex];
                            break;
                        default:
                            enemySelectedTarget = modifiedTargetsOfEnemyPool[randomTargetIndex];
                            break;
                    }
                    
                    battleActions.AssignNotificationHandlerReference(FindCharactersGameObjectByName(enemySelectedTarget));
                    battleActions.MakeAction(enemySelectedTarget, enemySelectedAbility, soAllCharacters,
                        false);
                    StartCoroutine(postProcessingController.MakeAttackPostEffects());
                }
                OnActionMade?.Invoke();
                battleUIController.DisableSelectionIndicators();
                battleUIController.LetPlayerChooseTarget(false);
                yield return new WaitForSecondsRealtime(delayBetweenActions);
                CurrentCharGameObject.GetComponent<MoveActiveCharacterToCenter>().MoveBack();
            }

            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        /// <summary>
        ///     Search through all attached character GameObjects, and return the one which DisplayCharacterData component has a
        ///     Character scriptable object attached with the same name as the chosen character
        /// </summary>
        private GameObject FindCharactersGameObjectByName(Character character)
        {
            foreach (var g in allCharactersGameObjects.Where(g =>
                         g.GetComponent<DisplayCharacterData>().character.characterName == character.characterName))
                return g;
            // if nothing could be found:
            Debug.Log($"Could not find that character! FindGameObjectWithMatchingName({character.characterName})");
            return null;
        }

        /// <summary>
        ///     Check if all the required steps to end the turn were completed, if so then set the state to PlayerFinalizedHisMove
        /// </summary>
        public void EndPlayerTurn()
        {
            if (stateController.fsm.State == StateController.States.SelectingTarget &&
                PlayerSelectedAbility != null && PlayerSelectedTarget != null && !PlayerSelectedTarget.IsDead)
            {
                bool thisAbilityCanOnlyTargetTeammates = PlayerSelectedAbility.abilityTarget is TargetType.MultipleTeammates or TargetType.SingleTeammate;
                if (targetsForPlayerPool.Contains(PlayerSelectedTarget) &&
                    !thisAbilityCanOnlyTargetTeammates && PlayerSelectedAbility.abilityTarget is not TargetType.SelfOnly)
                {
                    Debug.Log("Attacking enemy, setting state to PlayerFinalizedHisMove");
                    stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                }
                else
                {
                    if (PlayerSelectedTarget.isOwnedByPlayer && thisAbilityCanOnlyTargetTeammates)
                    {
                        Debug.Log("Character used an ability on a teammate");
                        stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                    }
                    else if (PlayerSelectedTarget == ThisTurnCharacter &&
                             PlayerSelectedAbility.abilityTarget == TargetType.SelfOnly)
                    {
                        Debug.Log("Character used an ability on self");
                        stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                    }
                    else
                    {
                        Debug.Log("Wrong target! You targeted: not a targetable enemy/ally with an ability that's not supportive");
                    }
                }
            }
            else
            {
                Debug.Log("No ability and/or target chosen!");
            }
        }
        
        public void DisableInteractabilityForCharacter()
        {
            CurrentCharGameObject.GetComponent<Button>().interactable = false;
        }
    }
}