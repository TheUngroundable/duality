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
        CreateApple(true);
        CreateApple(false);
        CreateApple(true);
        CreateApple(false);
        CreateApple(true);
        CreateApple(false);
    }

    public void CreateApple(bool white)
    {
        AppleClass curApple = Instantiate(applePrefab);
        Vector3 rndPos = new Vector3(0, 0.5f, 0);
        rndPos.x = Random.Range(-9, 9);
        rndPos.z = Random.Range(-9, 9);
        curApple.gameObject.transform.position = rndPos;
        if (white)
        {
            curApple.playerNumber = PlayerNumberEnum.Player1;
        }
        else
        {
            curApple.playerNumber = PlayerNumberEnum.Player2;
        }
        curApple.transform.SetParent (transform);
        apples.Add (curApple);
    }
}
