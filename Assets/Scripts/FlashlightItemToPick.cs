using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightItemToPick : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    FlashLightController flaslightController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            flaslightController = collision.gameObject.GetComponent<FlashLightController>();
            flaslightController.SwitchFlashLight();
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
