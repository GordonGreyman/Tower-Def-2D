using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Farm : MonoBehaviour
{
    [SerializeField] private int upgradeCostMoney;
    [SerializeField] private int upgradeCostFood;
    [SerializeField] private int upgradeCostWood;
    [SerializeField] private int upgradeCostStone;
    [SerializeField] private int upgradeCostPop;

    [SerializeField] public int foodAmount = 1;
    [SerializeField] public int moneyAmount = 1;

    [SerializeField] private string buildingName = "Farm";
    [SerializeField] private int destroyRevenue;
    [SerializeField] private int destroyPop;
    [SerializeField] private int buildingCount;
    [SerializeField] private int level = 1;
    [SerializeField] private float foodGainRate;
    //[SerializeField] private float health;
    //[SerializeField] private float takeDamageRadius;
    // [SerializeField] private float takenDamage;
    [SerializeField] private float buildingsAroundRadius;

    [SerializeField] private GameObject man;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject fire;

    //[SerializeField] private Slider healthBar;
    [SerializeField] private GameObject destroyButton;
    [SerializeField] private GameObject foodCountGameObject;
    [SerializeField] private TextMeshProUGUI foodCountText;

    //FOR UI BUILDING PANEL//
    [SerializeField] private Sprite mySprite;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    //FOR UI BUILDING PANEL//
    [SerializeField] private List<GameObject> men;

    private bool windmillPresent = false;
    private SpriteRenderer sp;
    private float nextFoodGain;
    private int windmillFood = 0;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CheckBuildingResourceStats();
        sp = GetComponent<SpriteRenderer>();
        buildingInfoPanel = GameObject.Find("BuildingInfoPanel").GetComponent<BuildingInfoPanel>();
        DestroyObjectsInThePlace();
        SpawnMan();
        //StartCoroutine(AddResources());
        StartCoroutine(CheckAroundandInfo());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.moneyGainDisplay == true)
        {
            foodCountText.text = moneyAmount.ToString();
        }
    }


    private IEnumerator CheckAroundandInfo()
    {
        while (true)
        {
            //foodAmount = level + buildingCount + windmillFood;
            //moneyAmount = level * 2;
            CheckBuildingsAround();
            yield return new WaitForSeconds(5f);
        }
    }

    //private IEnumerator AddResources()
    //{
    //    while (true)
    //    {
    //        GameManager.Instance.AddMoney(-moneyAmount);
    //        GameManager.Instance.AddFood(foodAmount);
    //        yield return new WaitForSeconds(5f);
    //    }
    //}

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

    private void CheckBuildingsAround()
    {
        UniqueBuildingImprovements();

        int groundLayer = LayerMask.GetMask("Collision Layer");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, buildingsAroundRadius, groundLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Windmill")
            {
                windmillPresent = true;
                break;
            }
            else
            {
                windmillPresent = false;
            }
        }
    }

    private void UniqueBuildingImprovements()
    {
        if (windmillPresent == true)
        {
            windmillFood = 5;
        }
        else
        {
            windmillFood = 0;
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
                foodAmount += 6;
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
        CheckDestroyRevenueAmount();
        GameManager.Instance.AddMoney(destroyRevenue);
        GameManager.Instance.ChangePopulation(destroyPop);

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
        buildingInfoPanel.level = level;
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
        GameObject man_ = Instantiate(man, transform.position, Quaternion.identity);
        men.Add(man_);
    }
}
