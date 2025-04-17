using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Pickup heldItem;
    [SerializeField] private GameObject heldItemContainer;

    [SerializeField] Vector3 heldItemOffset;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + heldItemOffset, Vector3.one * 0.5f);
    }

    private void Start()
    {
        heldItem = null;

        if (!heldItemContainer)
        {
            heldItemContainer = new GameObject("HeldItemContainer")
            {
                transform =
                {
                    parent = transform,
                    localPosition = heldItemOffset
                }
            };
        }
    }

    public void PickupItem(Pickup pickup)
    {
        
        if (heldItem || !heldItemContainer)
        {
            #if UNITY_EDITOR
            Debug.Log("Already holding an item or gameobject not valid");
            #endif
            return;
        }
        
        #if UNITY_EDITOR
        Debug.Log("Picked up item");
        #endif
        
        // update UI
        if (InventoryView.iv != null)
        {
            InventoryView.iv.Pickup(pickup);
        }
        
        heldItem = pickup;
        heldItem.transform.parent = heldItemContainer.transform;
        heldItem.transform.localPosition = Vector3.zero;
        
        if (heldItem.rb)
            heldItem.rb.isKinematic = true;
    }


    public Pickup GetHeld()
    {
        return heldItem;
    }

    public void DropItem()
    {
        if (heldItem == null) 
            return;
        if (heldItem.rb)
            heldItem.rb.isKinematic = true;
        
        heldItem.transform.SetParent(null);
        heldItem = null;
    }
}
