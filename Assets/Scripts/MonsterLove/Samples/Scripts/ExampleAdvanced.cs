using System.Collections.Generic;
using MonsterLove.StateMachine;
using UnityEngine;

public class ExampleAdvanced : MonoBehaviour
{
    public Item prefab;
    public float roundTime;

    private StateMachine<States, Driver> fsm;
    private float playStartTime;

    private List<Item> spawnedItems;
    private Item targetItem;

    private void Awake()
    {
        //Initialize the state machine
        fsm = new StateMachine<States, Driver>(this);
        fsm.ChangeState(States.Idle); //Remember to set an initial state!
    }

    private void Update()
    {
        fsm.Driver.Update
            .Invoke(); //Tap the state machine into Unity's update loop. We could choose to call this from anywhere though!
    }

    private void OnGUI()
    {
        fsm.Driver.OnGUI.Invoke(); //Tap into the OnGUI update loop. 
    }

    private void DestroyItem(Item item)
    {
        item.Triggered -=
            fsm.Driver.OnItemSelected.Invoke; //Good hygiene, always remove your listeners when you are done! 
        Destroy(item.gameObject);
    }

    #region fsm

    public enum States
    {
        Idle,
        Play,
        GameWin,
        GameLose
    }

    public class Driver
    {
        public StateEvent OnGUI;
        public StateEvent<Item> OnItemSelected;
        public StateEvent Update;
    }

    private void Idle_OnGUI()
    {
        if (GUI.Button(new Rect(20, 20, 100, 30), "Begin")) fsm.ChangeState(States.Play);
    }

    private void Play_Enter()
    {
        playStartTime = Time.time;

        var count = 10;
        var targetIndex = Random.Range(0, count);
        spawnedItems = new List<Item>(count);
        for (var i = 0; i < count; i++)
        {
            var pos2D = Random.insideUnitCircle * 5;
            var item = Instantiate(prefab, new Vector3(pos2D.x, 0, pos2D.y), Quaternion.identity);
            item.isTarget = i == targetIndex;
            item.Triggered +=
                fsm.Driver.OnItemSelected.Invoke; //Pipe external events into the fsm - this is very powerful! 
            spawnedItems.Add(item);
        }
    }

    private void Play_OnGUI()
    {
        var timeRemaining = roundTime - (Time.time - playStartTime);
        GUI.Label(new Rect(20, 20, 300, 30), "Click items to find the target");
        GUI.Label(new Rect(20, 50, 300, 30), $"Time Remaining: {timeRemaining:n3}");
    }

    private void Play_Update()
    {
        if (Time.time - playStartTime >= roundTime) fsm.ChangeState(States.GameLose);
    }

    private void
        Play_OnItemSelected(
            Item item) //Data driven events guarantee correctness - only the Play state responds to OnItemSelected events  
    {
        if (item.isTarget)
        {
            targetItem = item;
            fsm.ChangeState(States.GameWin);
        }
        else
        {
            spawnedItems.Remove(item);
            DestroyItem(item);
        }
    }

    private void GameWin_Enter()
    {
        spawnedItems.Remove(targetItem);

        for (var i = spawnedItems.Count - 1; i >= 0; i--) //Reverse order as we're modifying the list in place
        {
            var item = spawnedItems[i];
            spawnedItems.Remove(item);
            DestroyItem(item);
        }
    }

    private void GameWin_OnGUI()
    {
        GUI.Label(new Rect(20, 20, 300, 30), "Well done, you found it!");

        if (GUI.Button(new Rect(20, 50, 100, 30), "Restart")) fsm.ChangeState(States.Idle);
    }

    private void GameWin_Exit()
    {
        DestroyItem(targetItem);
        targetItem = null;
    }

    private void GameLose_Enter()
    {
        for (var i = spawnedItems.Count - 1; i >= 0; i--) //Reverse order as we're modifying the list in place
        {
            var item = spawnedItems[i];
            spawnedItems.Remove(item);
            DestroyItem(item);
        }
    }

    private void GameLose_OnGUI()
    {
        GUI.Label(new Rect(20, 20, 300, 30), "Bad luck, you didn't find it in time!");

        if (GUI.Button(new Rect(20, 50, 100, 30), "Restart")) fsm.ChangeState(States.Idle);
    }

    #endregion
}