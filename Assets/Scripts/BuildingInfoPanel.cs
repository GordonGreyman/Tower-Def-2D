using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildingInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI buildingName;
    public GameObject targetBuilding;
    public Image buildingImage;
    public GameObject upgradeInfo;
    public GameObject circle;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI popText;

    public int level;
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int popCost;

    private BuildingManager buildingManager;

    // Start is called before the first frame update
    void Start()
    {
        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();   
    }

    // Update is called once per frame
    void Update()
    {
        levelText.text = level.ToString();
        goldText.text = goldCost.ToString();
        foodText.text = foodCost.ToString();
        woodText.text = woodCost.ToString();
        stoneText.text = stoneCost.ToString();
        popText.text = popCost.ToString();

        if (targetBuilding == null)
        {
            circle.SetActive(false);
        }
        else if (circle != null && targetBuilding != null)
        {
            circle.SetActive(true);
            circle.transform.position = targetBuilding.transform.position;
        }
        
    }

    public void DestroyBuilding()
    {
        if (targetBuilding != null)
        {
            House house = targetBuilding.GetComponent<House>();
            Farm farm = targetBuilding.GetComponent<Farm>();
            Woodcutter woodcutter = targetBuilding.GetComponent<Woodcutter>();
            Tower tower = targetBuilding.GetComponent<Tower>();
            Windmill windmill = targetBuilding.GetComponent<Windmill>();
            StoneMason stonemason = targetBuilding.GetComponent<StoneMason>();

            if (house != null)
            {
                house.DestroyBuilding();
            }
            if (farm != null)
            {
                farm.DestroyBuilding();
            }
            if (tower != null)
            {
                tower.DestroyBuilding();
            }
            if (windmill != null)
            {
                windmill.DestroyBuilding();
            }
            if (woodcutter != null)
            {
                woodcutter.DestroyBuilding();
            }
            if (stonemason != null)
            {
                stonemason.DestroyBuilding();
            }
        }
    }

    public void UpgradeBuilding()
    {
        if (targetBuilding != null) 
        {
            House house = targetBuilding.GetComponent<House>();
            Farm farm = targetBuilding.GetComponent<Farm>();
            Woodcutter woodcutter = targetBuilding.GetComponent<Woodcutter>();
            Tower tower = targetBuilding.GetComponent<Tower>();
            Windmill windmill = targetBuilding.GetComponent<Windmill>();
            StoneMason stonemason = targetBuilding.GetComponent<StoneMason>();

            if (house != null)
            {
                house.UpgradeBuilding();
            }
            if (farm != null)
            {
                farm.UpgradeBuilding();
            }
            if (tower != null)
            {
                tower.UpgradeBuilding();
            }
            if (windmill != null)
            {
                windmill.DestroyBuilding();
            }
            if (woodcutter != null)
            {
                woodcutter.UpgradeBuilding();
            }
            if (stonemason != null)
            {
                stonemason.UpgradeBuilding();
            }
        }
    }
}
