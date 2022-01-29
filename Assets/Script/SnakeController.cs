using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public Material PlayerColor;

    public Text scoreText;

    private bool IsInvincible = false;

    public PlayerNumberEnum playerNumber;

    public bool IsInverted = false;

    public float MoveSpeed = 5;

    public float SteerSpeed = 180;

    public float BodySpeed = 5;

    public int Gap = 10;

    private GameManager gameManager;

    public int Length;

    // References
    public GameObject SnakePrefab;

    public GameObject BodyPrefab;

    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();

    private List<Vector3> PositionsHistory = new List<Vector3>();

    public GameObject terrain;

    public GameObject rotAnim;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();

        Length = gameManager.InitialLength;
        for (int i = 0; i < gameManager.InitialLength; i++)
        {
            GrowSnake();
        }
    }

    public void AddNewSegment()
    {
        GrowSnake();
    }

    void FixedUpdate()
    {
        Length = BodyParts.Count;
        scoreText.Text = Length;
        if (Length >= gameManager.Goal)
        {
            gameManager.EndGame();
        }

        Vector3 direction = SnakePrefab.transform.forward;

        // Steer
        float steerDirection = Input.GetAxis(playerNumber.ToString()); // Returns value -1, 0, or 1

        if (IsInverted)
        {
            steerDirection *= -1;
        }

        // Move forward
        SnakePrefab.transform.position +=
            direction * MoveSpeed * Time.deltaTime;

        SnakePrefab
            .transform
            .Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);

        // Store position history
        PositionsHistory.Insert(0, SnakePrefab.transform.position);

        // Move body parts
        int index = 0;
        foreach (var body in BodyParts)
        {
            Vector3 point =
                PositionsHistory[Mathf
                    .Clamp(index * Gap, 0, PositionsHistory.Count - 1)];

            // Move body towards the point along the snakes path
            Vector3 moveDirection = point - body.transform.position;
            body.transform.position +=
                moveDirection * BodySpeed * Time.deltaTime;

            // Rotate body towards the point along the snakes path
            body.transform.LookAt (point);

            index++;
        }
    }

    private void GrowSnake()
    {
        GameObject body = Instantiate(BodyPrefab);
        body.transform.GetChild(0).GetComponent<MeshRenderer>().material =
            PlayerColor;
        body.transform.SetParent(this.gameObject.transform);
        BodyParts.Add (body);
    }

    public void ShrinkSnake()
    {
        StartCoroutine(BeInvincible());
        Transform lastChild =
            this.gameObject.transform.GetChild(transform.childCount - 1);
        lastChild.SetParent(null);
        BodyParts.Remove(lastChild.gameObject);
        Destroy(lastChild.gameObject, 2);
        Length--;
    }

    private void InvertInput()
    {
        IsInverted = true;
    }

    private void RestoreInput()
    {
        IsInverted = false;
    }

    public void CollisionDetection(string collisionTag, GameObject collision)
    {
        switch (collisionTag)
        {
            case "Apple":
                EatApple(collision.GetComponent<Apple>());
                break;
            case "Player":
            case "Body":
                EatPlayer(collision
                    .transform
                    .root
                    .GetComponent<SnakeController>());
                break;
            case "Wall":
                ChangeDirection();
                break;
        }
    }

    private void EatPlayer(SnakeController player)
    {
        if (
            player &&
            player.playerNumber != playerNumber &&
            !player.IsInvincible
        )
        {
            player.ShrinkSnake();
        }
    }

    private void EatApple(Apple apple)
    {
        if (apple.playerNumber == playerNumber)
        {
            GrowSnake();
            gameManager.CreateRandomApple();
        }
        else
        {
            gameManager.CreateApple (playerNumber);
        }
        gameManager.RemoveApple (apple);
        apple.DestroyApple();
    }

    public void TerrainHitted(GameObject curTer)
    {
        StartCoroutine(ChangeInputAnimation());
        if (curTer != terrain)
        {
            InvertInput();
        }
        else
        {
            RestoreInput();
        }
    }

    public void ChangeDirection()
    {
        SnakePrefab.transform.Rotate(Vector3.up * 180f);
    }

    public IEnumerator BeInvincible()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(2);
        IsInvincible = false;
    }

    IEnumerator ChangeInputAnimation()
    {
        rotAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        rotAnim.SetActive(false);
    }
}
