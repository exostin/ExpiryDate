using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using UnityEngine;

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
        public IEnumerable<GameObject> AllCharacters => allCharacters;

        #endregion

        #region Other

        public bool PlayerWon { get; private set; }
        public Character PlayerSelectedTarget { get; set; }
        public Character PlayerHoveredOverTarget { get; set; }
        public Ability PlayerSelectedAbility { get; set; }

        #endregion

        #endregion

        #region Fields

        #region References set automatically in Start()

        private GameManager gm;
        private StateController stateController;
        private PostProcessingController postProcessingController;
        private BattleUIController battleUIController;

        #endregion

        #region GameObject Lists

        public List<GameObject> playerCharacters;
        public List<GameObject> enemyCharacters;
        private List<GameObject> allCharacters;

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

        #endregion
        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            stateController = FindObjectOfType<StateController>();
            postProcessingController = FindObjectOfType<PostProcessingController>();
            battleUIController = FindObjectOfType<BattleUIController>();

            #region Creating variables related to characters

            allCharacters = playerCharacters.Concat(enemyCharacters).ToList();
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

            #endregion
            StartCoroutine(PlayBattle());
        }

        /// <summary>
        ///     Initialize all characters' default stats (currentHealth = maxHealth etc.)
        /// </summary>
        private void InitializeAllCharactersDefaultStats()
        {
            foreach (var character in battleQueue) character.Initialize();
        }

        /// <summary>
        ///     Extract character scriptable object data from their game objects to their respective lists
        /// </summary>
        private void ExtractCharactersData()
        {
            foreach (var g in playerCharacters)
                soPlayerCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
            foreach (var g in enemyCharacters) soEnemyCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
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
        ///     Checks if either of teams has won (all characters are dead while the other team still stands)
        /// </summary>
        /// <returns></returns>
        private bool CheckIfAnySideWon()
        {
            PlayerWon = soPlayerCharacters.Any(character => character.health > 0) &&
                        soEnemyCharacters.All(character => character.health <= 0);
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return (soPlayerCharacters.Any(character => character.health > 0) &&
                    soEnemyCharacters.All(character => character.health <= 0))
                   || (soPlayerCharacters.All(character => character.health <= 0) &&
                       soEnemyCharacters.Any(character => character.health > 0));
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
                if (CheckIfAnySideWon()) break;
                if (character.isDead)
                {
                    if (character.isOwnedByPlayer)
                        targetsForEnemyPool.Remove(character);
                    else
                        targetsForPlayerPool.Remove(character);
                    battleQueue.Remove(character);
                    continue;
                }

                Debug.Log($"{character.characterName} turn!");
                var currentChar = FindCharactersGameObjectByName(character);

                // AD HOC, TO BE CHANGED ASAP
                currentChar.GetComponent<MoveActiveCharacterToCenter>().MoveToCenter(character.isOwnedByPlayer ? 1 : 2);
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
                    battleActions.MakeAction(character, PlayerSelectedTarget, PlayerSelectedAbility,
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
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    var enemySelectedTarget = targetsForEnemyPool[randomTargetIndex];
                    var randomEnemyAbilityIndex = Random.Range(0, character.abilities.Length);
                    var enemySelectedAbility = character.abilities[randomEnemyAbilityIndex];
                    battleActions.MakeAction(character, enemySelectedTarget, enemySelectedAbility, soAllCharacters,
                        false);
                    StartCoroutine(postProcessingController.MakeAttackPostEffects());
                }

                battleUIController.DisableSelectionIndicators();
                battleUIController.LetPlayerChooseTarget(false);
                yield return new WaitForSecondsRealtime(delayBetweenActions);
                currentChar.GetComponent<MoveActiveCharacterToCenter>().MoveBack();
            }

            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        /// <summary>
        ///     Search through all attached character GameObjects, and return the one which DisplayCharacterData component has a
        ///     Character scriptable object attached with the same name as the chosen character
        /// </summary>
        private GameObject FindCharactersGameObjectByName(Character character)
        {
            foreach (var g in allCharacters.Where(g =>
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
            Debug.Log(
                "Full end-turn data dump: " +
                $"State: {stateController.fsm.State}, " +
                $"Selected ability: {PlayerSelectedAbility}," +
                $"Selected target: {PlayerSelectedTarget}, is the target dead?: {PlayerSelectedTarget.isDead}, " +
                $"Targets for player pool: {targetsForPlayerPool}, Targets for enemy pool: {targetsForEnemyPool}" +
                $"Can the ability target only allies?: {PlayerSelectedAbility.canOnlyTargetOwnCharacters}," +
                $"Can the ability target only the caster?: {PlayerSelectedAbility.usedOnlyOnSelf}," +
                $"Does the ability target whole team?: {PlayerSelectedAbility.targetsWholeTeam}," +
                $"Is the ability a heal?: {PlayerSelectedAbility.heal}, is the ability a buff?: {PlayerSelectedAbility.buff}");

            if (stateController.fsm.State == StateController.States.SelectingTarget &&
                PlayerSelectedAbility != null && PlayerSelectedTarget != null && !PlayerSelectedTarget.isDead)
            {
                if (targetsForPlayerPool.Contains(PlayerSelectedTarget) &&
                    !PlayerSelectedAbility.canOnlyTargetOwnCharacters)
                {
                    Debug.Log("Attacking enemy, setting state to PlayerFinalizedHisMove");
                    stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                }
                else
                {
                    if (PlayerSelectedTarget.isOwnedByPlayer && PlayerSelectedAbility.canOnlyTargetOwnCharacters)
                    {
                        if (PlayerSelectedAbility.buff && PlayerSelectedAbility.usedOnlyOnSelf)
                        {
                            Debug.Log("Buffing own character, setting state to PlayerFinalizedHisMove");
                            stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                        }
                        else
                        {
                            Debug.Log(
                                "Targeting an alive ally with a supportive ability, setting state to PlayerFinalizedHisMove.");
                            stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                        }
                    }
                    else
                    {
                        Debug.Log(PlayerSelectedTarget.isDead
                            ? "You somehow selected a dead target!"
                            : "Wrong target! You targeted: not a targetable enemy/ally with an ability that's not supportive");
                    }
                }
            }
            else
            {
                Debug.Log("No ability and/or target chosen!");
            }
        }
    }
}