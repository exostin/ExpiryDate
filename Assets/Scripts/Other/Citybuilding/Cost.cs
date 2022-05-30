using Controllers;
using Other.Citybuilding;
using UnityEngine;
using Resources = Classes.Citybuilding.Resources;

public class Cost : MonoBehaviour
{
    public Resources Resources;

    [SerializeField] private CostResource water;
    [SerializeField] private CostResource titan;
    [SerializeField] private CostResource food;
    [SerializeField] private CostResource energy;

    private GameManager gm;
    private CitybuildingController cbc;
    
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (Resources is null) return;
        
        water.Amount = Resources.Water;
        water.gameObject.SetActive(Resources.Water != 0);
        water.IsMissing = Resources.Water > gm.cbm.PlayerResources.Water;
            
        titan.Amount = Resources.Titan;
        titan.gameObject.SetActive(Resources.Titan != 0);
        titan.IsMissing = Resources.Titan > gm.cbm.PlayerResources.Titan;
        
        food.Amount = Resources.Food;
        food.gameObject.SetActive(Resources.Food != 0);
        food.IsMissing = Resources.Food > gm.cbm.PlayerResources.Food;
        
        energy.Amount = Resources.Energy;
        energy.gameObject.SetActive(Resources.Energy != 0);
        energy.IsMissing = Resources.Energy > gm.cbm.PlayerResources.Energy;
    }
}
