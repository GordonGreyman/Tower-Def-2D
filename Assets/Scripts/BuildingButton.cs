using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingButton : MonoBehaviour
{
    [SerializeField] private GameObject selectedBuilding;
    [SerializeField] private Sprite dragBuilding;
    [SerializeField] private GameObject buildingInfo;
    [SerializeField] private int buildingCostMoney;
    [SerializeField] private int buildingCostFood;
    [SerializeField] private int buildingCostWood;
    [SerializeField] private int buildingCostStone;
    [SerializeField] private int buildingCostPop;

    public GameObject SelectedBuilding
    {
        get { return selectedBuilding; }
    }
    public Sprite DragBuilding
    {
        get { return dragBuilding; }
    }
    public GameObject BuildingInfo
    {
        get { return buildingInfo; }
    }
    public int BuildingCostMoney
    {
        get { return buildingCostMoney; }
    }
    public int BuildingCostFood
    {
        get { return buildingCostFood; }
    }
    public int BuildingCostWood
    {
        get { return buildingCostWood; }
    }
    public int BuildingCostStone
    {
        get { return buildingCostStone; }
    }
    public int BuildingCostPop
    {
        get { return buildingCostPop; }
    }
}