using System.Collections;
using TMPro;
using UnityEngine;

namespace Controllers.BattleScene.ActionNotifications
{
    public class TextZoomInAndOutAnimation : MonoBehaviour, ITextAnimation
    {
        [SerializeField] private float desiredFontSize = 2.6f;
        
        public IEnumerator Play(TMP_Text target)
        {
            for (var i = desiredFontSize; i < 3.5f; i += 0.05f)
            {
                target.fontSize = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            yield return new WaitForSecondsRealtime(0.05f);
            for (var i = 3.5f; i > desiredFontSize; i -= 0.1f)
            {
                target.fontSize = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            target.text = null;
        }
    }
}