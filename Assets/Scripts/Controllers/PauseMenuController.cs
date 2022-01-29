using System;
using System.Runtime.CompilerServices;
using Controllers;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void Unpause()
    {
        gm.stateController.fsm.ChangeState(StateController.States.Playing);
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
