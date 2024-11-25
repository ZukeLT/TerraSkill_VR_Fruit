using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System;

public class FruitSlicer : MonoBehaviour
{
    public LayerMask sliceableLayer;
    public Transform planePosition;
    public List<Action> OnPointFruitSliced = new List<Action>();
    public List<Action> OnBombSliced = new List<Action>();
    private HashSet<GameObject> recentlySlicedObjects = new HashSet<GameObject>();
    

    private void OnTriggerEnter(Collider other)
    {
        if ((sliceableLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            if (recentlySlicedObjects.Contains(other.gameObject))
            {
                return; // Skip if already processed recently
            }

            recentlySlicedObjects.Add(other.gameObject);
            StartCoroutine(RemoveFromRecentlySliced(other.gameObject));

            SliceObject(other.gameObject);
        }
    }
    private IEnumerator RemoveFromRecentlySliced(GameObject obj)
    {
        yield return new WaitForSeconds(0.1f); // Adjust delay as needed
        recentlySlicedObjects.Remove(obj);
    }
    private void SliceObject(GameObject fruit)
    {
        if (planePosition != null)
        {
            SlicedHull slicedObject = fruit.Slice(planePosition.transform.position, planePosition.transform.up, null);

            if (slicedObject != null)
            {
                Material selectedMaterial = Resources.Load<Material>("Materials/CenterFruit_P");

                if (selectedMaterial != null)
                {
                    GameObject upperHull = slicedObject.CreateUpperHull(fruit, selectedMaterial);
                    GameObject lowerHull = slicedObject.CreateLowerHull(fruit, selectedMaterial);

                    upperHull.transform.position = fruit.transform.position;
                    lowerHull.transform.position = fruit.transform.position;

                    upperHull.transform.rotation = fruit.transform.rotation;
                    lowerHull.transform.rotation = fruit.transform.rotation;

                    upperHull.transform.localScale = fruit.transform.localScale;
                    lowerHull.transform.localScale = fruit.transform.localScale;

                    AddHullComponents(upperHull);
                    AddHullComponents(lowerHull);
                    Destroy(fruit);

                    Destroy(upperHull, 2f);
                    Destroy(lowerHull, 2f);

                    var meniuFruitScript = fruit.GetComponentInParent<MenuFruitScript>();
                    if(meniuFruitScript!= null)
                    {
                        //Vadinasi meniu fruitas
                    }
                    else
                    {
                        bool isBomb = false; //Prideti logika kaip atpazinti ar bomba ar nea
                        if (isBomb)
                        {
                            foreach (var action in OnBombSliced)
                                action();
                        }
                        else
                        {
                            foreach (var action in OnPointFruitSliced)
                                action();
                        }
                    }
                }
                else
                {
                    Debug.LogError("Material 'CenterFruit_P' not found!");
                }
            }
        }
    }


    private void AddHullComponents(GameObject hullObject)
    {
        MeshCollider collider = hullObject.AddComponent<MeshCollider>();
        collider.convex = true;

        Rigidbody rb = hullObject.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        hullObject.transform.SetParent(null);
    }



#if UNITY_EDITOR
    /**
	 * This is for Visual debugging purposes in the editor 
	 */
    public void OnDrawGizmos()
    {
        EzySlice.Plane cuttingPlane = new EzySlice.Plane();

        if(planePosition != null)
        {
            // the plane will be set to the same coordinates as the object that this
            // script is attached to
            // NOTE -> Debug Gizmo drawing only works if we pass the transform
            cuttingPlane.Compute(planePosition.transform);

            // draw gizmos for the plane
            // NOTE -> Debug Gizmo drawing is ONLY available in editor mode. Do NOT try
            // to run this in the final build or you'll get crashes (most likey)
            cuttingPlane.OnDebugDraw();
        }
      
    }

#endif

}
