using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerControlMultiplayer : NetworkBehaviour
{
    float horzDirection;
    float vertDirection;
    Rigidbody playerRB;
    [SerializeField] float movementSpeed;
    float currentMovementSpeed;
    public bool hasFlashLight;
    [SerializeField] GameObject flashLight;
    Animator anim;
    float positionRange = 5f;
    PlayerInvNet playerInvNet;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        playerInvNet = GetComponent<PlayerInvNet>();
        transform.position = new Vector3(Random.Range(positionRange, -positionRange), 0, Random.Range(positionRange, -positionRange));
        transform.rotation = new Quaternion(0, Random.Range(0,360), 0, 0);
        anim = GetComponent<Animator>();
        currentMovementSpeed = movementSpeed;
        hasFlashLight = false;
        playerRB = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!IsOwner || !Application.isFocused)
        {
            return;
        }

        Movement();

        Flashlight();
    }
    void Movement()
    {
        horzDirection = Input.GetAxisRaw("Horizontal");
        vertDirection = Input.GetAxisRaw("Vertical");

        anim.SetFloat("vertical", Input.GetAxis("Vertical"));
        anim.SetFloat("horizontal", Input.GetAxis("Horizontal"));

        if(Input.GetAxisRaw("Horizontal") > 0 || Input.GetAxisRaw("Horizontal") < 0)
        anim.SetBool("moving", true);

        if(Input.GetAxisRaw("Vertical") > 0 || Input.GetAxisRaw("Vertical") < 0)
        anim.SetBool("moving", true);

        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0)
        anim.SetBool("moving", false);

        if(GroundCheck())
        {
            playerRB.velocity = new Vector3(horzDirection, 0, vertDirection).normalized * currentMovementSpeed;
        }
    }

    void Sneak()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMovementSpeed = movementSpeed * 0.6f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMovementSpeed = movementSpeed;
        }
    }

    void Flashlight()
    {
        if (hasFlashLight)
        {
            if (Input.GetKeyDown(KeyCode.F) && GetFlashLight() && !flashLight.activeSelf)
            {
                flashLight.SetActive(true);
            }
            else if (Input.GetKeyDown(KeyCode.F) && GetFlashLight() && flashLight.activeSelf)
            {
                flashLight.SetActive(false);
            }
            else if (Input.GetKeyDown(KeyCode.F) && !GetFlashLight())
            {
                print("Please replace batteries!");
            }

            if (flashLight.activeSelf && playerInvNet.batteryBar.fillAmount > 0)
            {
                playerInvNet.batteryBar.fillAmount -= 1f / 10 * Time.deltaTime;

                if (playerInvNet.batteryBar.fillAmount <= 0)
                {
                    hasFlashLight = false;
                    flashLight.SetActive(false);
                }
            }
        }
    }
    public void SwitchFlashLight()
    {
        hasFlashLight = !hasFlashLight;
    }
    public bool GetFlashLight()
    {
        return hasFlashLight;
    }
    bool GroundCheck()
    {
        RaycastHit hit;
        Vector3 rayStart = transform.position + (Vector3.up * 0.1f);  // offset ray start position
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 2.5f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
