using Controllers;
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
        [SerializeField] private BattleMenuController battleMenuController;

        public void UpdateAbilityDisplay()
        {
            nameTextContainer.text = ability.abilityName;
            image.sprite = ability.artwork;
            damageTextContainer.text = ability.damage + " DMG";
        }

        public void SelectAsAbilityForUse()
        {
            Debug.Log($"Selected: {ability.name}");
            battleMenuController.playerSelectedAbility = ability;
        }
    }
}