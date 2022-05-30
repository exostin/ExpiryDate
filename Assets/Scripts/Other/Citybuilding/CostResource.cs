using System;
using TMPro;
using UnityEngine;

namespace Other.Citybuilding
{
    public class CostResource : MonoBehaviour
    {
        public int Amount;
        public bool IsMissing;

        [SerializeField] private TMP_Text label;

        private void Update()
        {
            label.text = Amount.ToString();
            label.color = IsMissing ? Color.red : Color.black;
        }
    }
}