using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Net;

public class Windmill : MonoBehaviour
{
    [SerializeField] private int destroyRevenue;
    [SerializeField] private int destroyPop;
    [SerializeField] private int moneyAmount = 5;
    //[SerializeField] private float health;
    //[SerializeField] private float takeDamageRadius;
    //[SerializeField] private float takenDamage;
    [SerializeField] private float moneyGainRate;

    //[SerializeField] private Slider healthBar;
    [SerializeField] private GameObject destroyButton;

    private float nextMoneyGain;

    //FOR UI BUILDING PANEL//
    [SerializeField] private string buildingName = "Windmill";
    [SerializeField] private Sprite mySprite;
    [SerializeField] private BuildingInfoPanel buildingInfoPanel;
    //FOR UI BUILDING PANEL//

    // Start is called before the first frame update
    void Start()
    {
        buildingInfoPanel = GameObject.Find("BuildingInfoPanel").GetComponent<BuildingInfoPanel>();

    }

    // Update is called once per frame
    void Update()
    {
        //TakeDamageFromEnemies();
        //healthBar.value = health;

        if (Time.time >= nextMoneyGain)
        {
            nextMoneyGain = Time.time + 1 / moneyGainRate;
            StartCoroutine(AddResources());
        }
        /*if (health <= 0)
        {
            Destroy(gameObject);
        }
        if (health < 100)
        {
            healthBar.gameObject.SetActive(true);
        }*/
    }
    private IEnumerator AddResources()
    {
        yield return new WaitForSeconds(1f);
        GameManager.Instance.AddMoney(-moneyAmount);
    }

    /*private void TakeDamageFromEnemies()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, takeDamageRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                health = health - takenDamage;
            }
        }
    }*/
 
    public void DestroyBuilding()
    {
        GameManager.Instance.AddMoney(destroyRevenue);
        GameManager.Instance.ChangePopulation(destroyPop);
        Destroy(gameObject);
    }

    private void OnMouseDown()
    {
        buildingInfoPanel.targetBuilding = gameObject;
        buildingInfoPanel.buildingImage.sprite = mySprite;
        buildingInfoPanel.buildingName.text = buildingName;
    }
}
