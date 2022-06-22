using Classes;
using Controllers;
using TMPro;
using UnityEngine;

namespace Other
{
    public class PlaceholderDefenderAmountLabel : MonoBehaviour
    {
        [SerializeField] private DefenderType defenderType;

        private GameManager gm;
        private TextMeshProUGUI textComponent;
    
        void Start()
        {
            gm = FindObjectOfType<GameManager>();
            textComponent = gameObject.GetComponent<TextMeshProUGUI>();
        }
    
        void Update()
        {
            textComponent.text = gm.cbm.Defenders[defenderType].Amount.ToString();
        }
    }
}
