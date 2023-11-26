using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    public List<int> maxStats;
    List<int> currentStats;
    Coroutine healthCo; 

    void Start()
    {
        currentStats = new List<int>(maxStats);
        healthCo = StartCoroutine(DecreaseStats(1, 5, 1));
    }

    void Update()
    {
        if (!PlayerMovement.isPoisoned)
        {
            StopCoroutine(healthCo);
            ChangeStat(0, maxStats[0]);
        }
        if (PlayerMovement.isPoisoned)
        {
            healthCo = StartCoroutine(DecreaseStats(0, 3, 3)); 
        }
    }

    IEnumerator DecreaseStats(int stat, int interval, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if(currentStats[stat] > 0)
            {
                currentStats[stat] = Mathf.Max(currentStats[stat]-amount, 0);
            }
        }
    }

    public void ChangeStat(int stat, int refreshAmount)
    {
        if(refreshAmount > 0)
        {
            currentStats[stat] = Mathf.Min(currentStats[stat] + refreshAmount, maxStats[stat]);

        }
        else
        {
            currentStats[stat] = Mathf.Max(currentStats[stat] + refreshAmount, 0);
        }
    }
}
