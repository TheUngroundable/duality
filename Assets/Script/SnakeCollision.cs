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
}
