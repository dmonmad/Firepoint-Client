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

    }

    private void Update()
    {
        if(!UIManager.instance.IsUIOpen())
        if (Input.GetButtonDown("Fire1"))
        {            
            weaponManager.FireWeapon(camTransform);
            Debug.DrawLine(camTransform.position, camTransform.forward * 50, Color.blue, 10f);
        }

        if (Input.GetButtonUp("Fire1"))
        {
            weaponManager.StopFiringWeapon();
        }

        if(Input.GetButtonDown("Reload Weapon"))
        {
            ClientSend.ReloadWeapon();
        }

        if (Input.GetButtonDown("Drop Weapon"))
        {
            ClientSend.PlayerDropWeapon(camTransform.forward);
            weaponManager.DropWeapon(weaponDropper.forward, weaponDropper);
            Debug.DrawLine(camTransform.position, camTransform.forward * 50, Color.blue, 10f);
        }

        if (Input.GetButtonDown("Select Primary"))
        {
            weaponManager.SelectWeapon(0);
        }

        if (Input.GetButtonDown("Select Secondary"))
        {
            weaponManager.SelectWeapon(1);
        }

        if (Input.GetButtonDown("Select Tertiary"))
        {
            weaponManager.SelectWeapon(2);
        }

        if (Input.GetButtonDown("Select Equipment"))
        {
            //Select equipment weapon
        }

        if (Input.GetButtonDown("Change Weapon"))
        {
            weaponManager.SelectLastSelectedWeapon();
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