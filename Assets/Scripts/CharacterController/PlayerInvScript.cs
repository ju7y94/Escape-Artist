using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInvScript : MonoBehaviour
{
    PlayerControl playerControlScript;
    PlayerInteraction playerInteractionScript;

    public GameObject heldItem;
    public ItemObject heldItemData;
    [SerializeField] Image itemIcon;

    private void Start()
    {
        playerControlScript = GetComponent<PlayerControl>();
        playerInteractionScript = GetComponent<PlayerInteraction>();
    }

    public void AddToInventory(GameObject itemToAdd)
    {
        heldItem = itemToAdd.GetComponent<ItemPickup>().objectPrefab;
        heldItemData = heldItem.GetComponent<ItemObject>();

        itemIcon.sprite = heldItemData.itemIcon;
    }

    public void SwapItem(GameObject itemToSwap)
    {
        GameObject orgHeldItem = heldItem;
        ItemObject orgHeldItemObject = orgHeldItem.GetComponent<ItemObject>();

        AddToInventory(itemToSwap);

        Instantiate(orgHeldItemObject.pickUpObject, itemToSwap.transform.position, Quaternion.identity);
    }

    public void ClearInv()
    {
        heldItem = null;
        heldItemData = null;
        itemIcon.sprite = null;
    }
}
