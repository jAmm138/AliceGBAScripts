using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public float health = 5f;
    private bool giveHealth;

    public void AddHealth()
    {
        if (!giveHealth)
        {
            PlayerCharacter.Instance.AddHealth(health);
            AudioManager.Instance.Play("health_pickup");
            Destroy(this.gameObject);
            giveHealth = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerCharacter>())
        {
            AddHealth();
        }
    }
}
