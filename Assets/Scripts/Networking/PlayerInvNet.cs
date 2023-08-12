using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class PlayerInvNet : NetworkBehaviour
{
    RaycastHit interactionHit;
    [SerializeField] float interactRange;
    public Image batteryBar;
    GameObject currentItem, swapItem, itemToSpawn;
    ItemData itemData;
    [SerializeField] Image[] itemIcon;
    [SerializeField] int playerIndex;
    [SerializeField] Sprite[] itemSprites;
    public NetworkVariable<ushort> spriteIndex = new(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    GameObject spawnInstance;
    PlayerControlMultiplayer playControlNet;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerIndex = (int)OwnerClientId;
        print("Player index = " + playerIndex);
        batteryBar.fillAmount = 0f;
        spriteIndex.OnValueChanged += SpriteChanged;
        playControlNet = GetComponent<PlayerControlMultiplayer>();
    }

    private void Update() {
        if (!IsOwner || !Application.isFocused)
        {
            return;
        }

        FollowMouse();
        if(Input.GetMouseButtonDown(0))
        {
            //if (Physics.SphereCast(transform.position, 0.25f, transform.forward, out interactionHit, interactRange))
            CheckInteractionServerRpc();
        }
        
    }

    void SpriteChanged(ushort previousValue, ushort newValue)
    {
        Debug.Log("Server sprite index value " + spriteIndex.Value);
    }

    
    void FollowMouse() {
        
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
        transform.forward = Vector3.Normalize(new Vector3(hit.point.x - transform.position.x, 0f, hit.point.z - transform.position.z));
    }

    [ServerRpc]
    void CheckInteractionServerRpc()
    {
        if (Physics.Raycast(transform.position, transform.forward, out interactionHit, interactRange))
            switch (interactionHit.transform.tag)
                {
                    case("Door"):
                    break;

                    case("FlashLightPickUp"):
                        playControlNet.SwitchFlashLight();
                        batteryBar.fillAmount = 1f;
                        //DespawnObjectServerRpc();
                        interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
                    break;

                    case("BatteryPickUp"):
                        batteryBar.fillAmount = Mathf.Clamp01(batteryBar.fillAmount + 0.25f);
                        //DespawnObjectServerRpc();
                        interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
                    break;

                    case("ItemToPick"):
                        CheckToAddToInventoryServerRpc();
                        //interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
                    break;

                    default:
                        print("Can't interact with this");
                    break;
            
                }
    }

    [ServerRpc]
    void CheckToAddToInventoryServerRpc() {
        if (interactionHit.transform.gameObject == null) return;
        
        if(itemToSpawn == null)
        {
            currentItem = interactionHit.transform.gameObject;
            itemIcon[playerIndex].sprite = currentItem.GetComponent<ItemPickupNet>().spriteIcon;
            itemToSpawn = currentItem.GetComponent<ItemPickupNet>().objectPrefab.GetComponent<ItemData>().objectPrefab;
            interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
        }
        else if(itemToSpawn != null)
        {
            swapItem = interactionHit.transform.gameObject;
            itemIcon[playerIndex].sprite = swapItem.GetComponent<ItemPickupNet>().spriteIcon;
            spawnInstance = Instantiate(itemToSpawn, interactionHit.transform.position, Quaternion.identity);
            spawnInstance.GetComponent<NetworkObject>().Spawn();
            itemToSpawn = swapItem.GetComponent<ItemPickupNet>().objectPrefab.GetComponent<ItemData>().objectPrefab;
            interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
        }

        // ushort spriteID = currentItem.GetComponent<ItemPickupNet>().itemID;
        // spriteIndex.Value = spriteID;
    }

            // if (Input.GetMouseButtonDown(0)) {
        
        //     if (Physics.SphereCast(transform.position, 0.25f, transform.forward, out interactionHit, interactRange)) {
        //         switch (interactionHit.transform.tag)
        //         {
        //             case("Door"):
        //             break;

        //             case("FlashLightPickUp"):
        //                 playControlNet.SwitchFlashLight();
        //                 batteryBar.fillAmount = 1f;
        //                 interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
        //             break;

        //             case("BatteryPickUp"):
        //                 batteryBar.fillAmount = Mathf.Clamp01(batteryBar.fillAmount + 0.25f);
        //                 interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
        //             break;

        //             case("ItemToPick"):
        //                 if (heldItem == null) {
        //                     AddToInventoryClientRpc();
        //                 }
        //                 else if (heldItem != null) {
        //                     SwapItemClientRpc();
        //                 }
        //                 interactionHit.transform.gameObject.GetComponent<NetworkObject>().Despawn();
        //             break;

        //             default:
        //                 print("Can't interact with this");
        //             break;
            
        //         }
        //     }
        // }
}
