using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] protected float _life = 10;
    //static float _allLife = 10;//change life for all enemies (can be used for achievement of how many enemies have hit)
    public virtual void TakeDamage(float damage)
    {
        _life -= damage;
    }
}
