using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Live-resorts objects based on player position. Apply to objects whose sorting order should be changed
/// based on the player's position.
/// </summary>
[RequireComponent (typeof(PolygonCollider2D))]
public class IsometricObjectSorter : MonoBehaviour
{
    [Header("Player Positioning")]
    [Tooltip("If player is left of object, they are behind or in front of object.")]
    public ObjectDirection left;
    [Tooltip("If player is right of object, they are behind or in front of object.")]
    public ObjectDirection right;
    [Tooltip("The change between player sorting order and this object's sorting order.")]
    public int sortOrderDiff = 1;

    [Header("References")]
    [Tooltip("The player object. Will be set automatically during runtime.")]
    public GameObject player;
    [Tooltip("Any objects that should always render behind this one.")]
    public List<GameObject> alwaysBehind;

    PolygonCollider2D col;
    Renderer ren;

    // Directional booleans.
    bool playerIsBehind = false;
    bool playerIsFront = false;
    bool resort;
    float playerFeetPointY;
    float playerFeetPointX;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        col = GetComponent<PolygonCollider2D>();
        ren = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        playerFeetPointY = player.GetComponent<PlayerController>().feet.GetComponent<Collider2D>().bounds.center.y;
        playerFeetPointX = player.GetComponent<PlayerController>().feet.GetComponent<Collider2D>().bounds.center.x;

        if (playerFeetPointY < col.bounds.center.y + (col.bounds.size.y / 2) && playerFeetPointY > col.bounds.center.y - (col.bounds.size.y / 2))
        {
            resort = true;
        }

        // Behind
        // If player y is above upper edge of poly collider, player is "behind" object.
        if (playerFeetPointY > col.bounds.center.y + (col.bounds.size.y/2))
        {
            // Sort order for this object must be in front of player.
            // But, if object must always render in front of certain other objects, parse through the list of objects
            //    and find the one with the current largest sortingOrder. Use this as the current object's reference point.
            if (alwaysBehind != null && alwaysBehind.Count > 0)
            {
                int ord = 0;
                foreach (GameObject obj in alwaysBehind)
                {
                    if (ord <= obj.GetComponent<Renderer>().sortingOrder && obj.GetComponent<Renderer>().sortingOrder > player.GetComponent<Renderer>().sortingOrder)
                    {
                        ord = obj.GetComponent<Renderer>().sortingOrder + 1;
                    }
                    else
                    {
                        ord = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                    }
                }
                Debug.Log("Rendering this object in front: " + name);
                ren.sortingOrder = ord;
                playerIsBehind = true;
                playerIsFront = false;
                resort = false;
            } else
            {
                ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                playerIsBehind = true;
                playerIsFront = false;
                resort = false;
            }
        }

        // In Front
        // If player y is below lower edge of poly collider, player is "in front" of object.
        if (playerFeetPointY < col.bounds.center.y - (col.bounds.size.y / 2))
        {
            // Sort order for this object must be in behind of player.
            ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder - sortOrderDiff;
            playerIsBehind = false;
            playerIsFront = true;
            resort = false;
        }

        // Left
        // If player x is left of leftmost edge of poly collider...
        if (playerFeetPointX < col.bounds.center.x - (col.bounds.size.x / 2))
        {
            if (!resort)
                return;
            if (left == ObjectDirection.Behind)
            {
                // Sort order for this object must be in front of player.
                // But, if object must always render in front of certain other objects, parse through the list of objects
                //    and find the one with the current largest sortingOrder. Use this as the current object's reference point.
                if (alwaysBehind != null && alwaysBehind.Count > 0)
                {
                    int ord = 0;
                    foreach (GameObject obj in alwaysBehind)
                    {
                        if (ord <= obj.GetComponent<Renderer>().sortingOrder && obj.GetComponent<Renderer>().sortingOrder > player.GetComponent<Renderer>().sortingOrder)
                        {
                            ord = obj.GetComponent<Renderer>().sortingOrder + 1;
                        }
                        else
                        {
                            ord = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                        }
                    } 
                    Debug.Log("Rendering this object in front: " + name);
                    ren.sortingOrder = ord;
                    playerIsBehind = true;
                    playerIsFront = false;
                } else
                {
                    ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                    playerIsBehind = true;
                    playerIsFront = false;
                }
            }
            else if (left == ObjectDirection.Front)
            {
                // Sort order for this object must be in behind of player.
                ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder - sortOrderDiff;
                playerIsBehind = false;
                playerIsFront = true;
            }
        }

        // Right
        // If player x is right of rightmost edge of poly collider...
        if (playerFeetPointX > col.bounds.center.x + (col.bounds.size.x / 2))
        {
            if (!resort)
                return;
            if (right == ObjectDirection.Behind)
            {
                // Sort order for this object must be in front of player.
                // But, if object must always render in front of certain other objects, parse through the list of objects
                //    and find the one with the current largest sortingOrder. Use this as the current object's reference point.
                if (alwaysBehind != null && alwaysBehind.Count > 0)
                {
                    int ord = 0;
                    foreach (GameObject obj in alwaysBehind)
                    {
                        if (ord <= obj.GetComponent<Renderer>().sortingOrder && obj.GetComponent<Renderer>().sortingOrder > player.GetComponent<Renderer>().sortingOrder)
                        {
                            ord = obj.GetComponent<Renderer>().sortingOrder + 1;
                        } else
                        {
                            ord = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                        }
                    }
                    Debug.Log("Rendering this object in front: " + name);
                    ren.sortingOrder = ord;
                    playerIsBehind = true;
                    playerIsFront = false;
                } else
                {
                    ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder + sortOrderDiff;
                    playerIsBehind = true;
                    playerIsFront = false;
                }
            }
            else if (right == ObjectDirection.Front)
            {
                // Sort order for this object must be in behind of player.
                ren.sortingOrder = player.GetComponent<Renderer>().sortingOrder - sortOrderDiff;
                playerIsBehind = false;
                playerIsFront = true;
            }
        }
    }
}

/// <summary>
/// Directional reference enums.
/// </summary>
[System.Serializable] public enum ObjectDirection { Behind, Front }
