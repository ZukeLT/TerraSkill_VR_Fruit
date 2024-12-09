using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using System;

public class FruitSlicer : MonoBehaviour
{
    public LayerMask sliceableLayer;
    public Transform planePosition;
    public ParticleSystem slicedFruitParticle;
    public List<Action> OnPointFruitSliced = new List<Action>();
    public List<Action> OnBombSliced = new List<Action>();
    public MainController mainController;
    private HashSet<GameObject> recentlySlicedObjects = new HashSet<GameObject>();

    private void Start()
    {
        mainController = FindObjectOfType<MainController>();
    }

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

                    Rigidbody upperRb = upperHull.AddComponent<Rigidbody>();
                    Rigidbody lowerRb = lowerHull.AddComponent<Rigidbody>();

                    Vector3 forceDirectionUpper = (upperHull.transform.position - fruit.transform.position).normalized + Vector3.up;
                    Vector3 forceDirectionLower = (lowerHull.transform.position - fruit.transform.position).normalized - Vector3.up;

                    if(upperRb != null)
                        upperRb.AddForce(forceDirectionUpper * 10f, ForceMode.Impulse);
                    if (lowerRb != null)
                        lowerRb.AddForce(forceDirectionLower * 10f, ForceMode.Impulse);

                    PlaySlicedFruitParticles(fruit);
                    

                    Destroy(upperHull, 2f);
                    Destroy(lowerHull, 2f);

                    var meniuFruitScript = fruit.GetComponentInParent<MenuFruitScript>();
                    if(meniuFruitScript!= null)
                    {
                        //Vadinasi meniu fruitas - nenaikinam main body
                    }
                    else
                    {
                        Destroy(fruit);
                        
                        bool isBomb = fruit.name.ToLower().Contains("bomb"); //Prideti logika kaip atpazinti ar bomba ar nea
                        if (isBomb)
                        {
                            mainController.playBombSound();
                            foreach (var action in OnBombSliced)
                                action();
                        }
                        else
                        {
                            mainController.playSliceSound();
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

    private void PlaySlicedFruitParticles(GameObject fruit)
    {
        if (slicedFruitParticle != null && fruit != null)
        {
            ParticleSystem particleInstance = Instantiate(slicedFruitParticle, fruit.transform.position, fruit.transform.rotation);

            particleInstance.transform.SetParent(fruit.transform.parent, true);
            particleInstance.transform.localPosition = fruit.transform.localPosition;
            particleInstance.Play();

            Destroy(particleInstance.gameObject, particleInstance.main.duration + particleInstance.main.startLifetime.constantMax);
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
