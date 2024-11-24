using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningController : MonoBehaviour
{
    public GameObject timerComponent;

    private List<GameObject> spawners = new List<GameObject>();
   
    
    // Start is called before the first frame update
    void Start()
    {
        foreach (FruitSpawner spawner in GetComponentsInChildren<FruitSpawner>())
        {
            spawners.Add(spawner.gameObject);
        }
            

        if (timerComponent != null)
        {
            var comp = timerComponent.GetComponent<TimerScript>();
            if (comp != null)
            {
                comp.OnTimerEnded.Add(StopSpawningFruits);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartSpawningFruits()
    {

        if (spawners != null && spawners.Count > 0)
        {
            foreach (var spawner in spawners)
            {
                var comp = spawner.GetComponent<FruitSpawner>();
                if (comp != null)
                    comp.spawnFruits = true;
            }
        }
    }
    public void StopSpawningFruits()
    {

        if (spawners != null && spawners.Count > 0)
        {
            foreach (var spawner in spawners)
            {
                var comp = spawner.GetComponent<FruitSpawner>();
                if (comp != null)
                    comp.spawnFruits = false;
            }
        }
    }
}
