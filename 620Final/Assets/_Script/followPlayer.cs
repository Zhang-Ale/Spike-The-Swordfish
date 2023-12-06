using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform followPosition;

    void FixedUpdate()
    {
        followPosition.position = transform.position;
        followPosition.rotation = transform.rotation;
    }
}
