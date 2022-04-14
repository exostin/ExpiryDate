using UnityEngine;
using UnityEngine.SceneManagement;

namespace Other
{
    public class NextDayTemp : MonoBehaviour
    {
        // DELETE THIS SCRIPT ONCE CITYBUILDING SCENE IS FIXED
        public void NextDay()
        {
            SceneManager.LoadScene(2);
        }
    }
}