using Controllers;
using Controllers.BattleScene;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DisplayObjectData
{
    public class DisplayCharacterData : MonoBehaviour
    {
        public Character character;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private TMP_Text healthTextContainer;
        [SerializeField] private Slider hpSlider;

        [SerializeField] private BattleMenuController battleMenuController;

        private void Start()
        {
            hpSlider.maxValue = character.maxHealth;
            hpSlider.value = character.maxHealth;
            nameTextContainer.text = character.characterName;
            image.sprite = character.artwork;
        }

        private void Update()
        {
            healthTextContainer.text = character.health + " " + "HP";
            hpSlider.value = character.health;
        }

        public void SelectAsATarget()
        {
            Debug.Log($"Target selected: {character.name}, ending turn");
            battleMenuController.playerSelectedTarget = character;
            battleMenuController.EndPlayerTurn();
        }
    }
}