using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolScript : MonoBehaviour
{
    public static List<GameObject> goalList = new List<GameObject>(); //List of invisible goals
    const int avgNumFishInSchool = 8, schoolVariance = 4; //Average school size; a school can have the average plus or minus this number of fish
    const float xOffsetMax = 2f, yOffsetMax = 1f, zOffsetMax = 3f;
    const float speed = 10f, fleeSpeed = 20f, fleeRadius = 30f;
    const float xMin = -10f, xMax = 10f, yMax = 5f, zMax = 2000f; //Level dimensions, allowing for a little extra room for the fish
    [SerializeField] int i;
    [SerializeField] Vector3 goal;
    Vector3 vector;
    public GameObject fish;
    public GameObject player;
    GameObject newFish;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        int numFishInSchool = Random.Range(avgNumFishInSchool - schoolVariance, avgNumFishInSchool + schoolVariance + 1);
        for(int i = 0; i < numFishInSchool; ++i)
        {
            vector = new Vector3(this.transform.position.x + Random.Range(-xOffsetMax, xOffsetMax),
                this.transform.position.y + Random.Range(-yOffsetMax, yOffsetMax),
                this.transform.position.z + Random.Range(-zOffsetMax, zOffsetMax));
            newFish = Instantiate(fish, vector, Quaternion.identity);
            newFish.transform.SetParent(this.transform);
        }
        i = Random.Range(0, goalList.Count);
        goal = goalList[i].transform.position;
        this.transform.LookAt(goalList[i].transform);
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < fleeRadius) //Run away from player
        {
            Vector3 fleeDirection = (this.transform.position - player.transform.position).normalized;

            Vector3 fleePoint = this.transform.position + fleeDirection * fleeRadius;
            /*
            if(fleePoint.x < xMin)
                fleePoint.x = xMin;
            else if(fleePoint.x > xMax)
                fleePoint.x = xMax;
            if(fleePoint.y < 0f)
                fleePoint.y = 0f;
            else if(fleePoint.y > yMax)
                fleePoint.y = yMax;
            */
            
            this.transform.position = Vector3.MoveTowards(this.transform.position, fleePoint,
                fleeSpeed * Time.deltaTime);
            this.transform.LookAt(fleePoint);
        }
        else if(Vector3.Distance(goalList[i].transform.position, this.transform.position) < 1) //Switch goals if close to a goal
        {
            //Debug.Log("Switching directions.");
            i = Random.Range(0, goalList.Count);
            goal = goalList[i].transform.position;
            this.transform.LookAt(goalList[i].transform);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, goalList[i].transform.position,
                speed * Time.deltaTime);
            this.transform.LookAt(goalList[i].transform);
        }
    }
}