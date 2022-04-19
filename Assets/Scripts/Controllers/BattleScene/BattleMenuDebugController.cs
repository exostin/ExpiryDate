using System.Security.Cryptography;
using UnityEngine;

namespace Controllers.BattleScene
{
    public class BattleMenuDebugController : MonoBehaviour
    {
        private BattleMenuController battleMenuController;
        private StateController stateController;

        private void Start()
        {
            battleMenuController = FindObjectOfType<BattleMenuController>();
            stateController = FindObjectOfType<StateController>();
        }

        public void LoseBattle()
        {
            battleMenuController.soPlayerCharacters.ForEach(c => c.health = 0);
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void WinBattle()
        {
            battleMenuController.soEnemyCharacters.ForEach(c => c.health = 0);
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }
    }
}