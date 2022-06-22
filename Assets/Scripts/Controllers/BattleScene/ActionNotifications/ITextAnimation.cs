using System.Collections;
using TMPro;

namespace Controllers.BattleScene.ActionNotifications
{
    public interface ITextAnimation
    {
        IEnumerator Play(TMP_Text target);
    }
}