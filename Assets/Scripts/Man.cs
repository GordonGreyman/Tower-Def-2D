using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour
{
    [SerializeField] private Vector2 destination;
    [SerializeField] private Vector2 origin;

    [SerializeField] private bool destinationFound = false;
    [SerializeField] private bool waiting = false;

    [SerializeField] private float speed;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
        destination = origin;
        LookForDestination();
        StartCoroutine(WaitBeforeGoingToDestination());
    }

    // Update is called once per frame
    void Update()
    {
        if (!waiting) GoToDestination();
    }

    private void LookForDestination()
    {
        if (Vector2.Distance(transform.position, destination) < 0.02f)
        {
            destinationFound = false;

            if (!destinationFound)
            {
                destination = new Vector2(origin.x + Random.Range(-1f, 1f), origin.y + Random.Range(-1f, 1f));
                destinationFound = true;
            }
        }
    }

    private void GoToDestination()
    {
        LookForDestination();
        transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
    }

    private IEnumerator WaitBeforeGoingToDestination()
    {
        while (true) 
        {
            waiting = true;
            yield return new WaitForSeconds(2f);
            waiting = false;
            yield return new WaitForSeconds(5f);
        }
    }
}
