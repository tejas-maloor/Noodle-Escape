using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    private int target = 0;

    [SerializeField] private float speed;
    [SerializeField] private bool loop = true;

    private void FixedUpdate()
    {
        if(transform.position == waypoints[target].position)
        {
            target++;
            if(target == waypoints.Count)
            {
                if(!loop) 
                { 
                    waypoints.Reverse();
                    target = 1;
                }
                else 
                { 
                    target = 0; 
                }
            }
        }
        else
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, step);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform, true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
