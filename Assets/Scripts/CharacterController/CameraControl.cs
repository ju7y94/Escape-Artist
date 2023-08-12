using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    GameObject player;
    [SerializeField] bool isStatic;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic)
        {

        }
        else
        {
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}
