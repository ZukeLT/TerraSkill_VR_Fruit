using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFruitScript : MonoBehaviour
{
    public int rotationSpeed = 50;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject fruitPrefab;
    public GameObject timerComponent;
    public GameObject spawnersController;
    private Quaternion targetRotation;

    void Start()
    {
        if (fruitPrefab != null)
        {
            fruitPrefab.transform.parent = this.transform;
        }

        if (fruitPrefab != null)
        {
            targetRotation = Quaternion.Euler(rotationOffset);
            fruitPrefab.transform.rotation = targetRotation;
        }
    }

    void Update()
    {
        if (fruitPrefab != null)
        {
            targetRotation *= Quaternion.Euler(0, 0, rotationSpeed * Time.deltaTime);
            fruitPrefab.transform.rotation = Quaternion.Lerp(fruitPrefab.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.ToLower().Contains("sword"))
        {
            Debug.LogWarning("Pradeti zaidima");
            Destroy(this.gameObject);
            
            if (timerComponent != null)
            {
                var comp = timerComponent.GetComponent<TimerScript>();
                if(comp != null)
                {
                    comp.SetNewTime(180f);
                    comp.startTimer = true;
                }
            }

            var spanwerController = spawnersController.GetComponent<SpawningController>();
            if (spanwerController != null)
            {
                spanwerController.StartSpawningFruits();
            }
        }
    }
}
