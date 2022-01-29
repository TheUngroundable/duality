using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Apple applePrefab;

    public int BoardSize = 19;

    public int InitialLength = 3;

    private List<Apple> apples = new List<Apple>();

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
        Apple curApple = Instantiate(applePrefab);
        Vector3 rndPos = new Vector3(0, 0.5f, 0);
        rndPos.x = Random.Range(-BoardSize, BoardSize);
        rndPos.z = Random.Range(-BoardSize, BoardSize);
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
