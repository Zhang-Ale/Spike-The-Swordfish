using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Transform t;
    Rigidbody rb;
    public static bool inWater;
    public static bool isSwimming;
    public static bool isSpeeding;
    public static bool isPoisoned;
    public LayerMask waterMask; 
    public float forwardForce;
    public float sensitivity = 1;
    float rotationX, rotationY;
    public float rotationMin, rotationMax;
    [Header("Player Movement")]
    public float speed = 1;
    [SerializeField]float moveX, moveY, moveZ;
    public bool changeMoveMode;
    public Animator anim;
    bool canAttack;
    bool swimFast; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inWater = false;
    }

    private void Update()
    {
        LookAround();
    }

    public void FixedUpdate()
    {
        SwimmingOrFloating();
        if (Input.GetButtonDown("Attack") && canAttack)
        {
            Attack();
        }

        if (changeMoveMode)
        {
            MoveForward();
            WASDMove();
        }
        else
        {
            Move();
            SpeedUp();
            Debug.Log("isSpeeding = " + isSpeeding);
        }
    }

    void SwitchMovement()
    {
        inWater = !inWater;
        rb.useGravity = !rb.useGravity;
    }

    void SpeedUp()
    {
        if (Input.GetButton("SpeedUp"))
        {
            isSpeeding = true;
            anim.SetTrigger("SuddenSpeedUp");

            //calculate amount of time pressing the button to set swimFast true or false
            if (swimFast)
            {
                anim.SetBool("SwimFast", true);
            }
            else {
                anim.SetBool("SwimFast", false);
            }           
        }

        if (Input.GetButtonUp("SpeedUp"))
        {
            isSpeeding = false;
        }
    }

    void SwimmingOrFloating()
    {
        bool swimCheck = false;
        if (inWater)
        {
            RaycastHit hit;
            if (Physics.Raycast(new Vector3(t.position.x, t.position.y + 0.5f, t.position.z), Vector3.down, out hit, Mathf.Infinity, waterMask))
            {
                if (hit.distance < 0.1f)
                {
                    swimCheck = true;
                }
            }
            else
            {
                swimCheck = false;
            }
            isSwimming = swimCheck; 
        }
    }

    private void MoveForward()
    {
        rb.AddForce(Input.GetAxis("Forward") * 0.25f, Input.GetAxis("Forward") * 0.25f,
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
        if (!inWater)
        {
            rb.mass = 2f;
        }
        else
        {
            rb.mass = 0.7f;
            if (isSwimming)
            {
                moveY = Mathf.Min(moveY, 0);
                Vector3 clampedDirection = t.TransformDirection(new Vector3(moveX, moveY, moveZ));
                clampedDirection = new Vector3(clampedDirection.x, Mathf.Min(clampedDirection.y, 0), clampedDirection.z);
                t.Translate(clampedDirection * Time.deltaTime * speed, Space.World);
            }
            else
            {
                t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed);
                t.Translate(new Vector3(0, moveY, 0) * Time.deltaTime * speed, Space.World);
            }
        }
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

    private void Attack()
    {
        anim.SetTrigger("Attack");
        canAttack = false;
        //calculate time for canAttack to turn back to true (GhostBoy)
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Garbage")
        {
            Destroy(other.gameObject);
            isPoisoned = true;
        }

        if(other.gameObject.tag == "Heal")
        {
            Destroy(other.gameObject);
            isPoisoned = false;
        }

        if (other.gameObject.name == "Underwater")
        {
            UnderwaterEffect.Instance.effectActivate = true;
            SwitchMovement();
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Underwater")
        {
            UnderwaterEffect.Instance.effectActivate = false;
            SwitchMovement();
        }
    }
}
