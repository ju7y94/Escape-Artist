using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim_Script : MonoBehaviour
{
    // Declare an object to store the animator component
    public Animator anim;

    void Update()
    {
        // Set the "vertical" parameter in the animator to the value of the vertical axis
        //anim.SetFloat("vertical", Input.GetAxis("Vertical"));

        // Set the "horizontal" parameter in the animator to the value of the horizontal axis
        //anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));

        // Check if the left mouse button is pressed down, to set the "interact" parameter in the animator
        if (Input.GetMouseButtonDown(0)) 
        {
            //anim.SetTrigger("interact");
            //anim.SetBool("interacting", false);
        }


        // Check if the left control key is being held down, to set the "crouch" parameter in the animator
        if (Input.GetKey(KeyCode.C)) 
        {
            anim.SetBool("crouch", true);
        }
        else
        {
            anim.SetBool("crouch", false);
        }
    }
}