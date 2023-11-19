using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolScript : MonoBehaviour
{
    public static List<GameObject> goalList = new List<GameObject>();
    const int avgNumFishInSchool = 8;
    [SerializeField] int i;
    const float speed = 10f;
    Vector3 targetDirection, vector;
    public GameObject fish;
    GameObject newFish;

    void Start()
    {
        int numFishInSchool = Random.Range(avgNumFishInSchool - 3, avgNumFishInSchool + 4);
        for(int i = 0; i < numFishInSchool; ++i)
        {
            vector = new Vector3(this.transform.position.x + Random.Range(-2f, 2f),
                this.transform.position.y + Random.Range(-1f, 1f),
                this.transform.position.z + Random.Range(-3f, 3f));
            newFish = Instantiate(fish, vector, Quaternion.identity);
            newFish.transform.SetParent(this.transform);
        }
        i = Random.Range(0, goalList.Count);
        targetDirection = goalList[i].transform.position - this.transform.position;
        this.transform.LookAt(targetDirection);
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goalList[i].transform.position,
             speed * Time.deltaTime);

        if(Vector3.Distance(goalList[i].transform.position, this.transform.position) < 1)
        {
            i = Random.Range(0, goalList.Count);
            targetDirection = goalList[i].transform.position - this.transform.position;
            this.transform.LookAt(targetDirection);
        }
    }
}
