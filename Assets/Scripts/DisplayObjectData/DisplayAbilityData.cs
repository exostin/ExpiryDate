using System;
using Controllers.BattleScene;
using Other.Enums;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DisplayObjectData
{
    public class DisplayAbilityData : MonoBehaviour
    {
        public Ability ability;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private TMP_Text damageTextContainer;

        private BattleController battleController;
        
        public static event Action OnAbilitySelected;

        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
        }

        // Update information shown on screen to reflect the currently selected ability
        public void UpdateAbilityDisplay()
        {
            nameTextContainer.text = ability.abilityName;
            image.sprite = ability.artwork;

            if (ability.abilityType == AbilityType.Heal)
            {
                damageTextContainer.color = new Color(0.38f, 0.93f, 0.42f);
                damageTextContainer.text = $"Heal";
            }
            else if (ability.abilityType == AbilityType.Status)
            {
                damageTextContainer.color = new Color(0.94f, 0.93f, 0.42f);
                damageTextContainer.text = ability.statusType.ToString();
            }
            else if (ability.abilityType is AbilityType.DamageOnly)
            {
                damageTextContainer.color = new Color(0.84f, 0.18f, 0.22f);
                damageTextContainer.text = $"DMG";
            }
            else if (ability.abilityType is AbilityType.Shield)
            {
                damageTextContainer.color = new Color(0.12f, 0.42f, 1f);
                damageTextContainer.text = $"Shield";
            }

            if (ability.abilityTarget is TargetType.MultipleEnemies or TargetType.MultipleTeammates) damageTextContainer.text += " (AOE)";
            if (ability.abilityTarget is TargetType.SelfOnly) damageTextContainer.text += " (Self)";
        }

        public void SelectAsAbilityForUse()
        {
            Debug.Log($"Selected: {ability.name}");
            battleController.PlayerSelectedAbility = ability;
            OnAbilitySelected?.Invoke();
        }

    }
}