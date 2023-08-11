using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class Tower : MonoBehaviour
{
    [SerializeField] private int upgradeCostMoney;
    [SerializeField] private int upgradeCostFood;
    [SerializeField] private int upgradeCostWood;
    [SerializeField] private int upgradeCostStone;
    [SerializeField] private int upgradeCostPop;

    [SerializeField] private int destroyRevenue;
    [SerializeField] private int destroyPop;
    [SerializeField] private float detectionRadius;
    [SerializeField] private float fireRate;
    [SerializeField] private int level = 1;
    [SerializeField] public int moneyAmount = 10;
    
    [SerializeField] AudioClip arrowSound;

    private float nextFireTime;

    [SerializeField] private Sprite[] sprites;

    public GameObject arrowPrefab;

    //FOR UI BUILDING PANEL//
    [SerializeField] private string buildingName = "Tower";
    [SerializeField] private Sprite mySprite;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    [SerializeField] private GameObject range;
    [SerializeField] private GameObject upgradeInfo;

    private float X, Y;

    //FOR UI BUILDING PANEL//

    private SpriteRenderer sp;
    private AudioSource aS;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.CheckBuildingResourceStats();
        aS = GetComponent<AudioSource>();
        sp = GetComponent<SpriteRenderer>();
        DestroyObjectsInThePlace();
        buildingInfoPanel = GameObject.Find("BuildingInfoPanel").GetComponent<BuildingInfoPanel>();
        StartCoroutine(CheckInterwal());
        X = detectionRadius * 2f; Y = detectionRadius * 2f;
        range.transform.localScale = new Vector2(X, Y);
        //StartCoroutine(AddResources());

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckBuildingLevel()
    {
        if (level == 2)
        {
            detectionRadius += 0.1f;
            X = detectionRadius * 2f; Y = detectionRadius * 2f;
            range.transform.localScale = new Vector2(X, Y);
            sp.sprite = sprites[0];
            fireRate -= 0.33f;
        }
        if (level == 3)
        {
            detectionRadius += 0.1f;
            X = detectionRadius * 2f; Y = detectionRadius * 2f;
            range.transform.localScale = new Vector2(X, Y);
            sp.sprite = sprites[1];
            fireRate -= 0.33f;
        }
        if (level == 4)
        {
            detectionRadius += 0.1f;
            X = detectionRadius * 2f; Y = detectionRadius * 2f;
            range.transform.localScale = new Vector2(X, Y);
            sp.sprite = sprites[2];
            fireRate -= 0.34f;
        }
    }

    IEnumerator CheckInterwal()
    {
        while (true)
        {
            DetectAndShootAtEnemies();
            yield return new WaitForSeconds(fireRate);
        }
    }

    //private IEnumerator AddResources()
    //{
    //    while (true)
    //    {
    //        GameManager.Instance.AddMoney(-moneyAmount);
    //        yield return new WaitForSeconds(5f);
    //    }
    //}

    private void DetectAndShootAtEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        // Find the closest enemy
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                Transform enemyTransform = collider.transform;
                Vector3 directionToEnemy = enemyTransform.position - currentPosition;
                float distanceSqrToEnemy = directionToEnemy.sqrMagnitude;

                if (distanceSqrToEnemy < closestDistanceSqr)
                {
                    closestEnemy = enemyTransform;
                    closestDistanceSqr = distanceSqrToEnemy;
                }
            }
        }

        // Shoot an arrow at the closest enemy, if any
        if (closestEnemy != null)
        {
            ShootArrow(closestEnemy.gameObject);
        }
    }
    private void ShootArrow(GameObject enemy)
    {
        // Instantiate an arrow object
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);

        aS.PlayOneShot(arrowSound);


        //Arrow ar = arrow.GetComponent<Arrow>();


        //if (level == 2)
        //{
        //    ar.maxHitsAllowed = 2;
        //}
        //else if (level == 3)
        //{
        //    ar.maxHitsAllowed = 3;
        //}
        //else if (level == 4)
        //{
        //    ar.maxHitsAllowed = 4;
        //}

        // Calculate the direction to the enemy
        Vector2 direction = (enemy.transform.position - transform.position).normalized;

        // Calculate the angle in degrees between the direction and the arrow's initial orientation
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Rotate the arrow to face the enemy
        arrow.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // Set the arrow's initial velocity
        Rigidbody2D arrowRigidbody = arrow.GetComponent<Rigidbody2D>();
        arrowRigidbody.velocity = direction * 5f;

        Destroy(arrow, 3f);
        /* Set any other properties of the arrow (e.g., damage, effects, etc.)
        ArrowScript arrowScript = arrow.GetComponent<ArrowScript>();
        arrowScript.SetDamage(arrowDamage); */
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
        buildingInfoPanel.popCost = upgradeCostPop;

        GameManager.Instance.SetRangeDisactive();

        range.SetActive(true);

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
                moneyAmount -= 6;
                GameManager.Instance.CheckBuildingResourceStats();
                buildingInfoPanel.level = level;
            }
            else
            {
                GameManager.Instance.ChangeText("There is not enough resources!");
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
        gameObject.SetActive(false);
        GameManager.Instance.CheckBuildingResourceStats();
        Destroy(gameObject);
    }
}
