using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform cameraPosition;

    void FixedUpdate()
    {
        transform.position = cameraPosition.position;
    }
}
