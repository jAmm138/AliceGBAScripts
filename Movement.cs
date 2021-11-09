using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public PlayerCharacter pc; //Player Character hub.
    public Rigidbody rb;
    public float h; //Horizontal 
    public float v; //Vertical
    public float moveSpeed; //The movespeed of the character.
    public float rotateSpeed;
    public bool isLockedOn;
    public float knockback = 5f;
    public GameObject target;
    public Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerCharacter>();
        rb = GetComponent<Rigidbody>();

        rb.angularDrag = 999;
        rb.drag = 4;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueManager.Instance.dialogueActive)
            return;
        pc.onGround = true;

        AnimatorAxis();
        AxisInput();
        Move();
        Rotation();
    }

    void AxisInput()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
    }

    void AnimatorAxis()
    {
        pc.animator.SetBool("IsLockedOn", isLockedOn);

        if (isLockedOn)
        {
            pc.animator.SetFloat("Vertical", h);
            pc.animator.SetFloat("Horizontal", v);
        }
        else
            pc.animator.SetFloat("Vertical", MoveAmount());
        
    }

    //Handles the acclerated start of the player character.
    float MoveAmount()
    {
        float moveA = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));
        return moveA;
    }

    //Controls the player's movement. Handles both normal movement and lock on movement.
    void Move()
    {
        if (pc.isAttacking || pc.takingDamage)
            return;

        Vector3 targetVelocity = Vector3.zero;

        if(isLockedOn)
        {
            targetVelocity = transform.forward * v * moveSpeed;
            targetVelocity += transform.right * h * moveSpeed;
        }
        else
            targetVelocity = transform.forward * MoveAmount() * moveSpeed;

        if(pc.onGround)
        {
            if (MoveAmount() > 0.1f)
            {
                rb.isKinematic = false;
                rb.drag = 0;
            }
            else
            {
                rb.isKinematic = true;
                targetVelocity.y = 0;
                rb.drag = 4;
            }
        }
        else
        {
            rb.isKinematic = false;
            rb.drag = 0;
            targetVelocity.y = rb.velocity.y;
        }

        rb.velocity = targetVelocity;
    }

    float RotationSpeed()
    {
        float currentRotationSpeed;

        if (pc.isAttacking)
            currentRotationSpeed = pc.pa.attackRotSpeed;
        else
            currentRotationSpeed = rotateSpeed;

        return currentRotationSpeed;
    }

    //Controls the player's rotation. Handles both normal rotation and lock on rotation.
    void Rotation()
    {
        if ((pc.isAttacking && pc.pa.disableRotateDuringAttack) || pc.takingDamage)
            return;

        Vector3 direction;

        if(isLockedOn)
        {
            direction = target.transform.position - transform.position;
            direction.Normalize();
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            direction = cam.transform.forward * v;
            direction += cam.transform.right * h;
            direction.Normalize();

            direction.y = 0;
            if (direction == Vector3.zero)
                direction = transform.forward;

            Quaternion tr = Quaternion.LookRotation(direction);
            Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * MoveAmount()
                * RotationSpeed());

            transform.rotation = targetRotation;

        }
    }

    public void Knockback()
    {
        Vector3 targetVelocity = Vector3.zero;
        targetVelocity = -transform.forward * knockback;
        rb.velocity += targetVelocity;
    }
}
