using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTrigger : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Snake")
        {
            other
                .gameObject
                .transform
                .parent
                .GetComponent<SnakeController>()
                .TerrainHitted(gameObject);
        }
    }
}
