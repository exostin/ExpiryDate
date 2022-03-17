using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class CitybuildingManager : MonoBehaviour
    {
        private GameManager gm;

        void Start()
        {
            gm = FindObjectOfType<GameManager>();
        }
        
        public void EnterBattleMode()
        {
            gm.stateController.fsm.ChangeState(StateController.States.PlayerTurn);
            SceneManager.LoadScene(2);
        }
    }
}
