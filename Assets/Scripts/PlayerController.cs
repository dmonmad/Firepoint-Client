using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Transform camTransform;
    public Transform weaponTransform;
    public Transform weaponDropper;
    public WeaponManager weaponManager;

    private void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {            
            weaponManager.FireWeapon(camTransform);
            Debug.DrawLine(camTransform.position, camTransform.forward * 50, Color.blue, 10f);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            weaponManager.StopFiringWeapon();
        }

        if (Input.GetButtonDown("Drop Weapon"))
        {
            Debug.Log("Pressing G");
            ClientSend.PlayerDropWeapon(camTransform.forward);
            weaponManager.DropWeapon(weaponDropper.forward, weaponDropper);
            Debug.DrawLine(camTransform.position, camTransform.forward * 50, Color.blue, 10f);
        }

        if (Input.GetButtonDown("Select Primary"))
        {
            //Select first weapon
        }

        if (Input.GetButtonDown("Select Secondary"))
        {
            //Select second weapon
        }

        if (Input.GetButtonDown("Select Knife"))
        {
            //Select third weapon
        }

        if (Input.GetButtonDown("Select Equipment"))
        {
            //Select equipment weapon
        }

        if (Input.GetButtonDown("Change Weapon"))
        {
            //select last weapon
        }

    }

    private void FixedUpdate()
    {
        SendInputToServer();
    }

    /// <summary>Sends player input to the server.</summary>
    private void SendInputToServer()
    {
        bool[] _inputs = new bool[]
        {
            Input.GetKey(KeyCode.W),
            Input.GetKey(KeyCode.S),
            Input.GetKey(KeyCode.A),
            Input.GetKey(KeyCode.D),
            Input.GetButtonDown("Jump")
        };

        ClientSend.PlayerMovement(_inputs);


    }
}