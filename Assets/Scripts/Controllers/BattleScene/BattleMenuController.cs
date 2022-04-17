using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Controllers.BattleScene
{
    public class BattleMenuController : MonoBehaviour
    {
        public List<GameObject> playerCharacters;
        public List<GameObject> enemyCharacters;

        // so - scriptable object
        [HideInInspector] public List<Character> soPlayerCharacters;
        [HideInInspector] public List<Character> soEnemyCharacters;
        [SerializeField] private TMP_Text turnCounterText;
        [SerializeField] private GameObject[] skillButtons;
        [HideInInspector] public Character playerSelectedTarget;
        [HideInInspector] public Ability playerSelectedAbility;
        [SerializeField] private GameObject targetIndicator;
        [SerializeField] private GameObject abilityIndicator;
        [SerializeField] private float delayBetweenActions;
        [SerializeField] private float timeBeforeBattleStart;
        [SerializeField] private Volume postprocessingVolume;
        [SerializeField] private float defaultVignetteIntensity;
        [SerializeField] private float enemyTurnVignetteIntensity;
        [SerializeField] private float attackChromaticAberrationIntensity = 0.5f;
        [SerializeField] private Color defaultVignette;
        [SerializeField] private GameObject playerWonCanvas;
        [SerializeField] private GameObject playerLostCanvas;

        [SerializeField] private Color enemyTurnVignette;
        public GameManager gm;
        
        private readonly BattleMenuActions battleMenuActions = new();
        private readonly List<Character> targetsForEnemyPool = new();
        private readonly List<Character> targetsForPlayerPool = new();
        private List<GameObject> allCharacters;
        private List<Character> battleQueue = new();
        private ChromaticAberration chromaticAberration;
        private bool playerWon;
        private List<Character> soAllCharacters;
        private int turnCounter;
        private Vignette vignette;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();

            var postprocessingVolumeProfile = postprocessingVolume.GetComponent<Volume>()?.profile;
            if (!postprocessingVolumeProfile) throw new NullReferenceException(nameof(VolumeProfile));
            if (!postprocessingVolumeProfile.TryGet(out vignette)) throw new NullReferenceException(nameof(vignette));
            if (!postprocessingVolumeProfile.TryGet(out chromaticAberration))
                throw new NullReferenceException(nameof(chromaticAberration));

            allCharacters = playerCharacters.Concat(enemyCharacters).ToList();

            ExtractCharactersData();

            CreateTargetPools();
            CreateQueue();

            SetCurrentCharacterHpToMax();

            LetPlayerChooseTarget(false);
            ToggleSkillButtonsVisibility(false);

            StartCoroutine(PlayBattle());
        }

        /// <summary>
        /// Sets all characters in battleQueue current HP to their MaxHP value
        /// </summary>
        private void SetCurrentCharacterHpToMax()
        {
            foreach (var character in battleQueue) character.health = character.maxHealth;
        }

        /// <summary>
        /// Extract character scriptable object data from their game objects to their respective lists
        /// </summary>
        private void ExtractCharactersData()
        {
            foreach (var g in playerCharacters)
                soPlayerCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
            foreach (var g in enemyCharacters) soEnemyCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
            soAllCharacters = soPlayerCharacters.Concat(soEnemyCharacters).ToList();
        }
        
        private void CreateTargetPools()
        {
            targetsForPlayerPool.AddRange(soEnemyCharacters);
            targetsForEnemyPool.AddRange(soPlayerCharacters);
        }

        /// <summary>
        /// Create a queue in form of List of all the characters to be used in battle
        /// </summary>
        private void CreateQueue()
        {
            // Merge playerCharacters and enemyCharacters into one array
            battleQueue = soPlayerCharacters.Concat(soEnemyCharacters).ToList();
            // Sort the battle queue by initiative
            battleQueue = battleQueue.OrderByDescending(character => character.initiative).ToList();
        }

        /// <summary>
        /// Checks if either of teams has won (all characters are dead while the other team still stands)
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
        /// Starts a whole battle consisting of multiple turns
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
        /// Makes a turn consisting of one action for each character in the battle queue
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
                    ToggleEnemyTurnPostEffects();
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    ToggleSkillButtonsVisibility(true);
                    UpdateSkillButtons(character);
                    // Wait until player does his turn and then continue
                    yield return new WaitUntil(() =>
                        gm.stateController.fsm.State == StateController.States.PlayerFinalizedHisMove);
                    Debug.Log("Player finalized his move!");
                    battleMenuActions.MakeAction(character, playerSelectedTarget, playerSelectedAbility,
                        soAllCharacters, true);
                    StartCoroutine(MakeAttackPostEffects());
                    ToggleSkillButtonsVisibility(false);
                    playerSelectedAbility = null;
                    playerSelectedTarget = null;
                }
                else
                {
                    gm.stateController.fsm.ChangeState(StateController.States.EnemyTurn);
                    ToggleEnemyTurnPostEffects();
                    yield return new WaitForSecondsRealtime(delayBetweenActions);
                    var randomTargetIndex = Random.Range(0, targetsForEnemyPool.Count);
                    var enemySelectedTarget = targetsForEnemyPool[randomTargetIndex];
                    var randomEnemyAbilityIndex = Random.Range(0, character.abilities.Length);
                    var enemySelectedAbility = character.abilities[randomEnemyAbilityIndex];
                    battleMenuActions.MakeAction(character, enemySelectedTarget, enemySelectedAbility, soAllCharacters,
                        false);
                    StartCoroutine(MakeAttackPostEffects());
                }

                DisableSelectionIndicators();
                LetPlayerChooseTarget(false);
                yield return new WaitForSecondsRealtime(delayBetweenActions);
                currentChar.GetComponent<MoveActiveCharacterToCenter>().MoveBack();
            }

            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        /// <summary>
        /// Toggles post effects that should be visible during the enemy turn
        /// </summary>
        private void ToggleEnemyTurnPostEffects()
        {
            vignette.intensity.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn
                ? enemyTurnVignetteIntensity
                : defaultVignetteIntensity);
            vignette.color.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn
                ? enemyTurnVignette
                : defaultVignette);
        }

        /// <summary>
        /// Toggles post effects that should be visible when a character makes an attack
        /// </summary>
        private IEnumerator MakeAttackPostEffects()
        {
            for (float i = 0; i < attackChromaticAberrationIntensity; i += 0.05f)
            {
                Debug.Log($"Setting chromatic aberration to: {i}, actual: {attackChromaticAberrationIntensity}");
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }

            for (var i = attackChromaticAberrationIntensity; i > 0; i -= 0.05f)
            {
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }

        /// <summary>
        /// Increment the turn counter and show it on the screen
        /// </summary>
        private void UpdateTurnCounter()
        {
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter;
        }

        /// <summary>
        /// Search through all attached character GameObjects, and return the one which DisplayCharacterData component has a Character scriptable object attached with the same name as the chosen character
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
        /// Check if all the required steps to end the turn were completed, if so then set the state to PlayerFinalizedHisMove
        /// </summary>
        public void EndPlayerTurn()
        {
            Debug.Log(
                $"Full end-turn data dump: {gm.stateController.fsm.State}, {playerSelectedAbility}, {playerSelectedTarget}, {playerSelectedTarget.isDead}, {targetsForPlayerPool}, {targetsForEnemyPool}");
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

        public void StartTargetSelectionState()
        {
            gm.stateController.fsm.ChangeState(StateController.States.SelectingTarget);
            LetPlayerChooseTarget(true);
        }

        /// <summary>
        /// Disable/enable interactability of all characters buttons
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
        /// Set all ability data to match the character which turn it is
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

        private void ToggleSkillButtonsVisibility(bool enable)
        {
            Debug.Log($"Setting visibility of abilities to {enable}");
            for (var i = 0; i <= 3; i++) skillButtons[i].gameObject.SetActive(enable);
        }

        /// <summary>
        /// Disable the indicators that show what ability and target is currently selected by the player
        /// </summary>
        private void DisableSelectionIndicators()
        {
            targetIndicator.SetActive(false);
            abilityIndicator.SetActive(false);
        }

        /// <summary>
        /// Start game end sequence
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameEnd()
        {
            gm.stateController.fsm.ChangeState(StateController.States.GameEnded);
            Debug.Log($"Game ended, player won: {playerWon}");
            if (playerWon)
                Instantiate(playerWonCanvas);
            else
                Instantiate(playerLostCanvas);
            yield return new WaitForSecondsRealtime(2.8f);
            SceneManager.LoadScene(1);
            gm.stateController.fsm.ChangeState(StateController.States.Playing);
        }
    }
}