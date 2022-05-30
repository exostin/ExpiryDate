using UnityEngine;
using UnityEngine.UI;

namespace Controllers.BattleScene
{
    public class SelectionIndicatorController : MonoBehaviour
    {
        // [SerializeField] private GameObject targetIndicator;
        [SerializeField] private GameObject abilityIndicator;

        [SerializeField] private int abilityYOffset;
        // [SerializeField] private int characterYOffset;

        // [SerializeField] private Transform playerCharactersParent;
        // [SerializeField] private Transform enemyCharactersParent;
        //
        // public void SelectFriendlyTarget(int horizontalLocation)
        // {
        //     if (!targetIndicator.activeSelf) targetIndicator.SetActive(true);
        //     targetIndicator.GetComponent<Image>().color = Color.green;
        //     targetIndicator.transform.SetParent(playerCharactersParent);
        //     targetIndicator.transform.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0.5f);
        //     targetIndicator.transform.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0.5f);
        //     targetIndicator.transform.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
        //     targetIndicator.transform.localPosition = new Vector2(horizontalLocation, characterYOffset);
        // }
        //
        // public void SelectEnemyTarget(int horizontalLocation)
        // {
        //     if (!targetIndicator.activeSelf) targetIndicator.SetActive(true);
        //     targetIndicator.GetComponent<Image>().color = Color.red;
        //     targetIndicator.transform.SetParent(enemyCharactersParent);
        //     targetIndicator.transform.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0.5f);
        //     targetIndicator.transform.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0.5f);
        //     targetIndicator.transform.GetComponent<RectTransform>().pivot = new Vector2(1, 0.5f);
        //     targetIndicator.transform.localPosition = new Vector2(horizontalLocation, characterYOffset);
        // }

        public void SelectAbility(int horizontalLocation)
        {
            if (!abilityIndicator.activeSelf) abilityIndicator.SetActive(true);
            abilityIndicator.transform.localPosition = new Vector2(horizontalLocation, abilityYOffset);
        }
    }
}