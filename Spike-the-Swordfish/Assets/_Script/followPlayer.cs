using UnityEngine;

public class followPlayer : MonoBehaviour
{
    public Transform followPosition;
    public int index;

    void FixedUpdate()
    {
        if (index == 0)
        {
            transform.position = followPosition.position;
            transform.rotation = followPosition.rotation;
        }
        else
        {
            transform.position = new Vector3(followPosition.position.x, 50, followPosition.position.z); 
            transform.rotation = transform.rotation; 
        }
        
    }
}
