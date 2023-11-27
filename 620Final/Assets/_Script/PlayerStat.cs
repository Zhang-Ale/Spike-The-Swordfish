using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStat : MonoBehaviour
{
    public List<int> maxStats;
    List<int> currentStats;
    Coroutine healthCo;
    Coroutine staminaDeCo;
    Coroutine staminaInCo;
    bool poisonedCheck;
    bool speedingCheck; 
    public List<Slider> statBars;
    public List<TextMeshProUGUI> statNums;
    public PlayerMovement PM;

    void Start()
    {
        currentStats = new List<int>(maxStats);
        healthCo = StartCoroutine(DecreaseStats(0, 3, 0));
        staminaDeCo = StartCoroutine(DecreaseStats(1, 1, 0));
        staminaInCo = StartCoroutine(IncreaseStats(1, 2, 0));
        for (int i = 0; i < maxStats.Count; i++)
        {
            statBars[i].maxValue = maxStats[i];
        }
    }

    void Update()
    {
        if (!PlayerMovement.isPoisoned && poisonedCheck && healthCo!=null)
        {
            poisonedCheck = false;
            StopCoroutine(healthCo);
            ChangeStat(0, maxStats[0]);
        }
        if (PlayerMovement.isPoisoned && !poisonedCheck)
        {
            poisonedCheck = true;
            healthCo = StartCoroutine(DecreaseStats(0, 3, 3)); 
        }

        if (!PlayerMovement.isSpeeding && speedingCheck && staminaDeCo !=null)
        {
            speedingCheck = false;
            StopCoroutine(staminaDeCo);
            staminaInCo = StartCoroutine(IncreaseStats(1, 1, 5));
        }
        if (PlayerMovement.isSpeeding && !speedingCheck)
        {
            speedingCheck = true;
            staminaDeCo = StartCoroutine(DecreaseStats(1, 1, 5));
            StopCoroutine(staminaInCo);
        }

        for(int i = 0; i<maxStats.Count; i++)
        {
            statBars[i].value = currentStats[i];
            statNums[i].text = currentStats[i].ToString() + "/100";
        }

        if(statBars[0].value == 0)
        {
            PM.Death();
        }
    }

    IEnumerator DecreaseStats(int stat, int interval, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if(currentStats[stat] >= 0)
            {
                currentStats[stat] = Mathf.Max(currentStats[stat]-amount, 0);
            }
        }
    }

    IEnumerator IncreaseStats(int stat, int interval, int amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            if (currentStats[stat] < maxStats[stat] - 5)
            {
                currentStats[stat] = Mathf.Max(currentStats[stat] + amount, 0);
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
