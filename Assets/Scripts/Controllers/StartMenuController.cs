using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class StartMenuController : MonoBehaviour
    {
        public void PlayButton()
        {
            var gm = FindObjectOfType<GameManager>();
            gm.cbm.Load();
            SceneManager.LoadScene(1);
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}