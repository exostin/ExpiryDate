using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers
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
        private readonly BattleMenuEnemy enemy = new();
        private readonly BattleMenuPlayer player = new();
        private readonly List<Character> targetsForEnemyPool = new();
        private readonly List<Character> targetsForPlayerPool = new();
        private List<GameObject> allCharacters;
        private List<Character> battleQueue = new();
        private GameManager gm;
        private int turnCounter;
        [SerializeField] private Volume postprocessingVolume;
        private UnityEngine.Rendering.Universal.Vignette vignette;
        private UnityEngine.Rendering.Universal.ChromaticAberration chromaticAberration;
        [SerializeField] private float defaultVignetteIntensity;
        [SerializeField] private float enemyTurnVignetteIntensity;
        [SerializeField] private float attackChromaticAberrationIntensity = 0.5f;
        [SerializeField] private Color defaultVignete;
        [SerializeField] private Color enemyTurnVignete;

        private void Start()
        {
            gm = FindObjectOfType<GameManager>();
            
            VolumeProfile postprocessingVolumeProfile = postprocessingVolume.GetComponent<Volume>()?.profile;
            if(!postprocessingVolumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));
            if(!postprocessingVolumeProfile.TryGet(out vignette)) throw new System.NullReferenceException(nameof(vignette));
            if(!postprocessingVolumeProfile.TryGet(out chromaticAberration)) throw new System.NullReferenceException(nameof(chromaticAberration));

            allCharacters = playerCharacters.Concat(enemyCharacters).ToList();

            ExtractCharactersData();

            CreateTargetPools();
            CreateQueue();

            LetPlayerChooseTarget(false);
            ToggleSkillButtonsVisibility(false);

            StartCoroutine(PlayBattle());
        }

        /// <summary>
        /// Extract character scriptable objects data from their game objects
        /// </summary>
        private void ExtractCharactersData()
        {
            foreach (var g in playerCharacters)
                soPlayerCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
            foreach (var g in enemyCharacters) soEnemyCharacters.Add(g.GetComponent<DisplayCharacterData>().character);
        }

        private void CreateTargetPools()
        {
            targetsForPlayerPool.AddRange(soEnemyCharacters);
            targetsForEnemyPool.AddRange(soPlayerCharacters);
        }

        private void CreateQueue()
        {
            // Merge playerCharacters and enemyCharacters into one array
            battleQueue = soPlayerCharacters.Concat(soEnemyCharacters).ToList();
            // Sort the battle queue by initiative
            battleQueue = battleQueue.OrderByDescending(character => character.initiative).ToList();
        }

        private bool CheckIfAnySideWon()
        {
            // returns `true` if any player character is alive while all enemies are dead OR if all player characters are dead while any enemy is alive
            return soPlayerCharacters.Any(character => character.health > 0) &&
                   soEnemyCharacters.All(character => character.health <= 0)
                   || soPlayerCharacters.All(character => character.health <= 0) &&
                   soEnemyCharacters.Any(character => character.health > 0);
        }

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
                    player.MakeAttack(character, playerSelectedTarget, playerSelectedAbility);
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
                    enemy.MakeAttack(character, targetsForEnemyPool[randomTargetIndex]);
                    StartCoroutine(MakeAttackPostEffects());
                }

                DisableSelectionIndicators();
                LetPlayerChooseTarget(false);
                yield return new WaitForSecondsRealtime(delayBetweenActions);
                currentChar.GetComponent<MoveActiveCharacterToCenter>().MoveBack();
            }

            gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        private void ToggleEnemyTurnPostEffects()
        {
            vignette.intensity.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn ? enemyTurnVignetteIntensity : defaultVignetteIntensity);
            vignette.color.Override(gm.stateController.fsm.State == StateController.States.EnemyTurn ? enemyTurnVignete : defaultVignete);
        }

        private IEnumerator MakeAttackPostEffects()
        {
            for (float i = 0; i < attackChromaticAberrationIntensity; i += 0.05f)
            {
                Debug.Log($"Setting chromatic aberration to: {i}, actual: {attackChromaticAberrationIntensity}");
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }
            for(var i = attackChromaticAberrationIntensity; i > 0; i -= 0.05f)
            {
                chromaticAberration.intensity.Override(i);
                yield return new WaitForSecondsRealtime(0.02f);
            }
        }

        private void UpdateTurnCounter()
        {
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter;
        }

        private GameObject FindCharactersGameObjectByName(Character character)
        {
            // search through all character GameObjects, and return the one which DisplayCharacterData Character has the same name as the chosen character
            // in short - find characters corresponding GameObject
            foreach (var g in allCharacters.Where(g =>
                         g.GetComponent<DisplayCharacterData>().character.characterName == character.characterName))
                return g;
            // if nothing could be found:
            Debug.Log($"Could not find that character! FindGameObjectWithMatchingName({character.characterName})");
            return null;
        }

        public void EndPlayerTurn()
        {
            if (gm.stateController.fsm.State == StateController.States.SelectingTarget &&
                playerSelectedAbility != null && playerSelectedTarget != null && !playerSelectedTarget.isDead)
            {
                if (targetsForPlayerPool.Contains(playerSelectedTarget) && !playerSelectedAbility.canOnlyTargetAllies)
                {
                    Debug.Log("Attacking enemy, setting state to PlayerFinalizedHisMove");
                    gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
                }
                else
                {
                    if (playerSelectedTarget.isOwnedByPlayer && playerSelectedAbility.canOnlyTargetAllies)
                    {
                        Debug.Log(
                            "Targeting an alive ally with a supportive ability, setting state to PlayerFinalizedHisMove");
                        gm.stateController.fsm.ChangeState(StateController.States.PlayerFinalizedHisMove);
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
        ///     Disable/Enable interactability of all characters buttons
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

        private void DisableSelectionIndicators()
        {
            targetIndicator.SetActive(false);
            abilityIndicator.SetActive(false);
        }

        private IEnumerator GameEnd()
        {
            Debug.Log("Game ended!");
            gm.stateController.fsm.ChangeState(StateController.States.Playing);
            yield return new WaitForSecondsRealtime(2f);
            SceneManager.LoadScene(1);
        }
    }
}