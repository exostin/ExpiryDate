using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class StartMenuController : MonoBehaviour
    {
        public void PlayButton()
        {
            SceneManager.LoadScene(1);
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}