using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterType characterInParent;
    public Transform parent;
    public Rigidbody rb;
    public new Collider collider;
    public bool isProjectile;
    public bool ready;
    public bool active;
    public float damageOutput;
    public float returnTimer;
    public float returnTimerMax;

    #region Init
    public void Init()
    {
        //Collision
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
        //Layers

        rb = GetComponent<Rigidbody>();

        //if (rb == null)
       //     return;

        //rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        //Enables the collider ready for use so no collision mistakes occur.
        ready = true;

        //Finds Parent Script, which character type is owned by this collider.
        /*if (this.gameObject.GetComponentInParent<PlayerCharacter>())
        {
            characterInParent = CharacterType.Player;
            parent = this.gameObject.GetComponentInParent<PlayerCharacter>().pa.projectileParent;
            Rigid();
        }
        else if (GetComponentInParent<Doll>())
        {
            characterInParent = CharacterType.Doll;
            parent = GetComponentInParent<Doll>().projecitleParent;
            Rigid();
        }
        else if (GetComponentInParent<Enemy>())
        {
            characterInParent = CharacterType.Enemy;
            parent = GetComponentInParent<Enemy>().projectileParent;
            Rigid();
        }*/

    }

    #endregion

    #region Updating

    void Update()
    {
        ReturnTimer();
    }

    void ReturnTimer()
    {
        if(active)
        {
            returnTimer += Time.deltaTime;

            if(returnTimer >= returnTimerMax)
            {
                ReturnToCharacter();
            }
        }
    }

    #endregion

    #region Triggers

    void OnTriggerEnter(Collider other)
    {
        if (!ready)
            return;

        switch(characterInParent)
        {
            case CharacterType.Player:

                //Checks if Enemy is hit, dependant on Layer system.
                if (other.gameObject.layer == 10 && other.gameObject.GetComponent<Enemy>())
                {
                    switch(PlayerCharacter.Instance.mode)
                    {
                        case Mode.Melee:
                            other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, true, true);
                            break;
                        case Mode.LongRange:
                            other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, false, true);
                            break;
                    }
                    /*if (isProjectile)
                    {
                        other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, false, true);
                        ReturnToCharacter();
                    }
                    else
                        other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, true, true);*/
                }
                break;

            case CharacterType.Doll:
                if (other.gameObject.layer == 10 && other.gameObject.GetComponent<Enemy>())
                {
                    if (isProjectile)
                    {
                        other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, false, false);
                        ReturnToCharacter();
                        parent.GetComponentInParent<Doll>().Return();
                    }
                    else
                    {
                        other.gameObject.GetComponent<Enemy>().TakeDamage(damageOutput, true, false);
                        parent.GetComponentInParent<Doll>().Return();
                    }
                }

                break;

            case CharacterType.Enemy:
                {
                    if (other.gameObject.layer == 8 && other.gameObject.GetComponent<PlayerCharacter>())
                    {
                        other.gameObject.GetComponent<PlayerCharacter>().TakeDamage(damageOutput);
                    }
                }
                break;

        }
    }

    void ReturnToCharacter()
    {
        returnTimer = 0;
        active = false;
        this.gameObject.transform.SetParent(parent);
        this.gameObject.SetActive(false);
    }

    #endregion
}
