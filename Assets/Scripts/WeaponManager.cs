using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] Inventory = new GameObject[3];
    public Transform shootingOrigin;
    public int selectedIndex = -1;
    public int lastSelectedIndex = -1;
    public float dropForce = 30f;

    public void Start()
    {
        selectedIndex = -1;
    }

    public void SelectWeapon(int _index)
    {

        if (Inventory[_index] != null)
        {
            if (!Inventory[_index].Equals(Inventory[selectedIndex]))
            {
                Inventory[selectedIndex].SetActive(false);
                selectedIndex = _index;
                Inventory[selectedIndex].SetActive(true);
            }
        }
    }

    public void PickupWeapon(GameObject _itemObj, Transform _weaponHolder)
    {

        Debug.Log("PickupWeapon");

        for (int i = 0; i < Inventory.Length; i++)
        {
            Debug.Log("Checkeando slot " + i);
            if (Inventory[i] == null)
            {
                Debug.Log("Guardando arma en el slot " + i);
                Debug.Log("Selected index = " + selectedIndex);
                Inventory[i] = _itemObj;
                Debug.Log("itemObj transforming to 0");
                _itemObj.transform.parent = _weaponHolder.transform;
                _itemObj.transform.localPosition = Vector3.zero;
                _itemObj.transform.rotation = new Quaternion(0, 0, 0, 0);

                if (selectedIndex != -1)
                {
                    Debug.Log("Setting to false");
                    _itemObj.SetActive(false);
                }
                else
                {
                    selectedIndex = i;
                }

                Debug.Log("Selected index = " + selectedIndex);

                Rigidbody _rb = Inventory[selectedIndex].GetComponent<Rigidbody>();
                if (_rb)
                {
                    _rb.isKinematic = true;
                    break;
                }
            }
        }


    }

    public void DropWeapon(Vector3 _dropVector, Transform _weaponDropper)
    {
        if (Inventory[selectedIndex] != null)
        {
            Debug.Log("Has selected weapon");
            if (Inventory[selectedIndex].activeInHierarchy)
            {
                Debug.Log("Getting rb");
                Rigidbody _rb = Inventory[selectedIndex].GetComponent<Rigidbody>();
                if (_rb)
                {
                    Debug.Log("Throwing weapon");
                    Inventory[selectedIndex].transform.parent = null;
                    Inventory[selectedIndex].transform.position = _weaponDropper.position;
                    Inventory[selectedIndex] = null;
                    selectedIndex = -1;
                    _rb.isKinematic = false;
                    _rb.AddForce(gameObject.transform.forward * dropForce, ForceMode.Force);
                }
            }
        }
    }

    public void FireWeapon(Transform _camTransform)
    {
        if(!shootingOrigin)
            shootingOrigin = _camTransform;

        if (IsWeaponSelected())
        {
            if (Inventory[selectedIndex].GetComponent<Weapon>().Shoot(_camTransform))
            {
                Debug.Log("WeaponManager -> Shoot = true");
            }          
        }
    }

    public void StopFiringWeapon()
    {
        if (IsWeaponSelected())
        {
            Inventory[selectedIndex].GetComponent<Weapon>().StopFiring();
        }
    }

    public bool IsWeaponSelected()
    {
        return selectedIndex == -1 ? false : true;
    }
}
