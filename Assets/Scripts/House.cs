using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Net;

public class House : MonoBehaviour
{
    [SerializeField] private int upgradeCostMoney;
    [SerializeField] private int upgradeCostFood;
    [SerializeField] private int upgradeCostWood;
    [SerializeField] private int upgradeCostStone;

    [SerializeField] private int destroyRevenue;
    [SerializeField] private int destroyPop;
    [SerializeField] public int moneyAmount = 1;
    [SerializeField] public int foodAmount = 1;
    [SerializeField] private int popAmount = 4;
    [SerializeField] private int buildingCount;
    [SerializeField] private int level = 1;
    [SerializeField] private float moneyGainRate;
    [SerializeField] private float buildingsAroundRadius;

    [SerializeField] private GameObject man;

    [SerializeField] private Sprite[] sprites;

    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject upgradeButton;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject moneyCountGameObject;
    [SerializeField] private TextMeshProUGUI moneyCountText;

    //FOR UI BUILDING PANEL//
    [SerializeField] private string buildingName = "House";
    [SerializeField] private Sprite mySprite;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    [SerializeField] private GameObject upgradeInfo;

    //FOR UI BUILDING PANEL//

    [SerializeField] private List<GameObject> men;

    private bool towerPresent = false;
    private SpriteRenderer sp;
    private int towerMoney = 0;

    void Start()
    {
        GameManager.Instance.CheckBuildingResourceStats();
        sp = GetComponent<SpriteRenderer>();
        buildingInfoPanel = GameObject.Find("BuildingInfoPanel").GetComponent<BuildingInfoPanel>();
        GameManager.Instance.ChangePopulation(popAmount);
        DestroyObjectsInThePlace();
        SpawnMan();
        //StartCoroutine(AddResources());
        StartCoroutine(CheckAroundandInfo());
        SpawnFire();
    }

    void Update()
    {
        if (GameManager.Instance.moneyGainDisplay == true)
        {
            moneyCountText.text = moneyAmount.ToString();
        }
    }
   
        
    private void CheckBuildingLevel()
    {
        if (level == 2)
        {
            sp.sprite = sprites[0];
            SpawnMan();
        }

        if (level == 3)
        {
            sp.sprite = sprites[1];
            SpawnMan();
        }
        if (level == 4)
        {
            sp.sprite = sprites[2];
            SpawnMan();
        }

        //switch (level)
        //{
        //    case 2:
        //        sp.sprite = sprites[0];
        //        SpawnMan();
        //        break;
        //}
    }

    //private IEnumerator AddResources()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(5f);
    //        GameManager.Instance.AddMoney(moneyAmount);
    //        GameManager.Instance.AddFood(-foodAmount);
    //    }
    //}

    private IEnumerator CheckAroundandInfo()
    {
        while (true)
        {
            //moneyAmount = level + buildingCount + towerMoney;
            //foodAmount = level * 2;
            CheckBuildingsAround();
            yield return new WaitForSeconds(5f);
        }
    }

    private void CheckBuildingsAround()
    {
        UniqueBuildingImprovements();

        int groundLayer = LayerMask.GetMask("Collision Layer");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buildingsAroundRadius, groundLayer);

        /*List<GameObject> buildings = new List<GameObject>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "House")
            {
                buildings.Add(collider.gameObject);
                buildingCount = buildings.Count;
            }
            
        }*/

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Tower")
            {
                towerPresent = true;
                break;
            }
            else
            {
                towerPresent = false;
            }
        }
    }
    private void UniqueBuildingImprovements()
    {
        if (towerPresent == false)
        {
            towerMoney = 0;
        }
        else
        {
            towerMoney = 5;
        }
    }
    public void UpgradeBuilding()
    {
        if (level < 4)
        {
            if (GameManager.Instance.Money >= upgradeCostMoney
                && GameManager.Instance.Food >= upgradeCostFood
                && GameManager.Instance.Wood >= upgradeCostWood
                && GameManager.Instance.Stone >= upgradeCostStone)
            {
                level++;
                CheckBuildingLevel();
                GameManager.Instance.AddMoney(-upgradeCostMoney);
                GameManager.Instance.AddFood(-upgradeCostFood);
                GameManager.Instance.AddWood(-upgradeCostWood);
                GameManager.Instance.AddStone(-upgradeCostStone);
                GameManager.Instance.ChangePopulation(4);
                popAmount += 4;
                moneyAmount += 6;
                foodAmount -= 2;
                GameManager.Instance.CheckBuildingResourceStats();
                buildingInfoPanel.level = level;
            }
        }
        else
        {
            GameManager.Instance.ChangeText("This building has reached the highest level!");
        }
    }
    public void DestroyBuilding()
    {
        CheckDestroyRevenueAmount();
        GameManager.Instance.AddMoney(destroyRevenue);
        GameManager.Instance.ChangePopulation(-popAmount);

        foreach (GameObject man in men)
        {
            if (man != null)
            {
                Destroy(man.gameObject);
            }
        }
        gameObject.SetActive(false);
        GameManager.Instance.CheckBuildingResourceStats();
        Destroy(gameObject);
    }

    public void OnClick()
    {
        buildingInfoPanel.targetBuilding = gameObject;
        buildingInfoPanel.buildingImage.sprite = mySprite;
        buildingInfoPanel.buildingName.text = buildingName;
        buildingInfoPanel.upgradeInfo = upgradeInfo;

        buildingInfoPanel.level = level;
        buildingInfoPanel.goldCost = upgradeCostMoney;
        buildingInfoPanel.foodCost = upgradeCostFood;
        buildingInfoPanel.woodCost = upgradeCostWood;
        buildingInfoPanel.stoneCost = upgradeCostStone;
        buildingInfoPanel.popCost = 0;
        GameManager.Instance.SetRangeDisactive();

    }

    private void CheckDestroyRevenueAmount()
    {
        destroyRevenue = 10 * level;
    }

    private void DestroyObjectsInThePlace()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.10f);

        // Iterate through the detected colliders
        foreach (Collider2D collider in colliders)
        {
            // Check if the collider belongs to an enemy object
            if (collider.gameObject.CompareTag("Tree"))
            {
                // Shoot an arrow at the enemy
                Destroy(collider.gameObject);
            }
        }
    }

    private void SpawnMan()
    {
        GameObject man_ = Instantiate(man,transform.position, Quaternion.identity);
        men.Add(man_);
    }

    private void SpawnFire()
    {
        GameObject fireplace = Instantiate(fire, new Vector2(transform.position.x - Random.Range(-0.4f, 0.4f), transform.position.y - Random.Range(-0.4f, 0.4f)), Quaternion.identity);

        int buildingLayer = LayerMask.GetMask("Collision Layer");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(fireplace.transform.position, 0.22f, buildingLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != null)
            {
                Destroy(fireplace.gameObject);
            }
        }
    }
}
