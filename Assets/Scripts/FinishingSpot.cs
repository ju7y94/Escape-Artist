using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishingSpot : MonoBehaviour
{
    [SerializeField] GameObject endPanel;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.transform.tag == "Player")
        {
            print("finished");
            endPanel.SetActive(true);
        }
    }
}
