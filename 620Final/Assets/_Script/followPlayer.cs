using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform followPosition;
    public int index;

    void FixedUpdate()
    {
        if (index == 0)
        {
            followPosition.position = transform.position;
            followPosition.rotation = transform.rotation;
        }
        else
        {
            transform.position = new Vector3(followPosition.position.x, 10, followPosition.position.z); 
            transform.rotation = transform.rotation; 
        }
        
    }
}
