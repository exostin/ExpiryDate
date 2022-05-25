using Classes;
using Controllers;
using TMPro;
using UnityEngine;

public class PlaceholderDefenderAmountLabel : MonoBehaviour
{
    [SerializeField] private DefenderType defenderType;

    private GameManager gm;
    private TextMeshProUGUI textComponent;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        textComponent = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        textComponent.text = gm.cbm.Defenders[defenderType].Amount.ToString();
    }
}