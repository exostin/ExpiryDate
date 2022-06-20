using System.Linq;
using DisplayObjectData;
using Other.Enums;
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
        [SerializeField] private TMP_Text characterInspectorHeading;
        [SerializeField] private Image smallCharacterArtwork;

        #region Stats
        
        [SerializeField] private TMP_Text characterInspectorText;

        #endregion
        
        private Character currentCharacter;
        #endregion

        #region Ability inspector
        [Header("Ability inspector")]
        [SerializeField] private GameObject abilityInspector;
        [SerializeField] private TMP_Text abilityInspectorHeader;
        [SerializeField] private TMP_Text abilityDescription;

        #endregion
        private BattleController battleController;
        

        void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            DisplayCharacterData.OnHoveredOverCharacter += UpdateBasicCharacterData;
            BattleController.OnActionMade += LiveUpdateCharacterData;
            BattleController.OnStatusHandled += LiveUpdateCharacterData;
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
            if (currentCharacter is null)
            {
                characterInspector.SetActive(false);
                return;
            }
            
            characterInspector.SetActive(true);
            
            smallCharacterArtwork.sprite = currentCharacter.artworkPortrait;
            characterInspectorHeading.text = "Active statuses:";

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
                    activeStatusesList += status.ToString();
                    switch (status)
                    {
                        case StatusType.Bleed:
                            activeStatusesList += $" for {currentCharacter.CumulatedBleedDmg} dmg, {currentCharacter.BleedDurationLeft} turns left";
                            break;
                        case StatusType.Stun:
                            activeStatusesList += $", {currentCharacter.StunDurationLeft} turns left";
                            break;
                    }

                    activeStatusesList += "\n";
                }
            }
            else
            {
                activeStatusesList = "None";
            }
            
            characterInspectorText.text = $"{activeStatusesList}";
        }

        #endregion

        #region Ability-related methods

        private void UpdateAbilityData()
        {
            var currentAbility = battleController.PlayerSelectedAbility;
            abilityInspectorHeader.text = "Ability description:";
            abilityDescription.text = null;
            switch (currentAbility.abilityType)
            {
                case AbilityType.DamageOnly:
                case AbilityType.Status when currentAbility.statusType is StatusType.Bleed or StatusType.Stun:
                    abilityDescription.text += $"Damage: {currentAbility.minDamageAmount} - {currentAbility.maxDamageAmount}\n";
                    break;
                case AbilityType.Heal:
                    abilityDescription.text += $"Heal: {currentAbility.minHealAmount} - {currentAbility.maxHealAmount} (also alleviates bleeding)\n";
                    break;
                case AbilityType.Shield:
                    abilityDescription.text += $"Shield: {currentAbility.shieldAmount}\n";
                    break;
            }
            
            if (currentAbility.abilityType is AbilityType.Status)
            {
                abilityDescription.text += $"Applies {currentAbility.statusType}";
                switch (currentAbility.statusType)
                {
                    case StatusType.Bleed:
                        abilityDescription.text += $" for {currentAbility.bleedDuration} turns\n";
                        break;
                    case StatusType.Stun:
                        abilityDescription.text += $" for {currentAbility.stunDuration} turns\n";
                        break;
                    case StatusType.Dodge:
                        abilityDescription.text += " (avoid being targeted until next turn)\n";
                        break;
                }
            }
            abilityDescription.text += $"\n{currentAbility.abilityDescription}";
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
