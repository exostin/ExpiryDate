using Controllers;
using TMPro;
using UnityEngine;

namespace Other.GameOver
{
    public class Label : MonoBehaviour
    {
        void Start()
        {
            var gm = FindObjectOfType<GameManager>();
            var daysSurvived = gm is null ? 13 : gm.cbm.DaysSurvived;
            gameObject.GetComponent<TMP_Text>().text = $"You survived {daysSurvived} days...";
        }
    }
}
