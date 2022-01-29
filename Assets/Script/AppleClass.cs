using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleClass : MonoBehaviour
{
    public int score=1;
    public bool white=true;

    public void Update()
    {
        transform.Rotate(0,0,50*Time.deltaTime); 
    }

    public void DestroyApple()
    {
        GameObject.FindObjectOfType<GameManager>().CreateApple(!white);
    }
}
