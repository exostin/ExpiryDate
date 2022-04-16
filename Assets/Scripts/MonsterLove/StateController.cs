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

    // should be private
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
        yield return new WaitForSecondsRealtime(1f);

        fsm.ChangeState(States.Playing);
    }

    private void Playing_Enter()
    {
    }

    private void Playing_Update()
    {
    }

    private void Play_Exit()
    {
        Debug.Log("Game Over");
    }

    private void Lose_Enter()
    {
        Debug.Log("Lost");
    }

    private void Win_Enter()
    {
        Debug.Log("Won");
    }

    private void Pause_Enter()
    {
        Time.timeScale = 0f;
    }

    private void Pause_Exit()
    {
        Time.timeScale = 1f;
    }

    private void PlayerTurn_Enter()
    {
        Time.timeScale = 0f;
    }
}