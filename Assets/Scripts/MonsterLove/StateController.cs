using System.Collections;
using MonsterLove.StateMachine;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public enum States
    {
        Playing,
        Pause,
        PlayerTurn,
        PlayerFinalizedHisMove,
        EnemyTurn,
        Countdown,
        ReadyForNextTurn,
        SelectingTarget,
        GameEnded
    }

    public StateMachine<States, StateDriverUnity> fsm;

    private void Awake()
    {
        //Initialize State Machine Engine		
        fsm = new StateMachine<States, StateDriverUnity>(this);
        fsm.ChangeState(States.Playing);
    }

    private void Update()
    {
        fsm.Driver.Update.Invoke();
    }

    //We can return a coroutine, this is useful animations and the like
    private IEnumerator Countdown_Enter()
    {
        Debug.Log("Entering Countdown state");
        yield return new WaitForSecondsRealtime(1f);

        fsm.ChangeState(States.Playing);
    }

    private void Playing_Enter()
    {
        Debug.Log("Entering Playing state");
    }

    private void Playing_Update()
    {
    }

    private void Play_Exit()
    {
        Debug.Log("Exiting Play state");
    }

    private void Lose_Enter()
    {
        Debug.Log("Entering Lose state");
    }

    private void Win_Enter()
    {
        Debug.Log("Entering Win state");
    }

    private void Pause_Enter()
    {
        Debug.Log("Entering Pause state");
        Time.timeScale = 0f;
    }

    private void Pause_Exit()
    {
        Debug.Log("Exiting Pause state");
        Time.timeScale = 1f;
    }

    private void PlayerTurn_Enter()
    {
        Debug.Log("Entering player turn state");
    }
}