using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapPosition : MonoBehaviour
{
    public GameObject minimapIcon; 

    void Update()
    {
        minimapIcon.transform.position = new Vector3(transform.position.x, transform.position.y +0.3f, transform.position.z);
    }
}
