using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    GameManager gameManagerScript;

    float horzDirection;
    float vertDirection;

    Vector3 focus;
    Vector3 playerForward;
    [SerializeField] GameObject target;

    [SerializeField] GameObject playerDirectionObj;
    [SerializeField] float interactRange;

    PlayerInvScript playerInv;
    PlayerInteraction playerInteractionScript;

    Rigidbody playerRB;

    [SerializeField] float movementSpeed;
    float currentMovementSpeed;

    public bool hasFlashLight;
    [SerializeField] GameObject flashLight;
    public Image batteryBar;

    Camera currentCam;

    bool canRayCast;

    Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        currentMovementSpeed = movementSpeed;

        gameManagerScript = GameObject.FindGameObjectWithTag("TheGameManager").GetComponent<GameManager>();
        playerRB = GetComponent<Rigidbody>();
        playerInv = GetComponent<PlayerInvScript>();
        playerInteractionScript = GetComponent<PlayerInteraction>();

        currentCam = gameManagerScript.currentCam;

        batteryBar.fillAmount = 0f;

    }

    void Update()
    {

        MovePlayer();
        Sneak();

        if (hasFlashLight)
        {
            FlashLight();
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit mousePosHit;

        if (Physics.Raycast(ray, out mousePosHit))
        {
            target.transform.position = focus = mousePosHit.point;
            playerForward = Vector3.Normalize(new Vector3(focus.x - transform.position.x, 0f, focus.z - transform.position.z));
            transform.forward = playerForward;
        }

        playerRB.velocity = new Vector3(horzDirection, 0, vertDirection) * currentMovementSpeed;


        if (Input.GetMouseButtonDown(0) && canRayCast)
        {
            RaycastHit playerDirHit;

            if (Physics.Raycast(playerDirectionObj.transform.position, playerDirectionObj.transform.TransformDirection(Vector3.forward), out playerDirHit, interactRange))
            {
                playerInteractionScript.TriggerInteraction(playerDirHit.transform.gameObject);
            }

            //if(!anim.GetBool("moving"))
            //anim.SetBool("interacting", true);

            canRayCast = false;
        }

        if (Input.GetMouseButtonUp(0))
        {
            canRayCast = true;
        }
    }

    public void EndInteractAnimation()
    {
        anim.SetBool("interacting", false);
    }

    public void MovePlayer()
    {
        horzDirection = Input.GetAxisRaw("Horizontal");
        vertDirection = Input.GetAxisRaw("Vertical");

        Vector3 movementVector = new Vector3(horzDirection, 0, vertDirection);

        movementVector = Quaternion.LookRotation(playerForward) * movementVector;

        anim.SetFloat("vertical", movementVector.z);
        anim.SetFloat("horizontal", movementVector.x);

        bool isMoving = (horzDirection != 0 || vertDirection != 0);

        anim.SetBool("moving", isMoving);
    }

    public void Sneak()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentMovementSpeed = movementSpeed * 0.6f;
            anim.SetBool("crouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            currentMovementSpeed = movementSpeed;
            anim.SetBool("crouch", false);
        }
    }

    public void TriggerDeath()
    {
        gameManagerScript.PlayerDeath();
    }

    public void FlashLight()
    {
        if (Input.GetKeyDown(KeyCode.F) && hasFlashLight == true && !flashLight.activeSelf)
        {
            flashLight.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.F) && hasFlashLight == true && flashLight.activeSelf)
        {
            flashLight.SetActive(false);
        }
        else if (Input.GetKeyDown(KeyCode.F) && hasFlashLight == false)
        {
            print("Please replace batteries!");
        }

        if (flashLight.activeSelf && batteryBar.fillAmount > 0)
        {
            batteryBar.fillAmount -= 1f / 10 * Time.deltaTime;

            if (batteryBar.fillAmount <= 0)
            {
                hasFlashLight = false;
                flashLight.SetActive(false);
            }
        }
    }

    public void BeginInteraction()
    {

    }
}
