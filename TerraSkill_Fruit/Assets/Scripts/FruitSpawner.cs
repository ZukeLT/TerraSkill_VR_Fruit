using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FruitSpawner : MonoBehaviour
{
    public bool spawnFruits;
    public List<GameObject> fruitPrefabs;
    public float spawnInterval = 1f;
    public float shootForce = 10f;
    public Vector3 spawnOffset = new Vector3(0, 0, 0);
    public float randomness = 2f;
    public int fruitCount = 2;
    public float fruitLifetime = 5f;

    private float timer;

    void Start()
    {
        spawnFruits = false;
        timer = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            if(spawnFruits)
                SpawnFruit();
            timer = 0f;
        }
    }

    void SpawnFruit()
    {
        if (fruitPrefabs != null && fruitPrefabs.Count > 0)
        {
            GameObject obj = fruitPrefabs[Random.Range(0, fruitPrefabs.Count)];
            if (obj != null)
            {
                Vector3 spawnPosition = transform.position + spawnOffset;
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

                Destroy(fruit, fruitLifetime);
            }
        }
    }
}
