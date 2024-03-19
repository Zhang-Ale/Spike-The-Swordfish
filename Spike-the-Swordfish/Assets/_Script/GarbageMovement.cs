using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageMovement : Enemy
{
    public float Interpolator;
    public Vector3 TargetPosition;
    PlayerStat PS; 
    public static ObjectPooler op;
    MinimapPosition MP; 

    private void Start()
    {
        PS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
        MP = GetComponent<MinimapPosition>(); 
    }

    void FixedUpdate()
    {
        //transform.position = Vector3.Lerp(transform.position, TargetPosition, Interpolator * Time.deltaTime);
        if(_life <= 0)
        {
            //instantiate petroil smoke particles 
            Destroy(MP.minimapIcon);
            PS.AddXP(10);
            Destroy(this.gameObject);
        }
    }

    public override void TakeDamage(float damage)
    {
        ParticleSystem particle = op.GetParticle();
        particle.transform.position = transform.position;
        particle.Play();
        base.TakeDamage(damage);
    }
}
