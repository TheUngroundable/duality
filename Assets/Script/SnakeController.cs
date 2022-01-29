using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public List<GameObject> terrains = new List<GameObject>();

    public GameObject rotAnim;

    public float volume = 1f;

    private AudioSource audioSource;

    public AudioClip growSound;

    public AudioClip neutralize;

    public AudioClip hitWall;

    public AudioClip hurt;

    public AudioClip invert;

    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
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
        scoreText.text = Length.ToString();
        HandleEndGameCondition();
        if (gameManager.GameHasStarted)
        {
            Move();
        }
    }

    private void HandleEndGameCondition()
    {
        if (Length >= gameManager.Goal)
        {
            gameManager.EndGame();
        }
    }

    private void Move()
    {
        Vector3 direction = SnakePrefab.transform.forward;
        float steerDirection = GetSteerDirection();
        MoveForward (direction, steerDirection);
        PositionsHistory.Insert(0, SnakePrefab.transform.position);
        MakeBodyPartsFollowHead();
    }

    private float GetSteerDirection()
    {
        float steerDirection = Input.GetAxis(playerNumber.ToString()); // Returns value -1, 0, or 1

        if (IsInverted)
        {
            steerDirection *= -1;
        }
        return steerDirection;
    }

    private void MoveForward(Vector3 direction, float steerDirection)
    {
        SnakePrefab.transform.position +=
            direction * MoveSpeed * Time.deltaTime;

        SnakePrefab
            .transform
            .Rotate(Vector3.up * steerDirection * SteerSpeed * Time.deltaTime);
    }

    private void MakeBodyPartsFollowHead()
    {
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
        PlaySound (growSound);
        GameObject body =
            Instantiate(BodyPrefab,
            gameObject.transform.position,
            gameObject.transform.rotation);
        body.transform.GetChild(0).GetComponent<MeshRenderer>().material =
            PlayerColor;
        body.transform.SetParent(this.gameObject.transform);
        BodyParts.Add (body);
    }

    private void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot (audioClip, volume);
    }

    public void CutSnake(GameObject segment)
    {
        PlaySound (hurt);
        int segmentIndex = BodyParts.IndexOf(segment);
        Debug.Log (segmentIndex);
        for (int i = segmentIndex; i < BodyParts.Count; i++)
        {
            GameObject bodyPart = BodyParts[i];
            bodyPart.transform.SetParent(null);
            BodyParts.Remove (bodyPart);
            Destroy(bodyPart, 2);
        }

        StartCoroutine(BeInvincible());
    }

    private void InvertInput()
    {
        PlaySound (invert);
        if (!IsInverted) StartCoroutine(ChangeInputAnimation());
        IsInverted = true;
    }

    private void RestoreInput()
    {
        PlaySound (invert);
        if (IsInverted) StartCoroutine(ChangeInputAnimation());
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
                    .GetComponent<SnakeController>(),
                collision.transform.parent.gameObject);
                break;
            case "Wall":
                PlaySound (hitWall);
                ChangeDirection();
                break;
        }
    }

    private void EatPlayer(SnakeController player, GameObject segment)
    {
        if (
            player &&
            player.playerNumber != playerNumber &&
            !player.IsInvincible
        )
        {
            player.CutSnake (segment);
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
            PlaySound (neutralize);
            gameManager.CreateApple (playerNumber);
        }
        gameManager.RemoveApple (apple);
        apple.DestroyApple();
    }

    public void OnTerrainEnter(GameObject terrain)
    {
        if (terrains.IndexOf(terrain) != -1 && gameManager.GameHasStarted)
        {
            Debug.Log("Enter Area");
            RestoreInput();
        }
    }

    public void OnTerrainExit(GameObject terrain)
    {
        if (terrains.IndexOf(terrain) == -1 && gameManager.GameHasStarted)
        {
            Debug.Log("Exit Area");

            InvertInput();
        }
    }

    public void ChangeDirection()
    {
        SnakePrefab.transform.Rotate(Vector3.up * 180f);
    }

    public IEnumerator BeInvincible()
    {
        IsInvincible = true;
        yield return new WaitForSeconds(gameManager.SecondsInvincible);
        IsInvincible = false;
    }

    IEnumerator ChangeInputAnimation()
    {
        rotAnim.SetActive(true);
        yield return new WaitForSeconds(1);
        rotAnim.SetActive(false);
    }
}
