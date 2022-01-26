using System.Runtime.CompilerServices;
using Controllers;
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
        [SerializeField] private TMP_Text textContainer;
        
        void Start()
        {
            textContainer.text = character.characterName;
        }
    }
}
