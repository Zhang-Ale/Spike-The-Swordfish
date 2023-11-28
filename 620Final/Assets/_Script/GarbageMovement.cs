using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageMovement : Enemy
{
    public float Interpolator;
    public Vector3 TargetPosition;
    
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetPosition, Interpolator * Time.deltaTime);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
