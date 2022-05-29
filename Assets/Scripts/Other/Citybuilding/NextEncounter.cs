using System;
using Classes.Citybuilding;
using Controllers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Other.Citybuilding
{
    public class NextEncounter : MonoBehaviour
    {
        private void Start()
        {
            var slider = gameObject.GetComponent<Slider>();
            var cbm = FindObjectOfType<GameManager>().cbm;

            slider.value = cbm.NextEncounter;
            slider.minValue = 0;
            slider.maxValue = cbm.NextEncounterMax;
            slider.transform.Find("Label").GetComponent<TMP_Text>().text = $"{cbm.NextEncounter} days left!";
        }
    }
}