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

        /// <summary>
        ///     Kill all player characters
        /// </summary>
        public void LoseBattle()
        {
            foreach (var c in battleController.SoPlayerCharacters) c.Health = 0;
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }

        /// <summary>
        ///     Kill all enemy characters
        /// </summary>
        public void WinBattle()
        {
            foreach (var c in battleController.SoEnemyCharacters) c.Health = 0;
            stateController.fsm.ChangeState(StateController.States.ReadyForNextTurn);
        }
    }
}