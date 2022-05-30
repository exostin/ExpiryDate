using System;
using Classes;
using Controllers.BattleScene;
using Other.Enums;
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

        [Header("Status visual cues")]
        [SerializeField] private GameObject bleed;
        [SerializeField] private GameObject stun;
        [SerializeField] private GameObject dodge;
        
        public void Initialize()
        {
            battleController = FindObjectOfType<BattleController>();
            
            // It's expensive, but we've got no time left, to do it well ☠️
            BattleActions.OnBleedApplied += ShowBleed;
            BattleActions.OnStunApplied += ShowStun;
            BattleActions.OnDodgeApplied += ShowDodge;
            BattleActions.OnHealAppliedToCureBleed += HideBleed;
            StatusHandler.OnBleedRemoved += HideBleed;
            StatusHandler.OnStunRemoved += HideStun;
            StatusHandler.OnDodgeRemoved += HideDodge;
            
            nameTextContainer.text = character.characterName;
            image.sprite = character.artwork;
            hpSlider.maxValue = character.maxHealth;
            hpSlider.value = character.maxHealth;
            shieldSlider.maxValue = character.maxShield;
            shieldSlider.value = character.ShieldPoints;

            BattleController.OnActionMade += UpdateCurrentHpAndShield;
            BattleController.OnStatusHandled += UpdateCurrentHpAndShield;
            Character.OnCharacterDeath += VisualizeDeathOnDeadCharacters;
            Character.OnCharacterDeath += UnsubscribeFromStatusVisualQuesIfDead;
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
        private void VisualizeDeathOnDeadCharacters()
        {
            if (character.IsDead) image.color = battleController.deadCharacterTint;
        }

        private void UnsubscribeFromStatusVisualQuesIfDead()
        {
            if (!character.IsDead) return;
            BattleActions.OnBleedApplied -= ShowBleed;
            BattleActions.OnStunApplied -= ShowStun;
            BattleActions.OnDodgeApplied -= ShowDodge;
            StatusHandler.OnBleedRemoved -= HideBleed;
            StatusHandler.OnStunRemoved -= HideStun;
            StatusHandler.OnDodgeRemoved -= HideDodge;
        }
        
        private void ShowBleed()
        {
            if (!character.currentlyAppliedStatuses.Contains(StatusType.Bleed)) return;
            if (bleed.activeSelf) return;
            bleed.SetActive(true);
        }
        private void ShowStun()
        {
            if (!character.currentlyAppliedStatuses.Contains(StatusType.Stun)) return;
            if (stun.activeSelf) return;
            stun.SetActive(true);
        }
        private void ShowDodge()
        {
            if (!character.currentlyAppliedStatuses.Contains(StatusType.Dodge)) return;
            if (dodge.activeSelf) return;
            dodge.SetActive(true);
        }
        
        private void HideBleed()
        {
            if (character.currentlyAppliedStatuses.Contains(StatusType.Bleed)) return;
            bleed.SetActive(false);
        }
        
        private void HideStun()
        {
            if (character.currentlyAppliedStatuses.Contains(StatusType.Stun)) return;
            stun.SetActive(false);
        }
        
        private void HideDodge()
        {
            if (character.currentlyAppliedStatuses.Contains(StatusType.Dodge)) return;
            dodge.SetActive(false);
        }
    }
}