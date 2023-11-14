using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnderwaterEffect : MonoBehaviour
{
    [SerializeField]GameObject waterFx;

    private void OnTriggerEnter(Collider other)
    {
            waterFx.gameObject.SetActive(true);
            RenderSettings.fog = true;
    }

    private void OnTriggerExit(Collider other)
    { 
            waterFx.gameObject.SetActive(false);
            RenderSettings.fog = false;
    }
}
