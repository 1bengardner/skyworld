using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour {

    public int damage;
    const string instantDamageLayerName = "Lava";

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            bool bypassRecovery = gameObject.layer.Equals(LayerMask.NameToLayer(instantDamageLayerName));
            StartCoroutine(other.GetComponent<PlayerHealth>().TakeDamage(damage, bypassRecovery));
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            StartCoroutine(other.GetComponent<PlayerHealth>().TakeDamage(damage));
        }
    }
}
