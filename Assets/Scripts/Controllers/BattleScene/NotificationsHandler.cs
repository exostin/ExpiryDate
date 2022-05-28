using System;
using System.Collections;
using Other.Enums;
using UnityEngine;

namespace Controllers.BattleScene
{
    public class NotificationsHandler : MonoBehaviour
    {
        [SerializeField] private TMPro.TMP_ColorGradient healColor;
        [SerializeField] private TMPro.TMP_ColorGradient damageColor;
        [SerializeField] private TMPro.TMP_ColorGradient shieldColor;
        [SerializeField] private TMPro.TMP_ColorGradient statusColor;
        [SerializeField] private TMPro.TMP_Text notificationText;
        [SerializeField] private float desiredFontSize = 2.6f;

        public void HandleNotification(AbilityType abilityType, string value)
        {
            switch (abilityType)
            {
                case AbilityType.DamageOnly:
                    notificationText.colorGradientPreset = damageColor;
                    notificationText.text = $"-{value} HP";
                    break;
                case AbilityType.Heal:
                    notificationText.colorGradientPreset = healColor;
                    notificationText.text = $"+{value} HP";
                    break;
                case AbilityType.Shield:
                    notificationText.colorGradientPreset = shieldColor;
                    notificationText.text = $"+{value} SHIELD";
                    break;
                case AbilityType.Status:
                    notificationText.colorGradientPreset = statusColor;
                    notificationText.text = value;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(abilityType), abilityType, null);
            }

            StartCoroutine(NotificationPopupAnimation());
        }

        private IEnumerator NotificationPopupAnimation()
        {
            for (var i = desiredFontSize; i < 3.5f; i += 0.05f)
            {
                notificationText.fontSize = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }
            yield return new WaitForSecondsRealtime(0.05f);
            for (var i = 3.5f; i > desiredFontSize; i -= 0.1f)
            {
                notificationText.fontSize = i;
                yield return new WaitForSecondsRealtime(0.02f);
            }

            notificationText.text = null;
        }
    }
}
