using UnityEngine;

namespace Controllers.BattleScene
{
    public class BattleMenuDebugController : MonoBehaviour
    {
        // [SerializeField] private GameObject debugButtons;
        [SerializeField] private BattleMenuController battleMenuController;
        // private bool debugMode;

        // public void ToggleDebugMode()
        // {
        //     debugMode = !debugMode;
        //     debugButtons.SetActive(debugMode);
        // }

        public void LoseBattle()
        {
            battleMenuController.soPlayerCharacters.ForEach(c => c.health = 0);
            battleMenuController.gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void WinBattle()
        {
            battleMenuController.soEnemyCharacters.ForEach(c => c.health = 0);
            battleMenuController.gm.stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }
    }
}