using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterEffect : MonoBehaviour
{
    public static UnderwaterEffect Instance { get; private set; }
    public bool effectActivate;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (effectActivate)
        {
            RenderSettings.fog = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (effectActivate)
        {
            RenderSettings.fog = false;
        }
    }
}
