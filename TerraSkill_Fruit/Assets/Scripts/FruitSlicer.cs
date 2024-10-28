using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;

public class FruitSlicer : MonoBehaviour
{
    public LayerMask sliceableLayer;
    public Transform planePosition;


    private void OnTriggerEnter(Collider other)
    {
        if ((sliceableLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            SliceObject(other.gameObject);
        }
    }

    private void SliceObject(GameObject fruit)
    {
        if (planePosition != null)
        {
            SlicedHull slicedObject = fruit.Slice(planePosition.transform.position, planePosition.transform.up, null);

            if (slicedObject != null)
            {
                GameObject upperHull = slicedObject.CreateUpperHull(fruit, fruit.GetComponent<Renderer>().material);
                GameObject lowerHull = slicedObject.CreateLowerHull(fruit, fruit.GetComponent<Renderer>().material);
                AddHullComponents(upperHull);
                AddHullComponents(lowerHull);
                Destroy(fruit);
            }
        }
    }


    private void AddHullComponents(GameObject hullObject)
    {
        // Add MeshCollider and set to convex for proper collision
        MeshCollider collider = hullObject.AddComponent<MeshCollider>();
        collider.convex = true;

        // Add Rigidbody to make sliced pieces fall
        Rigidbody rb = hullObject.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        // Set parent to null if needed to separate the pieces visually
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
