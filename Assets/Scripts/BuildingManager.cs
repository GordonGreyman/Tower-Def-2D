using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public BuildingButton selectedBuilding;
    public SpriteRenderer spriteRenderer;
    private int selectedBuildingCostMoney;
    private int selectedBuildingCostFood;
    private int selectedBuildingCostWood;
    private int selectedBuildingCostStone;
    private int selectedBuildingCostCPop;

    public GameObject buildingInfo;

    private BuildingInfoPanel buildingInfoPanel;
    private void Start()
    {
        buildingInfoPanel = GetComponent<BuildingInfoPanel>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }

    void Update()
    {
        if (spriteRenderer.enabled)
        {
            followMouse();

            int bl = LayerMask.GetMask("Collision Layer");
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D bh = Physics2D.Raycast(wp, Vector2.zero, Mathf.Infinity, bl);

            if (bh.collider != null)
            {
                spriteRenderer.color = Color.red;
            }
            else
            {
                spriteRenderer.color = Color.green;
            }

            if (Input.GetMouseButtonDown(0))
            {

                int groundLayer = LayerMask.GetMask("Build Layer");
                int buildingLayer = LayerMask.GetMask("Collision Layer");


                Vector2 worldpoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D groundHit = Physics2D.Raycast(worldpoint, Vector2.zero, Mathf.Infinity, groundLayer);
                RaycastHit2D buildingHit = Physics2D.Raycast(worldpoint, Vector2.zero, Mathf.Infinity, buildingLayer);

                if (groundHit.collider != null)
                {
                    if (groundHit.collider.tag == "Ground" && !EventSystem.current.IsPointerOverGameObject())
                    {

                        if (GameManager.Instance.Money >= selectedBuildingCostMoney 
                            && GameManager.Instance.Food >= selectedBuildingCostFood 
                            && GameManager.Instance.Wood >= selectedBuildingCostWood 
                            && GameManager.Instance.Stone >= selectedBuildingCostStone
                            && GameManager.Instance.Pop >= selectedBuildingCostCPop)
                        {
                            if (buildingHit.collider == null)
                            {
                                GameManager.Instance.AddMoney(-selectedBuildingCostMoney);
                                GameManager.Instance.AddFood(-selectedBuildingCostFood);
                                GameManager.Instance.AddWood(-selectedBuildingCostWood);
                                GameManager.Instance.AddStone(-selectedBuildingCostStone);
                                GameManager.Instance.ChangePopulation(-selectedBuildingCostCPop);
                                PlaceBuilding(worldpoint);
                            }
                            else
                            {
                                GameManager.Instance.ChangeText("There is already a building there!");
                            }
                        }
                        else
                        {
                            if (GameManager.Instance.Money < selectedBuildingCostMoney)
                                GameManager.Instance.ChangeText("There is not enough resources! You need " + selectedBuildingCostMoney + " gold!");

                            else if (GameManager.Instance.Food < selectedBuildingCostFood)
                                GameManager.Instance.ChangeText("There is not enough resources! You need " + selectedBuildingCostFood + " food!");

                            else if (GameManager.Instance.Wood < selectedBuildingCostWood)
                                GameManager.Instance.ChangeText("There is not enough resources! You need " + selectedBuildingCostWood + " wood!");

                            else if (GameManager.Instance.Stone < selectedBuildingCostStone)
                                GameManager.Instance.ChangeText("There is not enough resources! You need " + selectedBuildingCostStone + " Stone!");

                            else if (GameManager.Instance.Pop < selectedBuildingCostCPop)
                                GameManager.Instance.ChangeText("There is not enough resources! You need " + selectedBuildingCostCPop + " people!");
                        }

                    }
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                spriteRenderer.enabled = false;
                selectedBuilding = null;
            }
        }

      
    }

    private void PlaceBuilding(Vector2 hit)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && selectedBuilding != null)
        {
            GameObject buildingToBuild = Instantiate(selectedBuilding.SelectedBuilding);
            buildingToBuild.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition - new Vector3(0,0, Camera.main.transform.position.z));
            hit = buildingToBuild.transform.position;
        }
    }

    
    // FOR UI BUTTON //
    public void SetTheBuilding(BuildingButton building)
    {
        selectedBuilding = building;
        buildingInfo = selectedBuilding.BuildingInfo;
        GameManager.Instance.buildingInfo = buildingInfo;
        GameManager.Instance.ChangeBuildingInfo();
        spriteRenderer.enabled = true;
        enableDragSprite(selectedBuilding.DragBuilding);
    }

    public void BuildingCostMoney(BuildingButton buildingCostMoney)
    {
        selectedBuildingCostMoney = buildingCostMoney.BuildingCostMoney;
    }
    public void BuildingCostFood(BuildingButton buildingCostFood)
    {
        selectedBuildingCostFood = buildingCostFood.BuildingCostFood;
    }
    public void BuildingCostWood(BuildingButton buildingCostWood)
    {
        selectedBuildingCostWood = buildingCostWood.BuildingCostWood;
    }
    public void BuildingCostStone(BuildingButton buildingCostStone)
    {
        selectedBuildingCostStone = buildingCostStone.BuildingCostStone;
    }
    public void BuildingCostPop(BuildingButton buildingCostPop)
    {
        selectedBuildingCostCPop = buildingCostPop.BuildingCostPop;
    }

    // FOR MOUSE CONFIGURATION //
    public void followMouse()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    public void enableDragSprite(Sprite sprite)
    {
        spriteRenderer.enabled = true;
        spriteRenderer.sprite = sprite;
    }

    public void disableDragSprite()
    {
        spriteRenderer.enabled = false;
    }
}
