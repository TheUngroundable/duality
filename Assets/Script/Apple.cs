using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : MonoBehaviour
{
    public PlayerNumberEnum playerNumber;

    private GameManager gameManager;

    public Material matW;

    public Material matB;

    private Vector3 startPosition;

    public void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

        if (playerNumber == PlayerNumberEnum.Player1)
        {
            transform.GetComponent<MeshRenderer>().material = matW;
        }
        else
        {
            transform.GetComponent<MeshRenderer>().material = matB;
        }
        startPosition = transform.position;
    }

    public void Update()
    {
        transform.Rotate(0, 50 * Time.deltaTime, 0);
        transform.position =
            new Vector3(startPosition.x,
                startPosition.y + Mathf.Sin(Time.time * 5) / 6,
                startPosition.z);
    }

    public void DestroyApple()
    {
        this.gameObject.GetComponent<Collider>().enabled = false;
        Destroy(this.gameObject);
    }
}
