using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpCollectable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter()
    {
        AudioManager.Instance.Play("collectible_fanfare");
        CollectableController.Instance.IncreaseItems();
        Destroy(this.gameObject);
    }
}
