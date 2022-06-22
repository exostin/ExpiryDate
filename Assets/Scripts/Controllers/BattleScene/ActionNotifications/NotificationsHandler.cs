using System;
using Other.Enums;
using TMPro;
using UnityEngine;

namespace Controllers.BattleScene.ActionNotifications
{
    public class NotificationsHandler : MonoBehaviour
    {
        [SerializeField] private TMP_ColorGradient healColor;
        [SerializeField] private TMP_ColorGradient damageColor;
        [SerializeField] private TMP_ColorGradient shieldColor;
        [SerializeField] private TMP_ColorGradient statusColor;
        
        private TMP_Text notificationText;
        private ITextAnimation actionNotificationAnimation;

        private void Awake()
        {
            actionNotificationAnimation = GetComponent<ITextAnimation>();
            notificationText = GetComponent<TMP_Text>();
        }

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

            StartCoroutine(actionNotificationAnimation.Play(notificationText));
        }
    }
}
