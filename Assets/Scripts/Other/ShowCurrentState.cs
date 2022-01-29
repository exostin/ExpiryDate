using System;
using TMPro;
using UnityEngine;

namespace Other
{
    public class ShowCurrentState : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI currentStateText;
        // Definitely could be done better, but the milestone is approaching and I had to do it ad hoc
        private GameObject stateController;

        private void Start()
        {
            stateController = GameObject.FindGameObjectWithTag("StateController");
        }

        private void Update()
        {
            currentStateText.text = "CURRENT STATE: " + stateController.GetComponent<StateController>().fsm.State.ToString();
        }
    }
}
