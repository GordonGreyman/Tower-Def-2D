using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float spawnRate;
    public float spawnTime;

    public float time;

    public int[] enemiesToSpawn;
    public int enemiesSpawned;
    public int level;

    public bool moneyGainDisplay = false;
    public bool DestroyBuildingsDisplay = false;

    public int moneyToAdd;
    public int foodToAdd;
    public int woodToAdd;
    public int stoneToAdd;

    public GameObject[] spawnPoint;
    public GameObject enemy;
    public GameObject buildingInfo;

    public TextMeshProUGUI levelText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI spawnTimeText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI popText;

    public TextMeshProUGUI moneyLevel;
    public TextMeshProUGUI foodLevel;
    public TextMeshProUGUI woodLevel;
    public TextMeshProUGUI stoneLevel;

    public Slider endHealthBar;

    public TextMeshProUGUI enemiesSpawnedText;
    public TextMeshProUGUI enemiesToSpawnText;
    public TextMeshProUGUI console;

    public Image buildingSprite;
    public Sprite[] treeSprites;
    public Sprite[] groundSprites;

    [SerializeField] private int money;
    [SerializeField] private int food;
    [SerializeField] private int wood;
    [SerializeField] private int stone;
    [SerializeField] private int pop;

    [SerializeField] private int endHealth;

    public TextMeshProUGUI Console { get { return console; } }
    public Image BuildingSprite { get { return buildingSprite; } }
    public int EndHealth { get { return endHealth; } }
    public int Money { get { return money; } }
    public int Food { get { return food; } }
    public int Wood { get { return wood; } }
    public int Stone { get { return stone; } }
    public int Pop { get { return pop; } }

    private AudioSource aS;
    [SerializeField] AudioClip[] songs;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        FindAllTrees();
        ChangeGround();
    }
    // Start is called before the first frame update
    void Start()
    {
        aS = GetComponent<AudioSource>();
        aS.PlayOneShot(songs[Random.Range(0,songs.Length)]);
        StartCoroutine(AddResources());
        StartCoroutine(UIInfo());
    }

    // Update is called once per frame
    void Update()
    {
        time = time + Time.deltaTime;
        Timer();
        MoneyGainDisplayOnScreen();
        DestroyBuildingsDisplayOnScreen();
        popText.text = pop.ToString();
        endHealthBar.value = endHealth;
    }

    public void ChangeBuildingInfo()
    {   
        GameObject buildInfo = Instantiate(buildingInfo);
        buildInfo.transform.SetParent(GameObject.Find("FindImage").transform);
        buildInfo.transform.localPosition = Vector3.zero;
        buildInfo.transform.localRotation = Quaternion.identity;
    }

    private void LateUpdate()
    {
    }
    private void Timer()
    {
        if (time >= spawnTime)
        {
            time = 0;
            if (time == 0)
            {
                StartCoroutine(spawn());
                level++;
            }
        }
    }
    public void ChangeText(string text)
    {
        console.text = text;
    }
    public void ChangeEndHealth(int amount)
    {
        endHealth += amount;
    }
    public void ChangePopulation(int amount)
    {
        pop += amount;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
    }
    public void AddFood(int amount)
    {
        food += amount;
    }
    public void AddWood(int amount)
    {
        wood += amount;
    }
    public void AddStone(int amount)
    {
        stone += amount;
    }
    public void AddPop(int amount)
    {
        pop += amount;
    }

    //FOR UI BUILDING INFO//
    public void ChangeUISprite(Sprite sprite)
    {
        buildingSprite.sprite = sprite;
    }


    IEnumerator spawn()
    {
        for (int i = 0; i < enemiesToSpawn[level]; i++)
        {
            yield return new WaitForSeconds(spawnRate);
            SpawnEnemy();
        }
    }

    public void CheckBuildingResourceStats()
    {
        int moneyS;
        int foodS;
        int woodS;
        int stoneS;

        moneyToAdd = 0;
        foodToAdd = 0;
        woodToAdd = 0;
        stoneToAdd = 0;

        GameObject[] houses = GameObject.FindGameObjectsWithTag("House");
        GameObject[] farms = GameObject.FindGameObjectsWithTag("Farm");
        GameObject[] woodcutters = GameObject.FindGameObjectsWithTag("WoodCutter");
        GameObject[] stonemasons = GameObject.FindGameObjectsWithTag("StoneMason");
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        if (houses.Length > 0)
        {
            foreach (GameObject house in houses)
            {
                moneyS = house.GetComponent<House>().moneyAmount;
                foodS = house.GetComponent<House>().foodAmount;
                moneyToAdd += moneyS;
                foodToAdd += foodS;
            }
        }

        if (farms.Length > 0)
        {
            foreach (GameObject farm in farms)
            {
                moneyS = farm.GetComponent<Farm>().moneyAmount;
                foodS = farm.GetComponent<Farm>().foodAmount;
                moneyToAdd += moneyS;
                foodToAdd += foodS;
            }
        }

        if (woodcutters.Length > 0)
        {
            foreach (GameObject woodcutter in woodcutters)
            {
                moneyS = woodcutter.GetComponent<Woodcutter>().moneyAmount;
                woodS = woodcutter.GetComponent<Woodcutter>().woodAmount;
                moneyToAdd += moneyS;
                woodToAdd += woodS;
            }
        }

        if (stonemasons.Length > 0)
        {
            foreach (GameObject stonemason in stonemasons)
            {
                moneyS = stonemason.GetComponent<StoneMason>().moneyAmount;
                stoneS = stonemason.GetComponent<StoneMason>().stoneAmount;
                moneyToAdd += moneyS;
                stoneToAdd += stoneS;
            }
        }

        if (towers.Length > 0)
        {
            foreach (GameObject tower in towers)
            {
                moneyS = tower.GetComponent<Tower>().moneyAmount;
                moneyToAdd += moneyS;
            }
        }
    }

    private IEnumerator AddResources()
    {
        while (true)
        {
            AddMoney(moneyToAdd);
            AddFood(foodToAdd);
            AddWood(woodToAdd);
            AddStone(stoneToAdd);
            yield return new WaitForSeconds(5f);
        }
    }
    private void SpawnEnemy()
    {
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = spawnPoint[Random.Range(0, spawnPoint.Length)].transform.position;
        enemiesSpawned++;
    }

    private void MoneyGainDisplayOnScreen()
    {
        if (moneyGainDisplay)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Transform child = obj.transform.Find("Money Text");
                if (child != null)
                {
                    // Set the child object active
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Transform child = obj.transform.Find("Money Text");
                if (child != null)
                {
                    // Set the child object active
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    private IEnumerator UIInfo()
    {
        while (true)
        {
            //RESOURCES//
            moneyText.text = money.ToString();
            foodText.text = food.ToString();
            woodText.text = wood.ToString();
            stoneText.text = stone.ToString();

            //RESOURCE PRODUCTION//
            moneyLevel.text = moneyToAdd.ToString();
            foodLevel.text = foodToAdd.ToString();
            woodLevel.text = woodToAdd.ToString();
            stoneLevel.text = stoneToAdd.ToString();

            if (moneyToAdd > 0) moneyLevel.color = Color.green;
            if (moneyToAdd <= 0) moneyLevel.color = Color.red;
            if (foodToAdd > 0) foodLevel.color = Color.green;
            if (foodToAdd <= 0) foodLevel.color = Color.red;
            if (woodToAdd > 0) woodLevel.color = Color.green;
            if (woodToAdd <= 0) woodLevel.color = Color.red;
            if (stoneToAdd > 0) stoneLevel.color = Color.green;
            if (stoneToAdd <= 0) stoneLevel.color = Color.red;

            //LEVEL INFO//

            levelText.text = level.ToString();
            int timINT = (int)time;
            timeText.text = timINT.ToString();
            enemiesSpawnedText.text = enemiesSpawned.ToString();
            enemiesToSpawnText.text = enemiesToSpawn[level].ToString();
            spawnTimeText.text = spawnTime.ToString();

            //LIMITERS//

            if (money <= 0)
            {
                money = 0;
            }

            if (food <= 0)
            {
                food = 0;
            }
            
            if (wood <= 0)
            {
                wood = 0;
            }

            if (stone <= 0)
            {
                stone = 0;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    public void ShowMoneyGain()
    {
        moneyGainDisplay = true;
    }
    public void HideMoneyGain()
    {
        moneyGainDisplay = false;
    }
    public void ShowBuildingDestroy()
    {
        DestroyBuildingsDisplay = true;
    }
    public void HideBuildingDestroy()
    {
        DestroyBuildingsDisplay = false;
    }
    public void HideBuildingDetails()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Transform child = obj.transform.Find("Upgrade");
            if (child != null)
            {
                // Set the child object active
                child.gameObject.SetActive(false);
            }
        }
        foreach (GameObject obj in allObjects)
        {
            Transform child = obj.transform.Find("Destroy");
            if (child != null)
            {
                // Set the child object active
                child.gameObject.SetActive(false);
            }
        }
    }
    private void DestroyBuildingsDisplayOnScreen()
    {
        if (DestroyBuildingsDisplay)
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Transform child = obj.transform.Find("Destroy");
                if (child != null)
                {
                    // Set the child object active
                    child.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            GameObject[] allObjects = FindObjectsOfType<GameObject>();

            foreach (GameObject obj in allObjects)
            {
                Transform child = obj.transform.Find("Destroy");
                if (child != null)
                {
                    // Set the child object active
                    child.gameObject.SetActive(false);
                }
            }
        }
    }
    public void DestroyBuildings()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Transform child = obj.transform.Find("Destroy");
            if (child != null)
            {
                // Set the child object active
                child.gameObject.SetActive(true);
            }
        }
    }

    private void FindAllTrees()
    {
        GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
        SpriteRenderer rb;

        foreach (GameObject tree in trees)
        {
            rb = tree.GetComponent<SpriteRenderer>();
            rb.sprite = treeSprites[Random.Range(0, treeSprites.Length)];
        }
    }

    private void ChangeGround()
    {
        GameObject[] groundTiles = GameObject.FindGameObjectsWithTag("Ground");
        SpriteRenderer rb;

        foreach (GameObject tile in groundTiles)
        {
            rb = tile.GetComponent<SpriteRenderer>();
            rb.sprite = groundSprites[Random.Range(0, groundSprites.Length)];
        }
    }

    public void SetRangeDisactive()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            Transform child = obj.transform.Find("Range");
            if (child != null)
            {
                // Set the child object active
                child.gameObject.SetActive(false);
            }
        }
    }
}
