using System;
using Classes;
using UnityEngine;

namespace Other.DefenderSelection
{
    public class Container : MonoBehaviour
    {
        [SerializeField] private GameObject defenderCardPrefab;
        private SimplifiedDefender[] items;

        public SimplifiedDefender[] Items
        {
            set
            {
                items = value;
                MakeUI();
            }
        }

        private void MakeUI()
        {
            // Remove all children
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            // Place new prefabs
            foreach (var item in items)
            {
                var go = Instantiate(defenderCardPrefab, transform);
                var defenderCard = go.GetComponent<DefenderCard>();
                defenderCard.Defender = item;
                defenderCard.OnClicked += DefenderCardOnOnClicked;
            }
        }
        
        public event Action<DefenderType> OnDefenderCardClicked;
        
        private void DefenderCardOnOnClicked(DefenderType obj)
        {
            OnDefenderCardClicked?.Invoke(obj);
        }
    }
}
