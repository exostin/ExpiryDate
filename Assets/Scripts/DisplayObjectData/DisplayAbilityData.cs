using Controllers.BattleScene;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DisplayObjectData
{
    public class DisplayAbilityData : MonoBehaviour
    {
        public delegate void AbilityEvent();

        public Ability ability;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private TMP_Text damageTextContainer;

        private BattleController battleController;

        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
        }

        public static event AbilityEvent OnAbilitySelected;

        // Update information shown on screen to reflect the currently selected ability
        public void UpdateAbilityDisplay()
        {
            nameTextContainer.text = ability.abilityName;
            image.sprite = ability.artwork;

            if (ability.damage < 0)
            {
                damageTextContainer.color = Color.green;
                damageTextContainer.text = $"Heal {-ability.damage}";
            }
            else if (ability.damage == 0)
            {
                damageTextContainer.color = Color.yellow;
                damageTextContainer.text = "Buff";
            }
            else
            {
                damageTextContainer.color = Color.red;
                damageTextContainer.text = ability.damage + " DMG";
            }

            if (ability.targetsWholeTeam) damageTextContainer.text += " (All)";
            if (ability.usedOnlyOnSelf) damageTextContainer.text += " (Self)";
        }

        public void SelectAsAbilityForUse()
        {
            Debug.Log($"Selected: {ability.name}");
            battleController.PlayerSelectedAbility = ability;
            OnAbilitySelected?.Invoke();
        }
    }
}