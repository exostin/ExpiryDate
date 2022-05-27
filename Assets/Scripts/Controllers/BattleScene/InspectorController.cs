using System.Linq;
using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.BattleScene
{
    public class InspectorController : MonoBehaviour
    {
        #region Character inspector
        [Header("Character inspector")]
        [SerializeField] private GameObject characterInspector;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private TMP_Text hpSliderText;
        [SerializeField] private Slider shieldSlider;
        [SerializeField] private TMP_Text shieldSliderText;
        [SerializeField] private TMP_Text characterName;
        [SerializeField] private Image smallCharacterArtwork;

        #region Stats
        
        [SerializeField] private TMP_Text initiative;

        #endregion
        
        private Character currentCharacter;
        #endregion

        #region Ability inspector
        [Header("Ability inspector")]
        [SerializeField] private GameObject abilityInspector;
        [SerializeField] private TMP_Text abilityName;
        [SerializeField] private TMP_Text abilityDescription;

        #endregion
        private BattleController battleController;
        

        void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            DisplayCharacterData.OnHoveredOverCharacter += UpdateBasicCharacterData;
            BattleController.OnActionMade += LiveUpdateCharacterData;
            BattleController.OnActionMade += HideAbilityData;
            DisplayAbilityData.OnAbilitySelected += UpdateAbilityData;
            DisplayAbilityData.OnAbilitySelected += ShowAbilityData;
            
            HideAbilityData();
            characterInspector.SetActive(false);
        }

        #region Character-related methods

        /// <summary>
        /// Used after switching characters
        /// </summary>
        private void UpdateBasicCharacterData()
        {
            currentCharacter = battleController.PlayerHoveredOverTarget;
            if (currentCharacter == null)
            {
                characterInspector.SetActive(false);
                return;
            }
            
            characterInspector.SetActive(true);
            
            smallCharacterArtwork.sprite = currentCharacter.artwork;
            characterName.text = currentCharacter.name;

            hpSlider.maxValue = currentCharacter.maxHealth;
            shieldSlider.maxValue = currentCharacter.maxShield;
            LiveUpdateCharacterData();
        }

        /// <summary>
        /// Used to update the stats, that are often changed during the battle
        /// </summary>
        private void LiveUpdateCharacterData()
        {
            if (!characterInspector.gameObject.activeSelf) return;
            hpSlider.value = currentCharacter.Health;
            hpSliderText.text = $"HP: {currentCharacter.Health}/{currentCharacter.maxHealth}";
            shieldSlider.value = currentCharacter.ShieldPoints;
            shieldSliderText.text = $"Shield: {currentCharacter.ShieldPoints}/{currentCharacter.maxShield}";
            UpdateStatuses();
        }

        private void UpdateStatuses()
        {
            string activeStatusesList = null;
            if (currentCharacter.currentlyAppliedStatuses.Any())
            {
                foreach (var status in currentCharacter.currentlyAppliedStatuses)
                {
                    activeStatusesList += status + "\n";
                }
            }
            else
            {
                activeStatusesList = "None";
            }
            
            initiative.text = $"Initiative: {currentCharacter.initiative}\nActive statuses: {activeStatusesList}";
        }

        #endregion

        #region Ability-related methods

        private void UpdateAbilityData()
        {
            abilityName.text = battleController.PlayerSelectedAbility.abilityName;
            abilityDescription.text = battleController.PlayerSelectedAbility.abilityDescription;
        }

        private void ShowAbilityData()
        {
            abilityInspector.SetActive(true);
        }
        private void HideAbilityData()
        {
            abilityInspector.SetActive(false);
        }

        #endregion
        
    }
}
