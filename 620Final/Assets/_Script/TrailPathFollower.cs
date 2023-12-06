using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation; 

public class TrailPathFollower : MonoBehaviour
{
    public PathCreator pathOne, pathTwo;
    public float speed = 5f;
    float distanceTraveled;
    bool travel1, travel2;
    public GameObject start1, end1, start2, end2;
    private void Update()
    {
        if (travel1)
        {
            TravelOne();
        }
        if (travel2)
        {
            TravelTwo();
        }
    }
    void TravelOne()
    {
        distanceTraveled += speed * Time.deltaTime;
        transform.position = pathOne.path.GetPointAtDistance(distanceTraveled);
        transform.rotation = pathOne.path.GetRotationAtDistance(distanceTraveled);
    }

    void TravelTwo()
    {
        distanceTraveled += speed * Time.deltaTime;
        transform.position = pathTwo.path.GetPointAtDistance(distanceTraveled);
        transform.rotation = pathTwo.path.GetRotationAtDistance(distanceTraveled);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == start1)
        {
            travel1 = true;
        }

        if (other.gameObject == end1)
        {
            travel1 = false;
        }

        if (other.gameObject == start2)
        {
            travel2 = true;
        }

        if (other.gameObject == end2)
        {
            travel2 = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == end1)
        {
            start2.gameObject.SetActive(true);
            end2.gameObject.SetActive(true);
            start1.gameObject.SetActive(false);
            end1.gameObject.SetActive(false);
        }

        if (other.gameObject == end2)
        {
            start2.gameObject.SetActive(false);
            end2.gameObject.SetActive(false);
            start1.gameObject.SetActive(true);
            end1.gameObject.SetActive(true);
        }
    }
}
