using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Apple applePrefab;

    public PlayerNumberEnum playerNumber;

    public int BoardSize = 19;

    public int InitialLength = 3;

    private List<Apple> apples = new List<Apple>();

    void Start()
    {
        CreateApple(PlayerNumberEnum.Player1);
        CreateApple(PlayerNumberEnum.Player2);
    }

    public void CreateApple(PlayerNumberEnum playerNumber)
    {
        Apple curApple = Instantiate(applePrefab);
        Vector3 rndPos = new Vector3(0, 0.5f, 0);
        rndPos.x = Random.Range(-BoardSize, BoardSize);
        rndPos.z = Random.Range(-BoardSize, BoardSize);
        curApple.gameObject.transform.position = rndPos;
        curApple.playerNumber = playerNumber;
        curApple.transform.SetParent (transform);
        apples.Add (curApple);
    }

    public void CreateRandomApple()
    {
        PlayerNumberEnum randomPlayerNumber =
            (PlayerNumberEnum) Random.Range(0, 2);
        Debug.Log("Creating random apple for " + randomPlayerNumber);
        CreateApple (randomPlayerNumber);
    }
}
