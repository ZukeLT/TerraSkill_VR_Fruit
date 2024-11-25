using UnityEngine;

public class HPFruitSpinner : MonoBehaviour
{
    public Vector3 rotationAxis = new Vector3(0, 1, 0); // Axis
    public float rotationSpeed = 10f;
    void Update()
    {
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime, Space.Self);
    }
}
