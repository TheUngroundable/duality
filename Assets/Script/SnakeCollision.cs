
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
        snakeController.CollisionDetection(collision.gameObject.tag,collision.gameObject);
        if(collision.gameObject.tag=="Apple")
        {
           collision.gameObject.GetComponent<Collider>().enabled=false;
           Destroy(collision.gameObject);
        }
       
    }
}
