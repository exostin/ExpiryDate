using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterScripts
{
    public class DisplayCharacterData : MonoBehaviour
    {
        [SerializeField] private Character character;

        [SerializeField] private RawImage image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private TMP_Text healthTextContainer;
        
        void Start()
        {
            nameTextContainer.text = character.characterName;
        }

        private void Update()
        {
            healthTextContainer.text = character.health.ToString() + " " + "HP";
        }
    }
}
