using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCollision : MonoBehaviour
{
    SnakeController snakeController;

    public void Start()
    {
        snakeController = transform.parent.GetComponent<SnakeController>();
    }

    void OnCollisionEnter(Collision collision)
    {
        snakeController
            .CollisionDetection(collision.gameObject.tag, collision.gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        snakeController.OnTerrainEnter(collider.gameObject);
    }

    private void OnTriggerExit(Collider collider)
    {
        snakeController.OnTerrainExit(collider.gameObject);
    }

    private void OnTriggerStay(Collider collider)
    {
        snakeController.OnTerrainStay(collider.gameObject);
    }
}
