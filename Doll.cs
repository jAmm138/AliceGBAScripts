using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doll : MonoBehaviour
{
    public Mode type;
    public Transform projecitleParent;
    public bool active;
    public AiMovement aim;
    public AiAttack aia;
    public int index = -1;

    // Start is called before the first frame update
    void Start()
    {
        if (!DollManager.Instance.dolls.Contains(this))
            DollManager.Instance.dolls.Add(this);

        aim = GetComponent<AiMovement>();
        aia = GetComponent<AiAttack>();

        this.gameObject.SetActive(false);
    }

    public void Init(bool melee)
    {
        this.transform.position = DollManager.Instance.GetRandomSpawnPosition().position;
        this.gameObject.SetActive(true);
        active = true;

        if (DollManager.Instance.activeDolls.Count == 0)
            index = 0;
        else if (DollManager.Instance.activeDolls.Count == 1)
            index = 1;
        else if (DollManager.Instance.activeDolls.Count == 2)
            index = 2;

        PlayerUI.Instance.AssignDoll_UI(index, melee);
        DollManager.Instance.activeDolls.Add(this);
    }

    public void Return()
    {
        aim.Return();
        aia.Return();
        active = false;
        this.gameObject.SetActive(false);
        DollManager.Instance.activeDolls.Remove(this);
        PlayerUI.Instance.RemoveDoll_UI(index);
        PlayerCharacter.Instance.pa.dollIndex--;

        index = -1;
        if (PlayerCharacter.Instance.pa.dollIndex < 0)
            PlayerCharacter.Instance.pa.dollIndex = 0;

        AudioManager.Instance.Play("doll_remove");
    }

    public void AttackMode()
    {
        aim.attackMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
