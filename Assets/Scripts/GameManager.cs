using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    PlayerControl playerControlScript;

    Transform lastCheckpoint;
    public Camera currentCam;
    public Camera lastCheckpointCam;

    public Material redOn;
    public Material redOff;
    public Material greenOff;
    public Material greenOn;

    void Start()
    {
        playerControlScript = player.GetComponent<PlayerControl>();

        lastCheckpoint = player.transform;
    }

    public void PlayerDeath()
    {
        player.transform.position = lastCheckpoint.transform.position;

        currentCam.enabled = false;
        lastCheckpointCam.enabled = true;
    }

    public void UpdateCheckpoint(Transform checkpoint)
    {
        lastCheckpoint = checkpoint;
    }
}
