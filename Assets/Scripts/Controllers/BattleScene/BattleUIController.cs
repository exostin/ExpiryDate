using System.Collections;
using System.Linq;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Controllers.BattleScene
{
    public class BattleUIController : MonoBehaviour
    {
        [SerializeField] private GameObject[] skillButtons;
        [SerializeField] private GameObject targetIndicator;
        [SerializeField] private GameObject abilityIndicator;
        private int turnCounter;
        [SerializeField] private TMP_Text turnCounterText;
        [SerializeField] private GameObject playerWonCanvas;
        [SerializeField] private GameObject playerLostCanvas;
        private StateController stateController;
        private BattleController battleController;

        private void Start()
        {
            stateController = FindObjectOfType<StateController>();
            battleController = FindObjectOfType<BattleController>();
        }
    
        /// <summary>
        ///     Increment the turn counter and show it on the screen
        /// </summary>
        public void UpdateTurnCounter()
        {
            turnCounter++;
            turnCounterText.text = "Turn: " + turnCounter;
        }
        /// <summary>
        ///     Disable/enable interactability of all characters buttons
        /// </summary>
        /// <param name="choice">true/false</param>
        public void LetPlayerChooseTarget(bool choice)
        {
            Debug.Log($"Setting interactability with characters to: {choice}");
            foreach (var character in battleController.battleQueue)
            foreach (var g in battleController.allCharacters.Where(g =>
                         g.GetComponent<DisplayCharacterData>().character.characterName == character.characterName))
                g.GetComponent<Button>().interactable = choice;
        }

        /// <summary>
        ///     Set all ability data to match the character which turn it is
        /// </summary>
        /// <param name="currentCharacter">character which turn it currently is</param>
        public void UpdateSkillButtons(Character currentCharacter)
        {
            Debug.Log("Updating ability info");
            for (var i = 0; i <= 3; i++)
            {
                skillButtons[i].GetComponent<DisplayAbilityData>().ability = currentCharacter.abilities[i];
                skillButtons[i].GetComponent<DisplayAbilityData>().UpdateAbilityDisplay();
            }
        }

        public void ToggleAbilityButtonsVisibility(bool enable)
        {
            Debug.Log($"Setting visibility of abilities to {enable}");
            for (var i = 0; i <= 3; i++) skillButtons[i].gameObject.SetActive(enable);
        }

        /// <summary>
        ///     Disable the indicators that show what ability and target is currently selected by the player
        /// </summary>
        public void DisableSelectionIndicators()
        {
            targetIndicator.SetActive(false);
            abilityIndicator.SetActive(false);
        }

        /// <summary>
        ///     Start game end sequence
        /// </summary>
        /// <returns></returns>
        public IEnumerator GameEnd()
        {
            stateController.fsm.ChangeState(StateController.States.GameEnded);
            Debug.Log($"Game ended, player won: {battleController.PlayerWon}");
            Instantiate(battleController.PlayerWon ? playerWonCanvas : playerLostCanvas);
            yield return new WaitForSecondsRealtime(2.8f);
            SceneManager.LoadScene(1);
            stateController.fsm.ChangeState(StateController.States.Playing);
        }
        
        #region Functions invoked only by buttons inside the UI

        public void StartTargetSelectionState()
        {
            stateController.fsm.ChangeState(StateController.States.SelectingTarget);
            LetPlayerChooseTarget(true);
        }

        #endregion
    }
}
