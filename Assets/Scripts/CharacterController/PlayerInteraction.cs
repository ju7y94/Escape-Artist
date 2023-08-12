using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    PlayerInvScript playerInv;
    PlayerControl playerControlScript;

    [SerializeField] GameObject torchMessage;

    private void Start()
    {
        playerInv = GetComponent<PlayerInvScript>();
        playerControlScript = GetComponent<PlayerControl>();
    }

    public void TriggerInteraction(GameObject interactedObject)
    {
        if (interactedObject.tag == "Door")
        {
            if (playerControlScript.hasFlashLight)
            {
                print("hit door");

                interactedObject.GetComponent<DoorScript>().TriggerDoor(playerInv);
            }
            else
            {
                torchMessage.SetActive(true);
                Invoke("RemoveTorchMessage", 3f);
            }
            

        }
        else if (interactedObject.tag == "FlashLightPickUp")
        {
            if (!playerControlScript.hasFlashLight)
            {
                playerControlScript.hasFlashLight = true;
            }

            playerControlScript.batteryBar.fillAmount = 1f;

            Destroy(interactedObject);

        }
        else if (interactedObject.tag == "BatteryPickUp")
        {
            if (playerControlScript.batteryBar.fillAmount <= 0.75f)
            {
                playerControlScript.batteryBar.fillAmount += 0.25f;
            }
            else
            {
                playerControlScript.batteryBar.fillAmount = 1f;
            }

            Destroy(interactedObject);
        }
        else if (interactedObject.tag == "ItemToPick")
        {
            if (playerInv.heldItem != null)
            {
                playerInv.SwapItem(interactedObject);
                Destroy(interactedObject);
            }
            else
            {
                playerInv.AddToInventory(interactedObject);
                Destroy(interactedObject);
            }
        }
        else
        {
            print("Cant interact with this");
        }
    }

    void RemoveTorchMessage()
    {
        torchMessage.SetActive(false);
    }
}
