using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HPController : MonoBehaviour
{
    public int HP = 3;
    public GameObject hpContainer;
    public GameObject hpFruitsPrefab;
    public List<Action> OnHPEnded = new List<Action>();
    private List<GameObject> HpFruits = new List<GameObject>();
    void Start()
    {
        HP = 3;
        if(hpContainer != null)
        {
            List<GameObject> fruitHP = new List<GameObject>();
            hpContainer.GetChildGameObjects(fruitHP);
            HpFruits.Clear();
            foreach (GameObject hp in fruitHP)
            {
                if(hp.name.EndsWith("_HP"))
                    HpFruits.Add(hp);

                HP = HpFruits.Count;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage()
    {
        if (HP > 0)
        {
            for (int i = HpFruits.Count - 1; i >= 0; i--)
            {
                GameObject obj = HpFruits[i];
                if (obj.name.ToLower().StartsWith(HP.ToString() + "_"))
                {
                    HpFruits.RemoveAt(i);
                    Destroy(obj);
                    // Apply effects if needed
                    HP--;

                    if (HP == 0)
                    {
                        foreach (Action action in OnHPEnded)
                            action();
                    }
                    break;
                }
            }
        }
    }
    public void ResetHP()
    {
        GameObject newHpFruit = Instantiate(hpFruitsPrefab, hpContainer.transform.parent);
        Destroy(hpContainer);
        hpContainer = newHpFruit;
        Start();
    }
}
