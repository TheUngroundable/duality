using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplesManager : MonoBehaviour
{
    public GameObject applePrefab;
    private List<GameObject> apples = new List<GameObject>();

    void Start()
    {
        
    }

    public void PlaceApple()
    {
        GameObject curApple = Instantiate(applePrefab);
        Vector3 rndPos = new Vector3(0,0,0);
        rndPos.x = Random.Range(0,2);
        rndPos.z = Random.Range(0,2);
        curApple.transform.position = rndPos;
        apples.Add(curApple);
    }
}
