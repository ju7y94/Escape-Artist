using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashLightController : MonoBehaviour
{
    [SerializeField] GameObject flashlight;
    [SerializeField] Image batteryBar;
    bool hasFlashlight;
    
    // Start is called before the first frame update
    void Start()
    {
        hasFlashlight = false;
        batteryBar.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && hasFlashlight == true && !flashlight.activeSelf)
        {
            flashlight.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.F) && hasFlashlight == true && flashlight.activeSelf)
        {
            flashlight.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.F) && hasFlashlight == false)
        {
            print("Please replace batteries!");
        }


        if (flashlight.activeSelf && batteryBar.fillAmount > 0)
        {
            batteryBar.fillAmount -= 1f/10 * Time.deltaTime;
            if (batteryBar.fillAmount <= 0)
            {
                hasFlashlight = false;
                flashlight.SetActive(false);
                print("Please replace batteries!");
            }
        }
    }

    public void SwitchFlashLight()
    {
        if (batteryBar.fillAmount <= 0.75f)
        {
            batteryBar.fillAmount += 0.25f;
        }
        else
        {
            batteryBar.fillAmount = 1f;
        }
        hasFlashlight = true;
        print(batteryBar.fillAmount);
    }

    public bool GetFlashLight()
    {
        return hasFlashlight;
    }
}
