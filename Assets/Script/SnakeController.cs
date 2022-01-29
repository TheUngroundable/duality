﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    public Material PlayerColor;

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

    public GameObject terrain;

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

    void FixedUpdate()
    {
        Vector3 direction = SnakePrefab.transform.forward;

        // Steer
        float steerDirection = Input.GetAxis(PlayerNumber.ToString()); // Returns value -1, 0, or 1

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

        if(Input.GetMouseButtonDown(0))
        {
            Debug.Log("ccicio");
            SnakePrefab.transform.Rotate(Vector3.up * 180f);
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
        Transform lastChild =
            this.gameObject.transform.GetChild(transform.childCount - 1);
        lastChild.SetParent(null);
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
        }
    }

    public void EatApple(Apple apple)
    {
        GrowSnake();
        apple.DestroyApple();
    }

    public void TerrainHitted(GameObject curTer)
    {
        if (curTer != terrain)
        {
            InvertInput();
        }
        else
        {
            RestoreInput();
        }
    }
}
