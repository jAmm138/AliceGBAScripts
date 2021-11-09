using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnManager : MonoBehaviour
{
    public LockOnTarget[] initTargets;
    public List<LockOnTarget> targets;
    public List<LockOnTarget> avaiableTargets;
    public float distance = 2f;
    public bool hasTarget;
    public bool quitLockOn;
    public Cinemachine.CinemachineTargetGroup targetGroup;
    public int targetIndex;

    public static LockOnManager Instance;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        initTargets = FindObjectsOfType<LockOnTarget>();

        foreach(LockOnTarget lot in initTargets)
        {
            if (!targets.Contains(lot))
                targets.Add(lot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetTarget();
        SwitchTarget();
    }

    void GetTarget()
    {
        if (avaiableTargets.Count < 0)
            return;

        if(Input.GetButton("Lock On"))
        {
            PlayerCharacter.Instance.m.target.transform.position = avaiableTargets[targetIndex].transform.position;
            PlayerCharacter.Instance.m.isLockedOn = true;

            if (!hasTarget)
            {
                targetGroup.AddMember(PlayerCharacter.Instance.m.target.transform, 1, 0.5f);
                hasTarget = true;
            }
        }
        else
        {
            RemoveTarget();
        }
    }

    void SwitchTarget()
    {
        if (avaiableTargets.Count < 1)
            return;

        if (Input.GetButtonDown("Switch Target"))
        {
            targetIndex++;

            if(targetIndex > avaiableTargets.Count - 1)
            {
                targetIndex = 0;
            }
        }

    }

    public void RemoveTarget()
    {
        if (hasTarget)
        {
            targetGroup.RemoveMember(PlayerCharacter.Instance.m.target.transform);
            PlayerCharacter.Instance.m.target.transform.position = Vector3.zero;
            targetIndex = 0;
            hasTarget = false;
            PlayerCharacter.Instance.m.isLockedOn = false;
        }
        else
        {
            targetIndex = 0;
            PlayerCharacter.Instance.m.isLockedOn = false;
        }
    }
}
