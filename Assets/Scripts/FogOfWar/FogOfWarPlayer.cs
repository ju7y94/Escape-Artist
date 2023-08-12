using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarPlayer : MonoBehaviour
{
    //public Transform fogOfWarPlayer;
    public GameObject fogOfWarPlane;
    public Material enemyMat;
    public Material transparentMat;
    float fogRadius;
    public int playerNumber = 1;
    public LayerMask spotlightLayerMask;
    //Collider[] spotlightColliders;
    void Start()
    {
        fogOfWarPlane = GameObject.FindGameObjectWithTag("FogPlane");
        fogRadius = fogOfWarPlane.GetComponent<Renderer>().sharedMaterial.GetFloat("_FogRadius");
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = fogRadius - 5f;
    }

    void Update()
    {
        fogOfWarPlane.GetComponent<Renderer>().sharedMaterial.SetVector("_Player" + playerNumber.ToString() + "_Pos", gameObject.transform.position);

        // spotlightColliders = Physics.OverlapSphere(gameObject.transform.position, fogRadius -2f, spotlightLayerMask);

        // foreach (Collider collider in spotlightColliders)
        // {
        //     OnOverlapSphereEnter(collider);
        // }
    }

    private void OnOverlapSphereEnter(Collider other) {
        // if(other.CompareTag("Spotlight"))
        // {
        //     other.gameObject.GetComponent<Light>().intensity = 10f;
        //     print("Spotlight enter");
        // }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Renderer>().material = enemyMat;
        }

        if(other.CompareTag("Spotlight"))
        {
            other.gameObject.GetComponent<Light>().intensity = 5f;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Renderer>().material = transparentMat;
        }

        if(other.CompareTag("Spotlight"))
        {
            other.gameObject.GetComponent<Light>().intensity = 0f;
        }
    }

    private void OnTriggerStay(Collider other) {
        if(other.CompareTag("Enemy"))
        {
            other.GetComponent<Renderer>().material = enemyMat;
        }

        if(other.CompareTag("Spotlight"))
        {
            other.gameObject.GetComponent<Light>().intensity = 5f;
        }
    }

void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fogRadius -2f);
    }
}
