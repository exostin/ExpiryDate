using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Controllers.BattleScene
{
    public class BattleMenuController : MonoBehaviour
    {
        private GameManager gm;
        private PostProcessingController postProcessingController;
        
        public List<GameObject> playerCharacters;
        public List<GameObject> enemyCharacters;
        private readonly BattleMenuActions battleMenuActions = new();
        private List<GameObject> allCharacters;
        private bool playerWon;
        
        #region Scriptable Objects

        // so - scriptable object
        [HideInInspector] public List<Character> soPlayerCharacters;
        [HideInInspector] public List<Character> soEnemyCharacters;

        [HideInInspector] public Character playerSelectedTarget;
        [HideInInspector] public Ability playerSelectedAbility;

        private readonly List<Character> targetsForEnemyPool = new();
        private readonly List<Character> targetsForPlayerPool = new();
        private List<Character> battleQueue = new();
        private List<Character> soAllCharacters;

        #endregion

        #region UI Elements

        [SerializeField] private GameObject[] skillButtons;
        [SerializeField] private GameObject targetIndicator;
        [SerializeField] private GameObject abilityIndicator;
        private int turnCounter;
        [SerializeField] private TMP_Text turnCounterText;
        [SerializeField] private GameObject playerWonCanvas;
        [SerializeField] private GameObject playerLostCanvas;

        #endregion

        #region Delays, offsets etc.

        [SerializeField] private float delayBetweenActions;
        [SerializeField] private float timeBeforeBattleStart;

        #endregion

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            postProcessingController = FindObjectOfType<PostProcessingController>();

            #region Creating variables related to characters
            
            allCharacters = playerCharacters.Concat(enemyCharacters).ToList();
            ExtractCharactersData();
            CreateTargetPools();
            CreateQueue();
            InitializeAllCharactersDefaultStats();
            
            #endregion

            #region UI elements

            LetPlayerChooseTarget(false);
            ToggleAbilityButtonsVisibility(false);

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
            playerWon = soPlayerCharacters.Any(character => character.health > 0) &&
                        soEnemyCharacters.All(character => character.health <= 0);
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return soPlayerCharacters.Any(character => character.health > 0) &&
                   soEnemyCharacters.All(character => character.health <= 0)
                   || soPlayerCharacters.All(character => character.health <= 0) &&
                   soEnemyCharacters.Any(character => character.health > 0);
        }

        /// <summary>
        ///     Starts a whole battle consisting of multiple turns
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayBattle()
        {
            yield return new WaitForSecondsRealtime(timeBeforeBattleStart);
            while (!CheckIfAnySideWon())
            {
                StartCoroutine(MakeTurn());
                yield return new WaitUntil(
                    () => gm.stateController.fsm.State == StateController.States.ReadyForNextTurn);
            }

            StartCoroutine(GameEnd());
        }

        /// <summary>
        ///     Makes a turn consisting of one action for each character in the battle queue
        /// </summary>
        /// <returns></returns>
        private IEnumerator MakeTurn()
        {
            UpdateTurnCounter();

            foreach (var character in battleQueue.ToList())
            {
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
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
                    postProcessingController.ToggleEnemyTurnPostEffects(gm);
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    ToggleAbilityButtonsVisibility(true);
                    UpdateSkillButtons(character);
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() =>
                        gm.stateController.fsm.State == StateController.States.PlayerFinalizedHisMove);
                    Debug.Log("Player finalized his move!");
                    battleMenuActions.MakeAction(character, playerSelectedTarget, playerSelectedAbility,
                        soAllCharacters, true);
                    StartCoroutine(postProcessingController.MakeAttackPostEffects());
                    ToggleAbilityButtonsVisibility(false);
                    playerSelectedAbility = null;
                    playerSelectedTarget = null;
                }
                else
                {
                    gm.stateController.fsm.ChangeState(StateController.States.EnemyTurn);
                    postProcessingController.ToggleEnemyTurnPostEffects(gm);
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    var enemySelectedTarget = targetsForEnemyPool[randomTargetIndex];
                    var randomEnemyAbilityIndex = Random.Range(0, character.abilities.Length);
                    var enemySelectedAbility = character.abilities[randomEnemyAbilityIndex];
                    battleMenuActions.MakeAction(character, enemySelectedTarget, enemySelectedAbility, soAllCharacters,
                        false);
                    StartCoroutine(postProcessingController.MakeAttackPostEffects());
                }
                DisableSelectionIndicators();
                LetPlayerChooseTarget(false);
                yield return new WaitForSecondsRealtime(delayBetweenActions);
                currentChar.GetComponent<MoveActiveCharacterToCenter>().MoveBack();
            }

            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        /// <summary>
        ///     Increment the turn counter and show it on the screen
        /// </summary>
        private void UpdateTurnCounter()
        {
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter;
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
                $"Full end-turn data dump: " +
                $"State: {gm.stateController.fsm.State}, " +
                $"Selected ability: {playerSelectedAbility}," +
                $"Selected target: {playerSelectedTarget}, is the target dead?: {playerSelectedTarget.isDead}, " +
                $"Targets for player pool: {targetsForPlayerPool}, Targets for enemy pool: {targetsForEnemyPool}" +
                $"Can the ability target only allies?: {playerSelectedAbility.canOnlyTargetOwnCharacters}," +
                $"Can the ability target only the caster?: {playerSelectedAbility.usedOnlyOnSelf}," +
                $"Does the ability target whole team?: {playerSelectedAbility.targetsWholeTeam}," +
                $"Is the ability a heal?: {playerSelectedAbility.heal}, is the ability a buff?: {playerSelectedAbility.buff}");
                
            if (gm.stateController.fsm.State == StateController.States.SelectingTarget &&
                playerSelectedAbility != null && playerSelectedTarget != null && !playerSelectedTarget.isDead)
            {
                if (targetsForPlayerPool.Contains(playerSelectedTarget) &&
                    !playerSelectedAbility.canOnlyTargetOwnCharacters)
                {
                    Debug.Log("Attacking enemy, setting state to PlayerFinalizedHisMove");
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                }
                else
                {
                    if (playerSelectedTarget.isOwnedByPlayer && playerSelectedAbility.canOnlyTargetOwnCharacters)
                    {
                        if (playerSelectedAbility.buff && playerSelectedAbility.usedOnlyOnSelf)
                        {
                            Debug.Log("Buffing own character, setting state to PlayerFinalizedHisMove");
                            gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                        }
                        else
                        {
                            Debug.Log(
                                "Targeting an alive ally with a supportive ability, setting state to PlayerFinalizedHisMove.");
                            gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                        }
                    }
                    else
                    {
                        Debug.Log(playerSelectedTarget.isDead
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
        
        /// <summary>
        ///     Disable/enable interactability of all characters buttons
        /// </summary>
        /// <param name="choice">true/false</param>
        private void LetPlayerChooseTarget(bool choice)
        {
            Debug.Log($"Setting interactability with characters to: {choice}");
            foreach (var character in battleQueue)
            foreach (var g in allCharacters.Where(g =>
                         g.GetComponent<DisplayCharacterData>().character.characterName == character.characterName))
                g.GetComponent<Button>().interactable = choice;
        }

        /// <summary>
        ///     Set all ability data to match the character which turn it is
        /// </summary>
        /// <param name="currentCharacter">character which turn it currently is</param>
        private void UpdateSkillButtons(Character currentCharacter)
        {
            Debug.Log("Updating ability info");
            for (var i = 0; i <= 3; i++)
            {
                skillButtons[i].GetComponent<DisplayAbilityData>().ability = currentCharacter.abilities[i];
                skillButtons[i].GetComponent<DisplayAbilityData>().UpdateAbilityDisplay();
            }
        }

        private void ToggleAbilityButtonsVisibility(bool enable)
        {
            Debug.Log($"Setting visibility of abilities to {enable}");
            for (var i = 0; i <= 3; i++) skillButtons[i].gameObject.SetActive(enable);
        }

        /// <summary>
        ///     Disable the indicators that show what ability and target is currently selected by the player
        /// </summary>
        private void DisableSelectionIndicators()
        {
            targetIndicator.SetActive(false);
            abilityIndicator.SetActive(false);
        }

        /// <summary>
        ///     Start game end sequence
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameEnd()
        {
            gm.stateController.fsm.ChangeState(StateController.States.GameEnded);
            Debug.Log($"Game ended, player won: {playerWon}");
            Instantiate(playerWon ? playerWonCanvas : playerLostCanvas);
            yield return new WaitForSecondsRealtime(2.8f);
            SceneManager.LoadScene(1);
            gm.stateController.fsm.ChangeState(StateController.States.Playing);
        }
        
        #region Functions invoked only by buttons inside the UI

        public void StartTargetSelectionState()
        {
            gm.stateController.fsm.ChangeState(StateController.States.SelectingTarget);
            LetPlayerChooseTarget(true);
        }

        #endregion
        
    }
}