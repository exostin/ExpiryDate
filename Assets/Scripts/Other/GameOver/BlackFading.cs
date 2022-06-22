using Controllers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Other.GameOver
{
    public class BlackFading : MonoBehaviour
    {
        private GameManager gm;
        private float alpha = 1.5f;

        private Image image;

        private void Start()
        {
            image = gameObject.GetComponent<Image>();
            gm = FindObjectOfType<GameManager>();
        }

        private void Update()
        {
            alpha -= 0.3f * Time.deltaTime;

            var newColor = Color.black;
            newColor.a = alpha;

            image.color = newColor;

            if (alpha < -1)
            {
                gm.cbm.RemoveSave();
                SceneManager.LoadScene("Scenes/StartMenu");
                gameObject.SetActive(false);
            };
        }
    }
}