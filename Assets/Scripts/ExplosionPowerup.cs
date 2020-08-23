using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPowerup : MonoBehaviour
{    
    public string type;
    public GameObject explosionPrefab;

    public void Blast()
    {
        if (gameObject.name == "BigBomb5x5x5(Clone)"){}
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
