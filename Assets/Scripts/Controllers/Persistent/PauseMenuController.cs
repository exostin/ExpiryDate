using Controllers;
using UnityEditor;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
#pragma warning disable CS0414
    private readonly string webPlayerQuitURL = "https://www.google.com";
#pragma warning restore CS0414
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
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webPlayerQuitURL);
#else
        Application.Quit();
#endif
    }
}