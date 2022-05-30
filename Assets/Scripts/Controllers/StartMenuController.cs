using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Controllers
{
    public class StartMenuController : MonoBehaviour
    {
#pragma warning disable CS0414
        private readonly string webPlayerQuitURL = "https://www.google.com";
#pragma warning restore CS0414

        public void PlayButton()
        {
            var gm = FindObjectOfType<GameManager>();
            gm.cbm.Load();
            SceneManager.LoadScene(1);
        }
        
        public void NewGameButton()
        {
            var gm = FindObjectOfType<GameManager>();
            gm.cbm.RemoveSave();
            gm.cbm.Load();
            SceneManager.LoadScene(1);
        }

        public void ExitButton()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
                Application.OpenURL(webPlayerQuitURL);
#else
                Application.Quit();
#endif
        }
    }
}