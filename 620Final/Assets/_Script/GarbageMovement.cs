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
        if(_life <= 0)
        {
            //instantiate 
            //1. petroil smoke particles 
            //2. gameobjects of separate garbage pieces 
            //3. trash remainants on the sea bed ground 
            Destroy(this.gameObject, 0.25f);
        }
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }
}
