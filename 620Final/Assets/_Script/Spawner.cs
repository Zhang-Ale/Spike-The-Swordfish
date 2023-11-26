using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject goals;
    public GameObject fishSchools;
    GameObject newObject;
    Vector3 vector;
    const int numGoals = 10;
    public const float xMin = -10f, xMax = 10f, yMax = 5f, zMax = 2000f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numGoals; ++i)
        {
            vector = new Vector3(Random.Range(xMin, xMax), Random.Range(0f, yMax), Random.Range(0f, zMax));
            newObject = Instantiate(goals, vector, Quaternion.identity);
            FishSchoolScript.goalList.Add(newObject);
            vector = new Vector3(Random.Range(xMin, xMax), Random.Range(0f, yMax), Random.Range(0f, zMax));
            newObject = Instantiate(fishSchools, vector, Quaternion.identity);
        }
    }
}