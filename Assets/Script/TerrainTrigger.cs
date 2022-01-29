using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTrigger : MonoBehaviour
{
    public bool white;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Snake Head")
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
