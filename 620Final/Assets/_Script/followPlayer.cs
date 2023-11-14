
using UnityEngine;

public class followPlayer : MonoBehaviour
{

    //public GameObject player;
    //private Vector3 camPosition;
    public Transform cameraPosition;

    void Start()
    {
        //player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //camPosition = new Vector3(player.transform.position.x, player.transform.position.y + 3.5f, player.transform.position.z - 2f);
        //this.transform.position = camPosition;
        transform.position = cameraPosition.position;
    }
}
