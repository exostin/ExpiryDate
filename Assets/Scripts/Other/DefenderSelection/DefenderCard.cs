using System;
using System.Collections.Generic;
using Classes;
using ScriptableObjects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Other.DefenderSelection
{
    public class DefenderCard : MonoBehaviour
    {
        public SimplifiedDefender Defender;

        private void Start()
        {
            if (Defender is null) throw new NullReferenceException();
            if (Defender.Count <= 0) gameObject.SetActive(false);
            
            var nameText = transform.Find("Name").GetComponent<TMP_Text>();

            nameText.text = Defender.Name;
        }
        
        public event Action<DefenderType> OnClicked;
        
        public void OnClick()
        {
            OnClicked?.Invoke(Defender.DefenderType);
        }
    }
}
