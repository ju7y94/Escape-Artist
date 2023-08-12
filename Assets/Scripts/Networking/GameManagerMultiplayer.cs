using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMultiplayer : MonoBehaviour
{
    public GameObject player;


    Transform lastCheckpoint;
    public Camera currentCam;
    public Camera lastCheckpointCam;

    void Start()
    {
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
