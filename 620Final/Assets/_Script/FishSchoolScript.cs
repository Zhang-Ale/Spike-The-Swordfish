using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolScript : MonoBehaviour
{
    public static List<GameObject> goalList = new List<GameObject>();
    const int avgNumFishInSchool = 8; //Average school size
    const int schoolVariance = 3; //A school can have the average plus or minus this number of fish
    const float xOffsetMax = 2f;
    const float yOffsetMax = 1f;
    const float zOffsetMax = 3f;
    [SerializeField] int i;
    const float speed = 10f;
    const float fleeRadius = 5f;
    Vector3 targetDirection, vector;
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
        targetDirection = goalList[i].transform.position - this.transform.position;
        this.transform.LookAt(targetDirection);
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, player.transform.position) < fleeRadius) //Run away from player
        {
            Vector3 fleeDirection = (this.transform.position - player.transform.position).normalized;
            this.transform.position = Vector3.MoveTowards(this.transform.position, fleeDirection * fleeRadius,
                speed * Time.deltaTime);
            this.transform.LookAt(fleeDirection);
        }
        else if(Vector3.Distance(goalList[i].transform.position, this.transform.position) < 1) //Switch goals if close to a goal
        {
            i = Random.Range(0, goalList.Count);
            targetDirection = (goalList[i].transform.position - this.transform.position).normalized;
            this.transform.LookAt(targetDirection);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, goalList[i].transform.position,
                speed * Time.deltaTime);
            this.transform.LookAt(targetDirection);
        }
    }
}