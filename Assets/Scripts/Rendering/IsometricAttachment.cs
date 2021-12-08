using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles rendering behavior for objects intended to be combined with other objects.
/// </summary>
[RequireComponent (typeof(Renderer))]
public class IsometricAttachment : MonoBehaviour
{
    [Tooltip("Uses this target object to compute sorting order. Uses object's hiearchy parent by default.")]
    public Transform parent;
    [Tooltip("Determines whether this object will always be behind or in front of parent object.")]
    public ObjectDirection position;
    [Tooltip("The change between parent object sorting order and this object's sorting order.")]
    public int sortOrderDiff = 1;

    // Other reference variables.
    Renderer ren;

    // Start is called before the first frame update
    void Start()
    {
        if (parent == null)
        {
            if (transform.parent != null)
            {
                parent = transform.parent;
            }
            else
            {
                Debug.LogWarning("No IsometricAttachment parent assigned to " + name + ".");
            }
        }
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (parent == null)
        {
            return;
        }

        if (position == ObjectDirection.Behind)
        {
            ren.sortingOrder = parent.GetComponent<Renderer>().sortingOrder - sortOrderDiff;
        }
        else if (position == ObjectDirection.Front)
        {
            ren.sortingOrder = parent.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
        }
    }
}
