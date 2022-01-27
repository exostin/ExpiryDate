using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DisplayObjectData
{
    public class DisplayAbilityData : MonoBehaviour
    {
        [SerializeField] private Ability ability;

        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameTextContainer;
        [SerializeField] private TMP_Text damageTextContainer;

        public void UpdateAbilityDisplay()
        {
            nameTextContainer.text = ability.abilityName;
            image.sprite = ability.artwork;
            damageTextContainer.text = ability.damage.ToString() + " DMG";
        }
        // ONLY TEMPORARY (FOR TESTING)
        public void Start()
        {
            nameTextContainer.text = ability.abilityName;
            image.sprite = ability.artwork;
            damageTextContainer.text = ability.damage.ToString() + " DMG";
        }
        // ---
    }
}
