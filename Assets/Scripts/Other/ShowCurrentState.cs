using TMPro;
using UnityEngine;

namespace Other
{
    public class ShowCurrentState : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentStateText;

        private void Start()
        {
            currentStateText.text = "CURRENT STATE: ";
        }
    }
}
