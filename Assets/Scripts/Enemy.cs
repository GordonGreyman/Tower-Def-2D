using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform[] targetCheckpoints;
    [SerializeField] private Spawner spawner;
    //[SerializeField] private float searchRadius;
    [SerializeField] private LayerMask whichLayer;
    [SerializeField] private float moveSpeed, targetX, targetY;
    [SerializeField] public int health;
    [SerializeField] private int arrowDamage;
    [SerializeField] private int checkpoint = 0;

    [SerializeField] private Slider healthBar;
    [SerializeField] private Rigidbody2D rb;

    private bool checkpointHit = false;

    // Start is called before the first frame update
    void Start()
    {
        //transform.position = new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f), transform.position.z);
        rb = GetComponent<Rigidbody2D>();
        moveSpeed = Random.Range(0.2f, 0.3f);
        targetX = Random.Range(-0.2f, 0.2f);
        targetY = Random.Range(-0.2f, 0.2f);

        StartCoroutine(CheckInterval());
        CheckForSpawner();
    }

    // Update is called once per frame
    void Update()
    {
        GoToCheckpoint();
    }

    public void CheckHealthStatus()
    {
        healthBar.value = health;

        if (health <= 0)
        {
            GameManager.Instance.enemiesSpawned--;
            GameManager.Instance.AddMoney(1);
            Destroy(gameObject);
        }

        if (health < 100)
        {
            healthBar.gameObject.SetActive(true);
        }
    }
    private void CheckForSpawner()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Spawner")
            {
                spawner = collider.GetComponent<Spawner>();
                targetCheckpoints = spawner.checkpoints;
                break;
            }
        }
    }

    private void GoToCheckpoint()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetCheckpoints[checkpoint].transform.position, Time.deltaTime * moveSpeed);
    }

    IEnumerator CheckInterval()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
        }
    }
    
    IEnumerator CheckPointCoolDown()
    {
        checkpointHit = true;
        yield return new WaitForSeconds(2f);
        checkpointHit = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Checkpoint")
        {
            if (!checkpointHit)
            {
                StartCoroutine(CheckPointCoolDown());
                checkpoint++;
            }
        }
        if (collision.gameObject.tag == "Finish")
        {
            GameManager.Instance.enemiesSpawned--;
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "End")
        {
            GameManager.Instance.ChangeEndHealth(-1);
        }
    }
}
