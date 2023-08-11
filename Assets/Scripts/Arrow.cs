using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;  

public class Arrow : MonoBehaviour
{
    public int arrowDamage;
    public int maxHitsAllowed = 1; // You can set the desired maximum hits in the Inspector
    private int hitCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        //salamsuck
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hitCount < maxHitsAllowed && collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.health -= arrowDamage;
            enemy.CheckHealthStatus();

            hitCount++;
                
            if (hitCount >= maxHitsAllowed)
            {
                Destroy(gameObject);
            }
        }
    }
}
