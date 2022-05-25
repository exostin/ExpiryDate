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
        public delegate void SelectedCharacter();

        public delegate void TurnAction();

        public Character character;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private Slider hpSlider;


        private BattleController battleController;

        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            nameTextContainer.text = character.characterName;
            image.sprite = character.artwork;
            hpSlider.maxValue = character.maxHealth;
            hpSlider.value = character.maxHealth;

            BattleController.OnActionMade += UpdateCurrentHp;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log($"Mouse hovered over: {character.name}");
            battleController.PlayerHoveredOverTarget = character;
            OnHoveredOverCharacter?.Invoke();
        }

        public static event SelectedCharacter OnHoveredOverCharacter;
        public static event TurnAction OnTurnEnd;

        private void UpdateCurrentHp()
        {
            hpSlider.value = character.health;
            if (character.health == 0) BattleController.OnActionMade -= UpdateCurrentHp;
        }

        public void SelectAsATarget()
        {
            Debug.Log($"Target selected: {character.name}, ending turn");
            battleController.PlayerSelectedTarget = character;
            OnTurnEnd?.Invoke();
        }
    }
}