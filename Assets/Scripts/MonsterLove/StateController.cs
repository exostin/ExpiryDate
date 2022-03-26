using UnityEngine;
using System.Collections;
using MonsterLove.StateMachine;

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
		SelectingTarget
	}
	// should be private
	public StateMachine<States, StateDriverUnity> fsm;

	private void Awake()
	{
		//Initialize State Machine Engine		
		fsm = new StateMachine<States, StateDriverUnity>(this);
		fsm.ChangeState(States.Playing);
	}

	void Update()
	{
		fsm.Driver.Update.Invoke();
	}

	//We can return a coroutine, this is useful animations and the like
	IEnumerator Countdown_Enter()
	{
		yield return new WaitForSecondsRealtime(1f);

		fsm.ChangeState(States.Playing);
	}
	
	void Playing_Enter()
	{
		
	}

	void Playing_Update()
	{
		
	}

	void Play_Exit()
	{
		Debug.Log("Game Over");
	}

	void Lose_Enter()
	{
		Debug.Log("Lost");
	}
	
	void Win_Enter()
	{
		Debug.Log("Won");
	}

	void Pause_Enter()
	{
		Time.timeScale = 0f;
	}

	void Pause_Exit()
	{
		Time.timeScale = 1f;
	}

	void PlayerTurn_Enter()
	{
		Time.timeScale = 0f;
	}
}
