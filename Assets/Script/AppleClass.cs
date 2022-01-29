using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleClass : MonoBehaviour
{
    public PlayerNumberEnum playerNumber;

    public int score = 1;

    public bool white;

    public Material matW;

    public Material matB;

    private Vector3 startPosition;

    public void Start()
    {
        if (white)
            transform.GetComponent<MeshRenderer>().material = matW;
        else
            transform.GetComponent<MeshRenderer>().material = matB;
        startPosition = transform.position; //per l'animazione di movimento
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
        //chiama la creazione di un altra mela opposta di colore
        GameObject.FindObjectOfType<GameManager>().CreateApple(!white);
    }
}
