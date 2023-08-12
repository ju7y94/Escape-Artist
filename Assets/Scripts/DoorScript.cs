using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    GameManager gameManagerScript;
    GameObject player;

    [SerializeField] bool needKey;
    [SerializeField] bool lockable;
    [SerializeField] GameObject greenLight;
    [SerializeField] GameObject redLight;

    enum KeyColour {green,red,blue,orange};

    [SerializeField] KeyColour keyColour;


    [SerializeField] GameObject destination;
    [SerializeField] Camera newCam;
    [SerializeField] Camera currentCam;

    [SerializeField] bool isCheckpoint;

    void Start()
    {
        gameManagerScript = GameObject.FindGameObjectWithTag("TheGameManager").GetComponent<GameManager>();

        player = gameManagerScript.player;
    }

    private void Update()
    {
        if (lockable)
        {
            if (needKey)
            {
                redLight.GetComponent<MeshRenderer>().material = gameManagerScript.redOn;
                greenLight.GetComponent<MeshRenderer>().material = gameManagerScript.greenOff;
            }
            else
            {
                redLight.GetComponent<MeshRenderer>().material = gameManagerScript.redOff;
                greenLight.GetComponent<MeshRenderer>().material = gameManagerScript.greenOn;
            }
        }
    }

    public void TriggerDoor(PlayerInvScript playerInv)
    {
        if (needKey == false)
        {
            player.transform.position = new Vector3(destination.transform.position.x, destination.transform.position.y, destination.transform.position.z);
            gameManagerScript.currentCam = newCam;
            newCam.enabled = true;
            newCam.tag = "MainCamera";

            currentCam.tag = "Untagged";
            currentCam.enabled = false;

        }
        else if (needKey)
        {
            if(keyColour == KeyColour.green)
            {
                if(playerInv.heldItem != null)
                {
                    if(playerInv.heldItemData.itemID == 1)
                    {
                        needKey = false;
                        playerInv.ClearInv();
                    }
                }
            }
            else if(keyColour == KeyColour.red)
            {
                if (playerInv.heldItem != null)
                {
                    if (playerInv.heldItemData.itemID == 2)
                    {
                        needKey = false;
                        playerInv.ClearInv();
                    }
                }
            }
            else if(keyColour == KeyColour.blue)
            {
                if (playerInv.heldItem != null)
                {
                    if (playerInv.heldItemData.itemID == 3)
                    {
                        needKey = false;
                        playerInv.ClearInv();
                    }
                }
            }
            else if(keyColour == KeyColour.orange)
            {
                if (playerInv.heldItem != null)
                {
                    if (playerInv.heldItemData.itemID == 4)
                    {
                        needKey = false;
                        playerInv.ClearInv();
                    }
                }
            }
        }

        if (isCheckpoint)
        {
            gameManagerScript.UpdateCheckpoint(destination.transform);
            gameManagerScript.lastCheckpointCam = newCam;
        }
    }
}
