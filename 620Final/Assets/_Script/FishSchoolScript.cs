using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSchoolScript : MonoBehaviour
{
    public static List<GameObject> goalList = new List<GameObject>();
    [SerializeField] int i;
    const float speed = 10f;
    Vector3 targetDirection;
    Vector3 newDirection;
    void Start()
    {
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
