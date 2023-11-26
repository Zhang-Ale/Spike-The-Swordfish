using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform t; 
    Rigidbody rb;
    public float forwardForce;
    public float sensitivity = 1;
    float rotationX, rotationY;
    public float rotationMin, rotationMax;
    private float lateralSpeed = 0.25f;
    [Header("Player Movement")]
    public float speed = 1;
    float moveX, moveY, moveZ, speedUp; 

    public GameObject winText, lastObstacle;
    public bool changeMoveMode;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = this.transform; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        LookAround();
    }

    public void FixedUpdate()
    {
        if (changeMoveMode)
        {
            MoveForward();     
            WASDMove();
        }
        else
        {
            Move();
            speedUp = Input.GetAxis("SpeedUp");
        }
    }

    private void MoveForward()
    {
        rb.AddForce(Input.GetAxis("Forward") * lateralSpeed, Input.GetAxis("Forward") * lateralSpeed,
            forwardForce * Time.deltaTime);
        rb.constraints = RigidbodyConstraints.FreezeRotationX;
        rb.constraints = RigidbodyConstraints.FreezeRotationY;
        rb.constraints = RigidbodyConstraints.FreezeRotationZ;
    }

    private void LookAround()
    {
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, rotationMin, rotationMax); 
        t.localRotation = Quaternion.Euler(-rotationY, rotationX, 0); 
    }

    private void Move()
    {
        rb.constraints = RigidbodyConstraints.None;
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        moveZ = Input.GetAxis("Forward");
        t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed);
        t.Translate(new Vector3(0, moveY, 0) * Time.deltaTime * speed, Space.World);         
    }

    private void WASDMove()
    {
        if (Input.GetKey("d"))
        {
            rb.AddForce(20 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
         if (Input.GetKey("a"))
        {
            rb.AddForce(-20 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("w")) { rb.AddForce(0, 10, 0); }
        if (Input.GetKey("s")) { rb.AddForce(0, -20, 0); }
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
