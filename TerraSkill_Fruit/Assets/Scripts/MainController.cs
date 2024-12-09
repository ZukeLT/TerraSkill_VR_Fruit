using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;

public class MainController : MonoBehaviour
{
    public GameObject pointComponent;
    public GameObject timerComponent;
    public GameObject healthComponent;
    public GameObject spawnersComponent;
    public GameObject playFruit;
    public GameObject rightHand_Controller;

    private TimerScript timerScript;
    private PointScript pointScript;
    private SpawningController spawnerController;
    private MenuFruitScript menuFruit;
    private FruitSlicer fruitSlicer;
    private HPController hpController;
    public AudioClip[] sliceSounds;
    public AudioSource sound;
    public AudioSource BGMusic;

    // Start is called before the first frame update
    void Start()
    {
        BGMusic.Play();
        if (timerComponent != null)
            timerScript = timerComponent.GetComponent<TimerScript>();

        if(pointComponent != null)
            pointScript = pointComponent.GetComponent<PointScript>();

        if (spawnersComponent != null)
            spawnerController = spawnersComponent.GetComponent<SpawningController>();

        if(playFruit != null)
            menuFruit = playFruit.GetComponent<MenuFruitScript>();

        if(healthComponent != null)
            hpController = healthComponent.GetComponent<HPController>();

        if(rightHand_Controller != null)
        {
            StartCoroutine(FindFruitSliceComponent());
        }

        if(menuFruit != null)
        {
            menuFruit.OnStart.Add(StartTimer);
            menuFruit.OnStart.Add(StartSpawningFruits);
            menuFruit.OnStart.Add(ResetPoints);
            menuFruit.OnStart.Add(ResetHP);
        }

        if (timerScript != null)
        {
            timerScript.OnTimerEnded.Add(StopSpawningFruits);
            timerScript.OnTimerEnded.Add(RespawnFruit);
        }

        if(hpController != null)
        {
            hpController.OnHPEnded.Add(StopSpawningFruits);
            hpController.OnHPEnded.Add(StopTimer);
            hpController.OnHPEnded.Add(RespawnFruit);
        }
    }

    void Update()
    {
        
    }
    private IEnumerator FindFruitSliceComponent()
    {
        while (fruitSlicer == null)
        {
            var handObject = rightHand_Controller.transform
                .Find("[RightHand Controller] Model Parent/VR_Sword_R(Clone)/Sword_Scabbard");

            if (handObject != null)
            {
                fruitSlicer = handObject.GetComponent<FruitSlicer>();
                if (fruitSlicer != null)
                {
                    fruitSlicer.OnPointFruitSliced.Add(AddPoints);
                    fruitSlicer.OnBombSliced.Add(OnBombSliced);

                    Debug.Log("FruitSlicer component found and referenced!");
                    yield break;
                }
            }

            yield return null;
        }
    }

    public void playSliceSound()
    {
        System.Random rand = new System.Random();
        sound.clip = sliceSounds[rand.Next(0, sliceSounds.Length-1)];
        sound.Play();
    }

    public void playBombSound()
    {
        sound.clip = sliceSounds[3];
        sound.Play();
    }

    public void StartTimer()
    {

        if (timerScript != null)
        {
            timerScript.SetNewTime(60f);
            timerScript.startTimer = true;
        }
        else
        {
            Debug.LogError("Nerasta laiko komponente!");
        }
    }
    public void StopTimer()
    {

        if (timerScript != null)
        {
            timerScript.startTimer = false;
        }
        else
        {
            Debug.LogError("Nerasta laiko komponente!");
        }
    }
    public void StartSpawningFruits()
    {
        if(spawnerController != null)
        {
            spawnerController.StartSpawningFruits();
        }
        else
        {
            Debug.LogError("Nerasta spawning controller komponente!");
        }
    }
    public void StopSpawningFruits()
    {
        if (spawnerController != null)
        {
            spawnerController.StopSpawningFruits();
        }
        else
        {
            Debug.LogError("Nerasta spawning controller komponente!");
        }
    }
    public void AddPoints()
    {
        if(pointScript != null)
        {
            pointScript.points += 1;
        }
    }
    public void ResetPoints()
    {
        if (pointScript != null)
        {
            pointScript.points = 0;
        }
    }
    public void OnBombSliced()
    {
        if (hpController != null)
        {
            hpController.TakeDamage();
        }
        Debug.Log("Bombaaaa!");
    }
    public void RespawnFruit()
    {
        if (playFruit != null)
        {
            playFruit.SetActive(true);
        }
    }
    public void ResetHP()
    {
        if (hpController != null)
        {
            hpController.ResetHP();
        }
    }
}
