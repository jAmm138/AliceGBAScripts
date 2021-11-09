using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [Header("States")]
    public bool isMoving;
    public bool isCoolingDown;
    public bool onGround;
    public bool isAttacking;
    public bool isDead;
    public bool changingMode;
    public bool iFrames;
    public bool takingDamage;
    public bool guard;
    public Mode mode;

    [Header("Stats")]
    public float health;
    public float healthMax;
    public float takingDamageTime;
    public float takingDamageTimeMax = 0.7f;

    [Header("Unity References")]
    public Animator animator;

    [Header("Custom References")]
    public Movement m;
    public DollManager dm;
    public PlayerAttack pa;
    public Transform dollFollowPosition;
    public float iFramesTime;
    public float iFramesTimeMax = 1f;
    public ParticleSystem damageParticles;

    public static PlayerCharacter Instance;

    void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        health = healthMax;
        animator = GetComponent<Animator>();
        m = GetComponent<Movement>();
        dm = GetComponent<DollManager>();
        pa = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
        {
            pa.enabled = false;
            m.enabled = false;
            return;
        }

        IFrames();
        TakingDamage();
    }

    void IFrames()
    {
        if (iFrames)
        {
            iFramesTime += Time.deltaTime;

            if (iFramesTime >= iFramesTimeMax)
            {
                iFramesTime = 0;
                iFrames = false;
            }
        }
    }

    void TakingDamage()
    {
        if (takingDamage)
        {
            takingDamageTime += Time.deltaTime;

            if (takingDamageTime >= takingDamageTimeMax)
            {
                takingDamageTime = 0;
                takingDamage = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (!iFrames)
        {
            health -= damage;
            PlayerUI.Instance.UpdateHealth_UI();
            iFramesTime = 0;
            takingDamageTime = 0;
            isAttacking = false;

            if (health > 0)
            {
                AudioManager.Instance.Play("player_damage");
                m.Knockback();
                animator.Play("Damage");
                damageParticles.Play();
                takingDamage = true;
                iFrames = true;
            }
            else
            {
                AudioManager.Instance.Play("player_death");
                damageParticles.Play();
                Death();
            }
        }
    }

    public void AddHealth(float h)
    {
        health += h;
        PlayerUI.Instance.UpdateHealth_UI();

        if (health > healthMax)
        {
            health = healthMax;
        }

    }

    void Death()
    {
        health = 0;
        animator.Play("Death");
        CanvasInstance.Instance.gameOverScreen.SetActive(true);
        isDead = true;
    }

    public void DisableTakingDamage()
    {
        takingDamage = false;
    }
}
