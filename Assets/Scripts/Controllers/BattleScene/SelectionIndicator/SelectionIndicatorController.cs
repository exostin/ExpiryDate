using UnityEngine;

namespace Controllers.BattleScene.SelectionIndicator
{
    public class SelectionIndicatorController : MonoBehaviour
    {
        [SerializeField] private GameObject abilityIndicator;

        [SerializeField] private int abilityYOffset;
        public void SelectAbility(int horizontalLocation)
        {
            if (!abilityIndicator.activeSelf) abilityIndicator.SetActive(true);
            abilityIndicator.transform.localPosition = new Vector2(horizontalLocation, abilityYOffset);
        }
    }
}