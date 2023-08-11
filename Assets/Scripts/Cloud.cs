using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private Vector2 destination;
    [SerializeField] private bool destinationFound = false;
    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        destination = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-6.5f, 6.5f));
    }

    // Update is called once per frame
    void Update()
    {
        GoToDestination();
        
    }

    private void LookForDestination()
    {
        if (Vector2.Distance(transform.position, destination) < 0.2f)
        {
            destinationFound = false;

            if (!destinationFound)
            {
                destination = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-6.5f, 6.5f));
                destinationFound = true;
            }
        }
    }

    private void GoToDestination()
    {
        LookForDestination();
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }
}

