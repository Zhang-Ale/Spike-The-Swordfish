using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
}
public abstract class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] float _life = 10;
    public virtual void TakeDamage(float damage)
    {
        _life = Mathf.Max(_life - damage, 0);
    }
}
