using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform followPosition;

    void FixedUpdate()
    {
        transform.position = followPosition.position;
        transform.rotation = followPosition.rotation;
    }
}
