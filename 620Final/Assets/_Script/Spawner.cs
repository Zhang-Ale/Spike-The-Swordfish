using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject goals;
    //public GameObject fishies;
    Vector3 vector;
    const int numGoals = 10;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < numGoals; ++i)
        {
            vector = new Vector3(Random.Range(0f, 2f), Random.Range(0f, 5f), Random.Range(1000f,2000f));
            GameObject newObject = Instantiate(goals, vector, Quaternion.identity);
            FishSchoolScript.goalList.Add(newObject);
            //vector = new Vector3(Random.Range(0f, 1000f), Random.Range(200f, 250f), Random.Range(0f,1000f));
            //newObject = Instantiate(fishies, vector, Quaternion.identity);
        }
    }
}