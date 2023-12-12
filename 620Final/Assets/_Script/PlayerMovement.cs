using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{ 
    Transform t;
    Rigidbody rb;
    public static bool inWater;
    public static bool isSwimming;
    public static bool isSpeeding;
    public static bool isPoisoned;
    [Header("Player Status")]
    bool poisoned;
    public bool changeMoveMode;
    public LayerMask waterMask;
    [Header("Player Movement")]
    public float forwardForce;
    public float sensitivity = 1f;
    float rotationX, rotationY;
    public float rotationMin, rotationMax;
    public float speed = 1f;
    float currentSpeed; 
    [SerializeField]float moveX, moveY, moveZ;
    Vector3 clampedDirection;
    Animator anim;
    public Animator anim1; 
    float holdTime; 
    public float holdLength = 2f; 
    bool isHoldActive = false;
    float hold;
    bool canSwimFast;
    [Header("Player Attack")]
    float attackRate = 1.5f;
    float nextAttackTime = 0f;
    public Transform attackHitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    GameObject fishModel;
    [Header("Others")]
    public SkinnedMeshRenderer MR;
    bool attacking;
    public GameObject minimapIcon;
    public GameObject tip1, tip2, tip3;
    public bool metCurrent;
    public bool metGarbage;
    public bool dead; 

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        t = this.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        inWater = false;
        anim = transform.GetChild(0).GetComponent<Animator>();
        fishModel = transform.GetChild(0).GetComponent<GameObject>();
        currentSpeed = speed;
        hold = 100f;
        metCurrent = false; 
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

        if (hold >= 0f){
            canSwimFast = true;
        }
        if (hold <= 0f)
        {
            canSwimFast = false;
        }

        if (poisoned)
        {
            anim1.SetBool("Poisoned", true);
            StartCoroutine("Poisoned");
        }
        else
        {
            anim1.SetBool("Poisoned", false);
        }
        minimapIcon.transform.position = new Vector3(transform.position.x, minimapIcon.transform.position.y, transform.position.z);
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
            SpeedUp();
            Move();
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Attack") &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            attacking = true;
        }
        else
        {
            attacking = false;
        }
    }

    IEnumerator IncreaseHold(float amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (hold <= 97.5f)
            {
                hold = Mathf.Max(hold + amount, 0);
            }
        }
    }

    IEnumerator DecreaseHold(float amount)
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (hold >= 2.5f)
            {
                hold = Mathf.Max(hold - amount, 0);
            }
        }
    }

    IEnumerator Poisoned()
    {
        tip2.SetActive(true);
        yield return new WaitForSeconds(10.5f);
        MR.materials[1].DOColor(new Color(0.2f, 0.4f, 0.62f, 1f), 2);
        isPoisoned = false;
        poisoned = false;
        tip2.SetActive(false);
    }


    void SwitchMovement()
    {
        inWater = !inWater;
        rb.useGravity = !rb.useGravity;
    }

    void SpeedUp()
    {
        if (canSwimFast)
        {
            if (Input.GetButtonDown("SpeedUp") && !isHoldActive)
            {
                isSpeeding = true;
                StopAllCoroutines();
                anim.SetTrigger("SuddenSpeedUp");
                StartCoroutine("StartCounting");
                StartCoroutine(DecreaseHold(5f));
                speed = 40f;
                StartCoroutine("Sprint");
            }
            if (isHoldActive && holdTime >= holdLength)
            {
                speed = 15f;
                currentSpeed = speed;
                anim.SetBool("SwimFast", true);
                StopCoroutine("StartCounting");
                isHoldActive = false;
            }
        }
        else
        {
            anim.SetBool("SwimFast", false);
        }
        
        if (Input.GetButtonUp("SpeedUp"))
        {
            StopAllCoroutines();
            anim.SetBool("SwimFast", false);
            StopCoroutine("StartCounting");
            StartCoroutine(IncreaseHold(5f));
            speed = 6f;
            currentSpeed = speed;
            isHoldActive = false;
            isSpeeding = false;
        }
    }

    IEnumerator Sprint()
    {
        while(speed > currentSpeed)
        {
            speed -= 2f;
            yield return new WaitForSeconds(.05f);
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
        rb.AddForce(Input.GetAxis("Forward") * 0.25f, Input.GetAxis("Forward") * 0.25f, forwardForce);
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

    public float forceMultiplicator = 100;
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
                //rb.AddForce(new Vector3(moveX, moveY, moveZ) * Time.deltaTime * speed * forceMultiplicator, ForceMode.Force);
            }

            if (!attacking)
            {
                if (Input.GetButton("Horizontal") || Input.GetButton("Forward") || Input.GetButton("Vertical"))
                {
                    anim.SetBool("Swim", true);
                }
                else if((!Input.GetButton("Horizontal") || !Input.GetButton("Forward") || !Input.GetButton("Vertical")))
                {
                    anim.SetBool("Swim", false);
                }
            }            
        }
    }

    private void WASDMove()
    {
        if (Input.GetKey("d"))
        {
            rb.AddForce(30 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("a"))
        {
            rb.AddForce(-30 * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        if (Input.GetKey("w")) { rb.AddForce(0, 30 * Time.deltaTime, 0); }
        if (Input.GetKey("s")) { rb.AddForce(0, -30 * Time.deltaTime, 0); }
    }

    void Attack()
    {
        attacking = true; 
        anim.SetTrigger("Attack");
        Collider[] hitEnemies = Physics.OverlapSphere(attackHitBox.position, attackRange, enemyLayers);
        foreach (Collider enemy in hitEnemies)
        {
            var damageable = enemy.GetComponent<IDamageable>();
            damageable.TakeDamage(attackDamage);
        }
    }

    public void Death()
    {
        anim.SetTrigger("Dead");
        dead = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Garbage")
        {
            isPoisoned = true;
            poisoned = true;
        }

        if (other.gameObject.name == "Underwater")
        {
            UnderwaterEffect.Instance.effectActivate = true;
            SwitchMovement();
        }

        if (other.gameObject.tag == "Ring")
        {
            //sudden AddForce to the rigidbody
            //fishModel.transform.Rotate(Vector3.forward * 360 * Time.deltaTime);
            //rotate the fishModel.rotation
            //enable a sprint particle system
        }

        if (other.gameObject.tag == "Teleport")
        {
            tip1.SetActive(true);
        }

        if (other.gameObject.tag == "StartTrail")
        {
            tip3.SetActive(true);
            metCurrent = true; 
        }

        if (other.gameObject.tag == "EndTrail")
        {
            tip3.SetActive(false);
            metCurrent = false; 
        }

        if(other.gameObject.name == "StopTrigGarbageMusic")
        {
            metGarbage = false; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "TrigGarbageMusic")
        {
            metGarbage = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Underwater")
        {
            UnderwaterEffect.Instance.effectActivate = false;
            SwitchMovement();
        }

        if (other.gameObject.tag == "Teleport")
        {
            tip1.SetActive(false);
        }
    }
}
