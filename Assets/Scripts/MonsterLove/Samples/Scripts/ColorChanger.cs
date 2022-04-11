using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public enum States
    {
        Blue,
        Green,
        Red
    }

    public float interval;

    private Camera cam;

    private StateMachine<States> fsm;

    private void Awake()
    {
        cam = Camera.main;

        fsm = StateMachine<States>.Initialize(this);
    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 150, 50, 100, 20), "Disco Time!"))
            fsm.ChangeState(States.Blue, StateTransition.Overwrite);
    }

    private IEnumerator Blue_Enter()
    {
        cam.backgroundColor = Color.blue * 0.5f;
        yield return new WaitForSeconds(interval);

        fsm.ChangeState(States.Red);
    }

    private IEnumerator Red_Enter()
    {
        cam.backgroundColor = Color.red * 0.5f;
        yield return new WaitForSeconds(interval);

        fsm.ChangeState(States.Green);
    }

    private IEnumerator Green_Enter()
    {
        cam.backgroundColor = Color.green * 0.5f;
        yield return new WaitForSeconds(interval);

        fsm.ChangeState(States.Blue);
    }
}