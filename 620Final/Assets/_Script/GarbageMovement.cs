using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageMovement : MonoBehaviour
{
    public float Interpolator;
    public Vector3 TargetPosition;
    private void Start()
    {
        //TargetPosition = new Vector3(transform.position.x - 12.6f, transform.position.y, transform.position.z);
    }
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Interpolator * Time.deltaTime);
    }
}
