using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; 

public class TrailPathFollower : MonoBehaviour
{
    public PathCreator pathOne;
    public float speed = 5f;
    float distanceTraveled;
    bool travel1;
    public GameObject start1, end1;
    public ParticleSystem bubbles;

    private void Update()
    {
        if (travel1)
        {
            bubbles.gameObject.SetActive(true); 
            bubbles.Play(); 
            TravelOne();
        }
        else
        {
            bubbles.Stop();
            bubbles.gameObject.SetActive(false);
        }
    }
    void TravelOne()
    {
        distanceTraveled += speed * Time.deltaTime;
        transform.position = pathOne.path.GetPointAtDistance(distanceTraveled);
        transform.rotation = pathOne.path.GetRotationAtDistance(distanceTraveled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("StartTrail"))
        {
            travel1 = true; 
        }

        if (other.gameObject.CompareTag("EndTrail"))
        {
            travel1 = false; 
        }
    }
}
