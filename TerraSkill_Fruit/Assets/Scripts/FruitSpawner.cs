using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public List<GameObject> fruitPrefabs; 
    public float spawnInterval = 1f;
    public float shootForce = 10f;
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
    public float randomness = 2f;
    public int fruitCount = 2;

    private float timer;

    void Start()
    {
        timer = 0f; 
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnFruit();
            timer = 0f;
        }
    }

    void SpawnFruit()
    {
        if(fruitPrefabs != null && fruitPrefabs.Count > 0)
        {
            GameObject obj = fruitPrefabs[Random.RandomRange(0, fruitPrefabs.Count - 1)];
            if (obj != null)
            {

                Vector3 spawnPosition = transform.position;

                GameObject fruit = Instantiate(obj, spawnPosition, Quaternion.identity);

                Rigidbody fruitRb = fruit.GetComponent<Rigidbody>();
                if (fruitRb == null)
                {
                    fruitRb = fruit.AddComponent<Rigidbody>();
                }

                Vector3 shootDirection = Vector3.up + new Vector3(
                    Random.Range(-randomness, randomness) * 0.5f,  
                    0,                                             
                    Random.Range(-randomness, randomness) * 0.5f 
                );

                fruitRb.AddForce(shootDirection * shootForce, ForceMode.Impulse);
            }
        }
       
    }
}
