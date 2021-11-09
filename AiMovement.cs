using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiMovement : MonoBehaviour
{
    [Header("Type")]
    public CharacterType type;

    [Header("Stats")]
    public float moveSpeed;
    public float rotationSpeed;
    public float closeDistance;

    [Header("References")]
    public Animator animator;
    public Rigidbody rb;
    public NavMeshAgent agent;
    public AiAttack aiAttack;
    public Transform target;
    public Vector3 directionToTarget;
    public Vector3 targetDestination;
    public GameObject spawnPosition;

    [Header("Doll Exclusive")]
    public bool attackMode;
    public float distanceToAttack = 8;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        aiAttack = GetComponent<AiAttack>();

        spawnPosition = new GameObject();
        spawnPosition.transform.position = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
        Movespeed();
        GetTarget();
        Behavior();
        LookTowardsTarget();
    }

    void MovementAnimation()
    {
        float square = agent.desiredVelocity.sqrMagnitude;

        float v = Mathf.Clamp(square, 0, .5f);

        animator.SetFloat("Vertical", v, 0.2f, Time.deltaTime);
    }

        public void LookTowardsTarget()
    {
        if (aiAttack.isAttacking && type == CharacterType.Doll)
            return;

        directionToTarget = target.position - transform.position;
        Vector3 dir = directionToTarget;
        dir.y = 0;
        if (dir == Vector3.zero)
            dir = transform.forward;

        Quaternion targetRotation = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void Movespeed()
    {
        if (aiAttack.isAttacking)
            agent.speed = 0f;
        else
            agent.speed = moveSpeed;
    }

    void GetTarget()
    {
        switch(type)
        {
            case CharacterType.Doll:
                if (attackMode)
                {
                    if (PlayerCharacter.Instance.m.isLockedOn)
                        target = PlayerCharacter.Instance.m.target.transform;
                    else
                    {
                        //If Enemies are close by, if not just follow the player.
                        if (EnemiesInScene.Instance.enemiesInScene.Count > 0)
                        {

                            for (int i = 0; i < EnemiesInScene.Instance.enemiesInScene.Count; i++)
                            {
                                float distance = Vector3.Distance(EnemiesInScene.Instance.enemiesInScene[i].transform.position, transform.position);

                                if (distance < distanceToAttack)
                                {
                                    target = EnemiesInScene.Instance.enemiesInScene[i].transform;
                                }
                                else
                                {
                                    if (target == EnemiesInScene.Instance.enemiesInScene[i].transform)
                                    {
                                        if (PlayerCharacter.Instance.m.target != null)
                                            target = PlayerCharacter.Instance.m.target.GetComponent<Enemy>().transform;
                                        else
                                            target = PlayerCharacter.Instance.dollFollowPosition;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (PlayerCharacter.Instance.m.target != null)
                                target = PlayerCharacter.Instance.m.target.transform;
                        }
                    }
                }
                else
                {
                    target = PlayerCharacter.Instance.dollFollowPosition;
                    if (attackMode)
                        attackMode = false;
                }
                break;

            case CharacterType.Enemy:
                if (attackMode)
                {
                    target = PlayerCharacter.Instance.transform;
                }
                else
                {
                    target = spawnPosition.transform;
                }

                break;

        }

    }

    float CheckDistance()
    {
        float distance = Vector3.Distance(target.position, this.transform.position);
        return distance;
    }

    float CheckDistanceFromPlayer()
    {
        float distance = Vector3.Distance(PlayerCharacter.Instance.transform.position, this.transform.position);
        return distance;
    }

    float CheckDistanceForFromTarget()
    {
        float distance = Vector3.Distance(targetDestination, target.position);
        return distance;
    }


    void Behavior()
    {
        switch(type)
        {
            case CharacterType.Doll:
               /* if (CheckDistanceForFromTarget() > aiAttack.attackDistance || CheckDistance() > aiAttack.attackDistance)
                    GoToTarget();

                if(CheckDistanceForFromTarget() < aiAttack.attackDistance)
                {
                    if (attackMode)
                        aiAttack.Attack();
                    else
                        StopAgent();
                }*/

                GoToDestination();

                if (CheckDistance() < aiAttack.attackDistance)
                {
                    if (attackMode)
                        aiAttack.Attack();
                    else
                        StopAgent();
                }
                else
                    ResumeAgent();
                    
                break;

            case CharacterType.Enemy:

                if (PlayerCharacter.Instance.isDead || aiAttack.isAttacking)
                {
                    StopAgent();
                    return;
                }

                GoToDestination();

                if (CheckDistanceFromPlayer() < closeDistance)
                {
                    GoToDestination();
                    attackMode = true;

                    if (CheckDistanceFromPlayer() < aiAttack.attackDistance)
                    {
                        StopAgent();
                        aiAttack.Attack();
                    }
                    else
                    {
                        ResumeAgent();
                        GoToDestination();
                    }
                }
                else
                {
                    attackMode = false;
                }

                break;
        }
    }

    #region Controls

    public void Return()
    {
        attackMode = false;
        directionToTarget = Vector3.zero;
        targetDestination = Vector3.zero;
    }

    public void SetTarget(Transform tp)
    {
        target = tp;
    }

    void GoToTarget()
    {
        ResumeAgent();
        SetDestination(target.position);
        GoToDestination();
    }

    void SetDestination(Vector3 d)
    {
        agent.SetDestination(d);
        targetDestination = d;
    }

    void GoToDestination()
    {
        agent.SetDestination(target.position);
    }

    void ResumeAgent()
    {
        agent.isStopped = false;

    }

    void StopAgent()
    {
        agent.isStopped = true;
    }

    #endregion
}
