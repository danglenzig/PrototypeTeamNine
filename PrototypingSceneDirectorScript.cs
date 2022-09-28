using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PrototypingSceneDirectorScript : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject projectileObjectPrefab;

    public float moveSpeed = 10f;
    public float rotateSpeed = .1f;
    public float maxVelocity = 10f;
    public float mouseDampening = 100f;
    public float launchPower = 1000f;

    private Keyboard myKB;
    private Mouse myMouse;
    private Rigidbody playerRB;
    private Vector3 playerMoveVector;
    private Vector3 playerRotateVector;
    private Transform playerProjectileStartTransform;
    private bool moveInput = false;
    
    // Start is called before the first frame update
    void Start()
    {
        myKB = Keyboard.current;
        myMouse = Mouse.current;
        playerRB = playerObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (myKB.escapeKey.wasPressedThisFrame)
        {
            Application.Quit();
        }

        // gather forward-back movement input
        if (myKB.wKey.isPressed)
        {
            playerMoveVector.z = 1f;
        } else if (myKB.sKey.isPressed)
        {
            playerMoveVector.z = -1;
        } else
        {
            playerMoveVector.z = 0f;
        }

        // gather right-left movement input
        if (myKB.dKey.isPressed)
        {
            playerMoveVector.x = 1f;
        } else if (myKB.aKey.isPressed)
        {
            playerMoveVector.x = -1f;
        } else
        {
            playerMoveVector.x = 0f;
        }

        
        // rotate the player in the direction they are moving
        playerRotateVector.y = playerMoveVector.x;

        // left click or space key to fire
        if (myMouse.leftButton.wasPressedThisFrame || myKB.spaceKey.wasPressedThisFrame)
        {
            PlayerFireProjectile();
        }
    }

    private void FixedUpdate()
    {
        // movement
        if(playerRB.velocity.magnitude > maxVelocity)
        {
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, maxVelocity);
        }

        playerRB.AddRelativeForce(playerMoveVector * moveSpeed, ForceMode.Acceleration);

        // rotation
        playerObject.transform.eulerAngles += playerRotateVector * rotateSpeed;
    }

    void PlayerFireProjectile()
    {
        GameObject projectile = Instantiate(projectileObjectPrefab, new Vector3(playerObject.transform.position.x, playerObject.transform.position.y + 1f, playerObject.transform.position.z + 1), playerObject.transform.rotation);
        Rigidbody projectileRB = projectile.GetComponent<Rigidbody>();
        projectileRB.AddRelativeForce(new Vector3(0, 0, launchPower), ForceMode.Impulse);
    }
}
