using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float forwardForce = 7000f;
    public float sensitivity = 1;
    float rotationX, rotationY;
    public float rotationMin, rotationMax; 
    private float lateralSpeed = 5.0f;

    public GameObject winText, lastObstacle;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
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
        

        /*if (Input.GetKey("d"))
        {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
         if (Input.GetKey("a"))
        {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("w")) { rb.AddForce(0, 10, 0); }
        if (Input.GetKey("s")) { rb.AddForce(0, -20, 0); }*/
    }

/*
    void FixedUpdate()
    {
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);
        //if (movement.x != 0 && Mathf.Sign(movement.x) != Mathf.Sign(transform.localScale.x))
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 0);
            rb.velocity = movement * lateralSpeed;
        }
        
        rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    */

    void LookAround()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, rotationMin, rotationMax); 
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
