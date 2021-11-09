using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    public float modeSwitchingTime;
    public float modeSwitchMax;
    PlayerCharacter pc;
    Mode targetMode;

    // Start is called before the first frame update
    void Start()
    {
        pc = GetComponent<PlayerCharacter>();    
    }

    // Update is called once per frame
    void Update()
    {
        ModeSwitcherTimer();
        InputSwitch();
        
    }

    void InputSwitch()
    {
        if(Input.GetButtonDown("Mode Switch"))
        {
            if (targetMode == Mode.Melee)
                targetMode = Mode.LongRange;
            else
                targetMode = Mode.Melee;

            pc.changingMode = true;
        }
    }

    void ModeSwitcherTimer()
    {
        if(pc.changingMode)
        {
            if (Input.GetButtonDown("Mode Switch"))
                modeSwitchingTime = 0;
            else
                modeSwitchingTime += Time.deltaTime;

            if(modeSwitchingTime >= modeSwitchMax || pc.mode == targetMode)
            {
                modeSwitchingTime = 0;
                PlayerUI.Instance.ModeSwitch_UI();
                AudioManager.Instance.Play("mode_change");
                pc.mode = targetMode;
                pc.changingMode = false;
            }

        }
    }
}
