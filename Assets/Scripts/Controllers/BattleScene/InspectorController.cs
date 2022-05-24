using DisplayObjectData;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers.BattleScene
{
    public class InspectorController : MonoBehaviour
    {
        [SerializeField] private Slider inspectorHPSlider;
        [SerializeField] private TMP_Text inspectorHPSliderText;
        [SerializeField] private TMP_Text inspectorCharacterName;
        private BattleController battleController;
        private Character currentCharacter;

        void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            DisplayCharacterData.OnHoveredOverCharacter += UpdateInspector;
        }

        private void UpdateInspector()
        {
            currentCharacter = battleController.PlayerHoveredOverTarget;
            
            inspectorHPSlider.maxValue = currentCharacter.maxHealth;
            inspectorHPSlider.value = currentCharacter.health;
            inspectorHPSliderText.text = $"{currentCharacter.health} HP";
            
            inspectorCharacterName.text = currentCharacter.name;
        }
    }
}
