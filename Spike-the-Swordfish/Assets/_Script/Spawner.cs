using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner spawn;
    public float progress;
    public bool isDone; 
    public GameObject goals;
    public GameObject fishSchools;
    GameObject newObject;
    Vector3 vector;
    const int numGoals = 10;
    public const float xMin = -10f, xMax = 10f, yMax = 5f, zMax = 2000f;

    void Start()
    {
        spawn = this;
        isDone = false; 
        StartCoroutine("SpawnFish"); 
    }
    IEnumerator SpawnFish()
    {
        int count = 0; 
        for(count = 0; count < numGoals; ++count)
        {
            vector = new Vector3(Random.Range(xMin, xMax), Random.Range(0f, yMax), Random.Range(0f, zMax));
            newObject = Instantiate(goals, vector, Quaternion.identity);
            FishSchoolScript.goalList.Add(newObject);
            vector = new Vector3(Random.Range(xMin, xMax), Random.Range(0f, yMax), Random.Range(0f, zMax));
            newObject = Instantiate(fishSchools, vector, Quaternion.identity);
            progress = ((float)count / (float)numGoals); 
        }
        yield return new WaitForSeconds(0.5f);
        isDone = true; 
    }
}