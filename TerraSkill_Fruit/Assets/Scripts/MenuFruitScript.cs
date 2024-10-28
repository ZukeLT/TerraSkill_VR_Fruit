using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFruitScript : MonoBehaviour
{
    public GameObject fruitPrefab;

    public int rotationSpeed = 50; // Rotation speed in degrees per second
    public Vector3 rotationOffset = new Vector3(0, 0, 0); // Offset for initial rotation


    void Start()
    {
        if (fruitPrefab != null)
        {
            fruitPrefab.transform.parent = this.transform;
        }
    }

    void Update()
    {
        if (fruitPrefab != null)
        {
            fruitPrefab.transform.Rotate(rotationOffset.x, rotationOffset.y, rotationOffset.z+ rotationSpeed * Time.deltaTime);
        }
    }
}
