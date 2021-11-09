using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPlayer : MonoBehaviour
{
    public EventPlayerData[] data;
    public bool delayEvents;

    // Start is called before the first frame update
    void Start()
    {
        foreach (EventPlayerData epd in data)
        {
            if(epd.indexType == EventIndexType.automated && !epd.requiresDistance)
                epd.isActive = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ExecuteByDistance();
        PlayEventData();
        PlayEventDataByInput();
    }

    float RequiredDistance()
    {
        float rDis = Vector3.Distance(PlayerCharacter.Instance.transform.position, this.transform.position);
        return rDis;
    }

    void ExecuteByDistance()
    {
        foreach (EventPlayerData epd in data)
        {
            if (!epd.requiresDistance)
                return;

            if(epd.awayFromEventDistance)
            {
                if (RequiredDistance() > epd.requiredDistance && !epd.isActive)
                {
                    switch (epd.indexType)
                    {
                        case EventIndexType.automated:
                            epd.isActive = true;
                            break;

                        case EventIndexType.inputIndex:
                            if (Input.GetButtonDown("Interaction"))
                            {
                                epd.isActive = true;
                            }
                            break;

                    }
                }
                else if (!epd.runsRegardlessOfDistance)
                {
                    epd.isActive = false;
                }
            }
            else
            {
                if (RequiredDistance() < epd.requiredDistance && !epd.isActive)
                {
                    switch (epd.indexType)
                    {
                        case EventIndexType.automated:
                            epd.isActive = true;
                            break;

                        case EventIndexType.inputIndex:
                            if (Input.GetButtonDown("Interaction"))
                            {
                                epd.isActive = true;
                            }
                            break;

                    }
                }
                else if (!epd.runsRegardlessOfDistance)
                {
                    epd.isActive = false;
                }
            }
            
            /*else if (epd.isActive)
            {
                ResetEvent(epd);
            }*/
            
        }
    }

    void PlayEventData()
    {

        foreach(EventPlayerData epd in data)
        {
            /*switch(epd.indexType)
            {
                case EventIndexType.automated:
                    break;
                case EventIndexType.inputIndex:
                    if(!epd.events[epd.index].eventPlaying)
                    {
                        if(Input.GetButtonDown("Interaction"))
                        {
                            epd.isActive = true;
                        }
                    }

                    break;
            }*/

            RunEventData_Automated(epd);
        }
    }

    void PlayEventDataByInput()
    {
        foreach (EventPlayerData epd in data)
        {
            if (epd.indexType != EventIndexType.inputIndex)
                return;

            if (epd.cannotRunWithAttack && PlayerCharacter.Instance.isAttacking)
                return;

            if (epd.requiresDistance)
                return;

            if (!epd.events[epd.index].eventPlaying)
            {
                if (Input.GetButtonDown("Interaction"))
                {
                    epd.isActive = true;
                }
            }
        }
    }

    /*void StartDelay(EventPlayerData epd)
    {
        if(epd.startDelay > 0)
        {
            delayEvents = true;

            if (delayEvents)
            {
                isActive = false;
                epd.delayTimer += Time.deltaTime;

                if (epd.delayTimer >= epd.startDelay)
                {
                    epd.delayTimer = 0;
                    isActive = true;
                    delayEvents = false;
                }
            }
        }
        else
            isActive = true;
    }*/

    void ResetEvent(EventPlayerData epd)
    {
        epd.index = 0;
    }

    void RunEventData_Automated(EventPlayerData epd)
    {
        if (!epd.isActive)
            return;

        if (epd.index > epd.events.Length - 1)
        {
            epd.index = 0;

            if (epd.loopEvent)
            {
                epd.isActive = true;
            }
            else
            {
                epd.isActive = false;
            }
        }
        else
        {
            epd.events[epd.index].e.Invoke();
            epd.events[epd.index].eventPlaying = true;
            epd.events[epd.index].timer += Time.deltaTime;

            if (epd.events[epd.index].timer >= epd.events[epd.index].delay)
            {
                epd.events[epd.index].timer = epd.events[epd.index].delay;
                epd.events[epd.index].eventPlaying = false;

                if (Input.GetButtonDown("Interaction") && epd.requiresInputToProceed && epd.indexType == EventIndexType.inputIndex)
                {
                    epd.events[epd.index].timer = 0;
                    epd.index++;
                }
                else if (!epd.requiresInputToProceed)
                {
                    epd.events[epd.index].timer = 0;
                    epd.index++;
                }
            }
        }
        

        /*epd.events[epd.index].e.Invoke();
        epd.events[epd.index].eventPlaying = true;
        epd.events[epd.index].timer += Time.deltaTime;

        if(epd.events[epd.index].timer >= epd.events[epd.index].delay)
        {
            epd.events[epd.index].timer = 0;
            epd.events[epd.index].eventPlaying = false;
            epd.index++;

            if(epd.index > epd.events.Length)
            {
                epd.index = 0;

                if (epd.loopEvent)
                {
                    isActive = true;
                }
                else
                {
                    isActive = false;
                }
            }
        }*/
    }
}
