using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterEffect : MonoBehaviour
{
    public bool effectActivate; 

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
