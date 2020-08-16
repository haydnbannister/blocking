using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPowerup : MonoBehaviour
{    
    public string type;
    public GameObject explosionPrefab;

    public void Blast()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
