using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventPlayerData 
{
    [Header("Event Player Data")]
    [Space]
    [Header("Distance Settings")]
    public bool requiresDistance;
    public bool awayFromEventDistance;
    public bool runsRegardlessOfDistance;
    public float requiredDistance;

    [HideInInspector]
    public bool isActive;
    [Space]
    [Header("Input and Index Settings")]
    public bool requiresInputToProceed;
    public bool loopEvent;

    [Header("Player Settings")]
    public bool cannotRunWithAttack;

    [HideInInspector]
    public int index;

    public EventIndexType indexType; //This is mainly for interactions.
    [Space]
    [Header("Events")]
    public UnityEventDelay[] events;
}

public enum EventIndexType
{
    automated, inputIndex, extTrigger
}
