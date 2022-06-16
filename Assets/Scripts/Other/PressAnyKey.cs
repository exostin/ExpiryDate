using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PressAnyKey : MonoBehaviour
{
    private const float DottingInterval = 0.6f;
    public TextMeshProUGUI pressAnyKeyText;
    private string _dots;
    private bool _dottingRunning = true;

    private void Start()
    {
        StartCoroutine(AnyKeyDotting());
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            _dottingRunning = false;
            SceneManager.LoadScene(1);
        }
    }

    private IEnumerator AnyKeyDotting()
    {
        while (_dottingRunning)
            for (var i = 0; i < 4; i++)
            {
                _dots = new string('.', i);
                pressAnyKeyText.SetText("Press any key to continue" + _dots);

                yield return new WaitForSeconds(DottingInterval);
            }
    }
}