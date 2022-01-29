using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundRandomColor : MonoBehaviour
{
    public Material mat;
     public Material matB;
    public Color[] colors;

    public void Start()
    {
        Color curCol = colors[Random.Range(0,colors.Length)];
        mat.color = curCol ;
        matB.color =  curCol;
    }
}
