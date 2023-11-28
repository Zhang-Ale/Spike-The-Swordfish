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
    public float sensitivity = 1f;
    float rotationX, rotationY;
    public float rotationMin, rotationMax;
    [Header("Player Movement")]
    public float speed = 1f;
    [SerializeField]float moveX, moveY, moveZ;
    Vector3 clampedDirection;
    public float RunMultiplier = 2f; 
    public bool changeMoveMode;
    Animator anim;
    float holdTime; 
    public float holdLength = 2f; 
    bool isHoldActive = false;
    public float attackRate = 1.5f;
    float nextAttackTime = 0f;
    public Transform attackHitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inWater = false;
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    private void Update()
    {
        LookAround();
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }

    public void FixedUpdate()
    {
        SwimmingOrFloating();

        if (changeMoveMode)
        {
            MoveForward();
            WASDMove();
        }
        else
        {
            Move();
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
        if (Input.GetButtonDown("SpeedUp") && !isHoldActive)
        {
            isSpeeding = true;
            anim.SetTrigger("SuddenSpeedUp");
            StartCoroutine("StartCounting");
            //this code below is not working!!
            t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * RunMultiplier, Space.World); 
        }

        if (isHoldActive && holdTime >= holdLength)
        {
            anim.SetBool("SwimFast", true);
            StopCoroutine("StartCounting");
            isHoldActive = false;
        }

        if (Input.GetButtonUp("SpeedUp"))
        {
            anim.SetBool("SwimFast", false);
            StopCoroutine("StartCounting");
            isHoldActive = false;
            isSpeeding = false;
        }
    }
    IEnumerator StartCounting()
    {
        for (holdTime = 0f; holdTime <= holdLength; holdTime += Time.deltaTime)
        {
            yield return new WaitForSeconds(Time.deltaTime);     
        }
        isHoldActive = true;
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
        if (inWater)
        {
            rb.mass = 2f;
        }
        else
        {
            rb.mass = 0.7f;
            if (isSwimming)
            {
                moveY = Mathf.Min(moveY, 0);
                clampedDirection = t.TransformDirection(new Vector3(moveX, moveY, moveZ));
                clampedDirection = new Vector3(clampedDirection.x, Mathf.Min(clampedDirection.y, 0), clampedDirection.z);
                t.Translate(clampedDirection * Time.deltaTime * speed, Space.World);
            }
            else
            {
                t.Translate(new Vector3(moveX, 0, moveZ) * Time.deltaTime * speed);
                t.Translate(new Vector3(0, moveY, 0) * Time.deltaTime * speed, Space.World);
            }
        }

        SpeedUp();
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

    void Attack()
    {
        anim.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            var damageable = enemy.GetComponent<IDamageable>();
            damageable.TakeDamage(attackDamage);
        }
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
