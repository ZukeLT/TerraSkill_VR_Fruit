using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PointScript : MonoBehaviour
{
    public TextMeshPro pointText;
    public int points;

    void Start()
    {
        points = 0;
    }

    void Update()
    {
        if(pointText != null)
        {
            pointText.text = points.ToString() + " P";
        }
    }
}
