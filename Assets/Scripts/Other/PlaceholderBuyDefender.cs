using Classes;
using Controllers;
using UnityEngine;

public class PlaceholderBuyDefender : MonoBehaviour
{
    [SerializeField] private DefenderType defenderType;

    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    public void BuyDefender()
    {
        gm.cbm.Simulation.BuyDefender(defenderType);
    }
}
