using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{ 
    
    public AppleClass applePrefab;
    private List<AppleClass> apples = new List<AppleClass>();

    void Start()
    {
        CreateApple(true);
        CreateApple(false);
    }

    public void CreateApple(bool white)
    {
        AppleClass curApple = Instantiate(applePrefab);
        Vector3 rndPos = new Vector3(0,4.5f,0);
        rndPos.x = Random.Range(-5,5);
        rndPos.z = Random.Range(-5,5);
        curApple.gameObject.transform.position = rndPos;
        if(white) curApple.white = white;
        curApple.transform.SetParent(transform);
        apples.Add(curApple);
    }
}
