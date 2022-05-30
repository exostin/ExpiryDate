using System;
using Controllers;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Other.GameOver
{
    public class BlackFading : MonoBehaviour
    {
        private float alpha = 1.5f;

        private Image image;

        private void Start()
        {
            image = gameObject.GetComponent<Image>();
        }

        private void Update()
        {
            alpha -= 0.3f * Time.deltaTime;

            var newColor = Color.black;
            newColor.a = alpha;

            image.color = newColor;

            if (alpha < -1)
            {
                FindObjectOfType<GameManager>().cbm.RemoveSave();
                SceneManager.LoadScene("Scenes/StartMenu");
                gameObject.SetActive(false);
            };
        }
    }
}