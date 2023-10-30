using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;

    public float forwardForce = 7000f;
    public float sidewaysForce = 120f;
    public Vector2 rotationRange = new Vector2(.05f, .85f);
    float rotationX, rotationY; 

    public GameObject winText, lastObstacle;
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        LookAround();
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }

        if (Input.GetKey("w")) { rb.AddForce(0, 10, 0); }

        if (Input.GetKey("s")) { rb.AddForce(0, -20, 0); }
    }

    void LookAround()
    {
        rotationX += Input.GetAxis("Mouse X") * sidewaysForce;
        rotationY += Input.GetAxis("Mouse Y") * sidewaysForce;
        transform.localRotation = Quaternion.Euler(-rotationY, rotationX, 0); 
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            this.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Garbage")
        {
            Destroy(other.gameObject);
        }

        if (other.gameObject == lastObstacle)
        {
            winText.SetActive(true);
        }
    }
}
