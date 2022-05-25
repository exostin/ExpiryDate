using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.BattleScene
{
    public class InspectorController : MonoBehaviour
    {
        private BattleController battleController;


        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            DisplayCharacterData.OnHoveredOverCharacter += UpdateCharacterData;
            BattleController.OnActionMade += UpdateCharacterCurrentHp;
            BattleController.OnActionMade += HideAbilityData;
            DisplayAbilityData.OnAbilitySelected += UpdateAbilityData;
            DisplayAbilityData.OnAbilitySelected += ShowAbilityData;

            HideAbilityData();
            characterInspector.SetActive(false);
        }

        #region Character inspector

        [Header("Character inspector")] [SerializeField]
        private GameObject characterInspector;

        [SerializeField] private Slider hpSlider;
        [SerializeField] private TMP_Text hpSliderText;
        [SerializeField] private TMP_Text characterName;
        [SerializeField] private Image smallCharacterArtwork;

        #region Stats

        [SerializeField] private TMP_Text initiative;

        #endregion

        private Character currentCharacter;

        #endregion

        #region Ability inspector

        [Header("Ability inspector")] [SerializeField]
        private GameObject abilityInspector;

        [SerializeField] private TMP_Text abilityName;
        [SerializeField] private TMP_Text abilityDescription;

        #endregion

        #region Character-related methods

        private void UpdateCharacterData()
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
            initiative.text = $"Initiative: {currentCharacter.initiative}";

            hpSlider.maxValue = currentCharacter.maxHealth;
            UpdateCharacterCurrentHp();
        }

        private void UpdateCharacterCurrentHp()
        {
            if (!characterInspector.gameObject.activeSelf) return;
            hpSlider.value = currentCharacter.health;
            hpSliderText.text = $"{currentCharacter.health} HP";
        }

        #endregion

        #region Ability-related methods

        private void UpdateAbilityData()
        {
            abilityName.text = battleController.PlayerSelectedAbility.abilityName;
            abilityDescription.text = battleController.PlayerSelectedAbility.description;
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