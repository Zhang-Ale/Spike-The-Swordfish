using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWakk : MonoBehaviour
{
    private Material _mat;
    private GameObject _player;
    void Start()
    {
        _mat = GetComponent<MeshRenderer>().materials[0];
        _player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        _mat.SetVector("_PlayerPos", _player.transform.position);
    }
}
