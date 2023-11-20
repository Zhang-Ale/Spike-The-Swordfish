using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterEffect : MonoBehaviour
{
    [SerializeField]GameObject waterFx;
    public bool effectActivate; 

    private void OnTriggerEnter(Collider other)
    {
        if (effectActivate)
        {
            waterFx.gameObject.SetActive(true);
            RenderSettings.fog = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (effectActivate)
        {
            waterFx.gameObject.SetActive(false);
            RenderSettings.fog = false;
        }
    }
}
