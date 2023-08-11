using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private GameObject[] walls;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CheckTheWallsAroundInterwal());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckTheWallsAround()
    {
        int groundLayer = LayerMask.GetMask("Collision Layer");
        
        RaycastHit2D checkRight = Physics2D.Raycast(new Vector2(transform.position.x + 0.31f, transform.position.y), Vector2.right, 1.2f, groundLayer);
        RaycastHit2D checkLeft = Physics2D.Raycast(new Vector2(transform.position.x - 0.31f, transform.position.y), Vector2.left, 1.2f, groundLayer);
        RaycastHit2D checkAbove = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.31f), Vector2.up, 1.2f, groundLayer);
        RaycastHit2D checkBelow = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.31f), Vector2.down, 1.2f, groundLayer);

        if (checkAbove.collider != null && checkAbove.collider.tag == "Wall") walls[0].SetActive(true);
        else walls[0].SetActive(false);
        if (checkRight.collider != null && checkRight.collider.tag == "Wall") walls[1].SetActive(true);
        else walls[1].SetActive(false);
        if (checkBelow.collider != null && checkBelow.collider.tag == "Wall") walls[2].SetActive(true);
        else walls[2].SetActive(false);
        if (checkLeft.collider != null  && checkLeft.collider.tag ==  "Wall") walls[3].SetActive(true);
        else walls[3].SetActive(false);
    }
    
    private IEnumerator CheckTheWallsAroundInterwal()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.3f);
            CheckTheWallsAround();
        }
    }
}
