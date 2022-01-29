using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{

    public enum PlayerNumberEnum {Player1, Player2};

    public PlayerNumberEnum PlayerNumber;

    public bool IsInverted = false;
    public float MoveSpeed = 5;
    public float SteerSpeed = 180;
    public float BodySpeed = 5;
    public int Gap = 10;
    public int InitialLength = 3;
    public int Length;
    // References
    public GameObject SnakePrefab;
    public GameObject BodyPrefab;
    // Lists
    private List<GameObject> BodyParts = new List<GameObject>();
    private List<Vector3> PositionsHistory = new List<Vector3>();

    void Start()
    {
        Length = InitialLength;
        for (int i = 0; i < InitialLength; i++)
        {
            GrowSnake();
        }
    }

    public void AddNewSegment()
    {
        Length++;
        GrowSnake();
    }

    void Update()
    {
        // Move forward
        SnakePrefab.transform.position +=
            SnakePrefab.transform.forward * MoveSpeed * Time.deltaTime;

        // Steer
        float steerDirection = Input.GetAxis(PlayerNumber.ToString()); // Returns value -1, 0, or 1
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
        body.transform.SetParent(this.gameObject.transform);
        BodyParts.Add (body);
    }

    public void ShrinkSnake(){
        Transform lastChild = this.gameObject.transform.GetChild(transform.childCount - 1);
        lastChild.SetParent(null);
        Length--;
    }

    private void InvertInput(){
        IsInverted = !IsInverted;
    }
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.transform.parent.gameObject.tag == "Player"){
            collision.gameObject.GetComponent<SnakeController>().ShrinkSnake();
        }
    }
}
