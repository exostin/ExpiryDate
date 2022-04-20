using System.Security.Cryptography;
using UnityEngine;

namespace Controllers.BattleScene
{
    public class BattleDebugController : MonoBehaviour
    {
        private BattleController battleController;
        private StateController stateController;

        private void Start()
        {
            battleController = FindObjectOfType<BattleController>();
            stateController = FindObjectOfType<StateController>();
        }

        public void LoseBattle()
        {
            battleController.soPlayerCharacters.ForEach(c => c.health = 0);
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        public void WinBattle()
        {
            battleController.soEnemyCharacters.ForEach(c => c.health = 0);
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }
    }
}