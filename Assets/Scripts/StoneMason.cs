using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Net;
using UnityEditor;

public class StoneMason : MonoBehaviour
{
    [SerializeField] private int upgradeCostMoney;
    [SerializeField] private int upgradeCostFood;
    [SerializeField] private int upgradeCostWood;
    [SerializeField] private int upgradeCostStone;
    [SerializeField] private int upgradeCostPop;

    [SerializeField] private int destroyRevenue;
    [SerializeField] private int destroyPop;
    [SerializeField] public int stoneAmount = 5;
    [SerializeField] public int moneyAmount = 5;
    [SerializeField] private int level = 1;
    [SerializeField] private float stoneGainRate;
    [SerializeField] private float stoneAroundRadius;
    [SerializeField] private GameObject man;
    [SerializeField] private GameObject fire;

    [SerializeField] private GameObject stoneCountGameObject;
    [SerializeField] private TextMeshProUGUI stoneCountText;
    [SerializeField] private Sprite[] sprites;

    bool stoneIsFound = false;
    private float nextStoneGain;
    [SerializeField] private List<GameObject> men;

    //FOR UI BUILDING PANEL//
    [SerializeField] private string buildingName = "StoneMason";
    [SerializeField] private Sprite mySprite;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    //FOR UI BUILDING PANEL//

    private SpriteRenderer sp;

    void Start()
    {
        GameManager.Instance.CheckBuildingResourceStats();
        DestroyObjectsInThePlace();
        buildingInfoPanel = GameObject.Find("BuildingInfoPanel").GetComponent<BuildingInfoPanel>();
        sp = GetComponent<SpriteRenderer>();
        //StartCoroutine(AddResources());
        SpawnFire();
        SpawnMan();
    }

    void Update()
    {
        if (!stoneIsFound)
        {
            CheckStoneAround();
        }

        if (GameManager.Instance.moneyGainDisplay == true)
        {
            stoneCountText.text = moneyAmount.ToString();
        }
    }

    //private IEnumerator AddResources()
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(5f);
    //        if (stoneIsFound)
    //        {
    //            GameManager.Instance.AddStone(stoneAmount);
    //        }
    //        GameManager.Instance.AddMoney(-moneyAmount);
    //    }

    //}

    private void CheckStoneAround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, stoneAroundRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Stone") stoneIsFound = true;
        }
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
    }

    public void UpgradeBuilding()
    {
        if (level < 4)
        {
            if (GameManager.Instance.Money >= upgradeCostMoney
                && GameManager.Instance.Food >= upgradeCostFood
                && GameManager.Instance.Wood >= upgradeCostWood
                && GameManager.Instance.Stone >= upgradeCostStone
                && GameManager.Instance.Pop >= upgradeCostPop)
            {
                level++;
                CheckBuildingLevel();
                GameManager.Instance.AddMoney(-upgradeCostMoney);
                GameManager.Instance.AddFood(-upgradeCostFood);
                GameManager.Instance.AddWood(-upgradeCostWood);
                GameManager.Instance.AddStone(-upgradeCostStone);
                GameManager.Instance.ChangePopulation(-upgradeCostPop);
                destroyPop += upgradeCostPop;
                stoneAmount += 6;
                moneyAmount -= 2;
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
        GameManager.Instance.AddMoney(destroyRevenue);
        GameManager.Instance.ChangePopulation(destroyPop);
        gameObject.SetActive(false);
        GameManager.Instance.CheckBuildingResourceStats();
        Destroy(gameObject);
    }
    public void OnClick()
    {
        buildingInfoPanel.targetBuilding = gameObject;
        buildingInfoPanel.buildingImage.sprite = mySprite;
        buildingInfoPanel.buildingName.text = buildingName;
        buildingInfoPanel.level = level;
        GameManager.Instance.SetRangeDisactive();

    }
    private void SpawnMan()
    {
        GameObject man_ = Instantiate(man, transform.position, Quaternion.identity);
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

