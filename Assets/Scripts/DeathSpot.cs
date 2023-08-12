// Script written by Louis M. Green [001103565]
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathSpot : MonoBehaviour
{
    DDAManager DDAManager;
    [SerializeField] float DDAVal;

    GameManager gameManagerScript;

    private void Start()
    {
        DDAManager = GameObject.FindGameObjectWithTag("DDAManager").GetComponent<DDAManager>();

        gameManagerScript = GameObject.FindGameObjectWithTag("TheGameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "Player")
        {
            PlayerControl player = other.gameObject.GetComponent<PlayerControl>();

            DDAFunction();

            gameManagerScript.PlayerDeath();
        }
    }

    void DDAFunction()
    {
        DDAManager.DDADown(DDAVal);
    }
}
