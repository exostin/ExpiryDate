using System;
using Controllers.BattleScene;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DisplayObjectData
{
    public class DisplayCharacterData : MonoBehaviour, IPointerEnterHandler
    {
        public Character character;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private Slider hpSlider;
        [SerializeField] private Slider shieldSlider;

        private BattleController battleController;
        
        public delegate void SelectedCharacter();
        public static event SelectedCharacter OnHoveredOverCharacter;
        
        public delegate void TurnAction();
        public static event TurnAction OnTurnEnd;

        public void Initialize()
        {
            battleController = FindObjectOfType<BattleController>();
            nameTextContainer.text = character.characterName;
            image.sprite = character.artwork;
            hpSlider.maxValue = character.maxHealth;
            hpSlider.value = character.maxHealth;
            shieldSlider.maxValue = character.maxShield;
            shieldSlider.value = character.ShieldPoints;

            BattleController.OnActionMade += UpdateCurrentHpAndShield;
            BattleController.OnStatusHandled += UpdateCurrentHpAndShield;
            Character.OnCharacterDeath += VisualizeDeathOnDeadCharacters;
        }

        private void UpdateCurrentHpAndShield()
        {
            hpSlider.value = character.Health;
            shieldSlider.value = character.ShieldPoints;
            if (character.Health == 0)
            {
                BattleController.OnActionMade -= UpdateCurrentHpAndShield;
                BattleController.OnStatusHandled -= UpdateCurrentHpAndShield;
            }
        }
        
        public void SelectAsATarget()
        {
            Debug.Log($"Target selected: {character.name}, ending turn");
            battleController.PlayerSelectedTarget = character;
            OnTurnEnd?.Invoke();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            battleController.PlayerHoveredOverTarget = character;
            ActivateOnHoveredEvent();
        }

        public static void ActivateOnHoveredEvent()
        {
            OnHoveredOverCharacter?.Invoke();
        }

        // It's expensive, but we've got no time left, to do it well ☠️
        public void VisualizeDeathOnDeadCharacters()
        {
            if (character.IsDead) image.color = battleController.deadCharacterTint;
        }
    }
}