using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Apple applePrefab;

    public Text goalText;

    public PlayerNumberEnum playerNumber;

    public int BoardSize = 19;

    public int InitialLength = 3;

    public int Goal = 10;

    public bool GameIsOver = false;

    private List<Apple> apples = new List<Apple>();

    void Start()
    {
        CreateApple(PlayerNumberEnum.Player1);
        CreateApple(PlayerNumberEnum.Player2);
        goalText.text = Goal;
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
        CreateApple (randomPlayerNumber);
    }

    public void EndGame()
    {
        GameIsOver = true;
        Time.timeScale = 0;
    }

    public void RemoveApple(Apple apple)
    {
        apples.Remove (apple);
    }
}
