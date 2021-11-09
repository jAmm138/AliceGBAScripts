using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Refs")]
    public Image[] dollSelection;
    public BlinkingImage[] dollSelectionBlink;
    public Image[] dollStatus;
    public Image mode_h;
    public Image mode_s;
    public Image mode_select;
    public Slider playerHealth;

    [Header("Sprites")]
    public Sprite h_sprite;
    public Sprite s_sprite;

    public static PlayerUI Instance;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerHealth.maxValue = PlayerCharacter.Instance.healthMax;
        playerHealth.value = playerHealth.maxValue;

        foreach (Image img in dollSelection)
        {
            img.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        foreach(Image img in dollStatus)
        {
            if(img.enabled)
            {

            }
        }
    }

    public void AssignDoll_UI(int index, bool s)
    {
        dollSelection[index].gameObject.SetActive(true);
        if (s)
            dollSelection[index].sprite = s_sprite;
        else
            dollSelection[index].sprite = h_sprite;

        if (index == PlayerCharacter.Instance.pa.dollIndex)
            dollSelectionBlink[index].enabled = true;
        else
            dollSelectionBlink[index].enabled = false;
    }

    public void RemoveDoll_UI(int index)
    {
        dollSelection[index].sprite = null;
        dollSelection[index].gameObject.SetActive(false);
        if (dollSelectionBlink[index].enabled)
            dollSelectionBlink[index].enabled = false;

        dollSelectionBlink[PlayerCharacter.Instance.pa.dollIndex].enabled = true;
    }

    public void ModeSwitch_UI()
    {
        switch(PlayerCharacter.Instance.mode)
        {
            case Mode.Melee:
                mode_select.rectTransform.position = mode_h.rectTransform.position;
                break;
            case Mode.LongRange:
                mode_select.rectTransform.position = mode_s.rectTransform.position;
                break;
        }
    }

    public void UpdateHealth_UI()
    {
        playerHealth.value = PlayerCharacter.Instance.health;
    }
}
