using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponManager : MonoBehaviour
{
    public GameObject[] InventoryWeapons = new GameObject[3];
    public Image[] InventoryHudSlots = new Image[3];
    public TextMeshProUGUI bulletsText;
    public Image bulletIcon;
    public GameObject selectedWeapon;
    public GameObject lastSelectedWeapon;
    public float dropForce = 100f;

    public void Start()
    {
        selectedWeapon = null;
    }

    public void Update()
    {
        UpdateBullets();
    }

    public void SelectWeapon(int _index)
    {
        if (InventoryWeapons[_index] != null)
        {
            if (InventoryWeapons[_index] != selectedWeapon)
            {
                ClientSend.ChangeWeapon(_index);
                DeselectWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                selectedWeapon.SetActive(false);
                selectedWeapon = InventoryWeapons[_index];
                SelectWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                selectedWeapon.SetActive(true);
            }
        }
    }

    public void ChangeWeapon(int _index)
    {
        if (InventoryWeapons[_index] != null)
        {
            if (InventoryWeapons[_index] != selectedWeapon)
            {
                if (selectedWeapon)
                    selectedWeapon.SetActive(false);

                selectedWeapon = InventoryWeapons[_index];
                selectedWeapon.SetActive(true);
            }
        }
    }

    public void PickupWeapon(GameObject _itemObj, Transform _weaponHolder)
    {
        for (int i = 0; i < InventoryWeapons.Length; i++)
        {
            if (InventoryWeapons[i] == null)
            {
                InventoryWeapons[i] = _itemObj;
                _itemObj.transform.parent = _weaponHolder.transform;
                _itemObj.transform.localPosition = Vector3.zero;
                _itemObj.transform.rotation = new Quaternion(0, 0, 0, 0);
                PickupWeaponImage(InventoryHudSlots[i], _itemObj.GetComponent<Weapon>().hudSprite);

                if (selectedWeapon != null)
                {
                    _itemObj.SetActive(false);
                    DeselectWeaponImage(InventoryHudSlots[i]);
                }
                else
                {
                    selectedWeapon = InventoryWeapons[i];
                    SelectWeaponImage(InventoryHudSlots[i]);
                }

                Rigidbody _rb = _itemObj.GetComponent<Rigidbody>();
                if (_rb)
                {
                    _rb.isKinematic = true;
                    break;
                }
            }
        }


    }

    public void SelectLastSelectedWeapon()
    {
        if (lastSelectedWeapon != null)
        {
            if (lastSelectedWeapon != null)
            {
                SelectWeapon(GetWeaponIndex(lastSelectedWeapon));
            }
            else
            {
                lastSelectedWeapon = null;
            }
        }
    }

    public void DropWeapon(Vector3 _dropVector, Transform _weaponDropper)
    {
        if (selectedWeapon != null)
        {
            Debug.Log("WM - DROPWEAPON - Has selected weapon");
            if (selectedWeapon.activeInHierarchy)
            {
                Debug.Log("WM - DROPWEAPON - Getting rb");
                Rigidbody _rb = selectedWeapon.GetComponent<Rigidbody>();
                if (_rb)
                {
                    Debug.Log("WM - DROPWEAPON - Throwing weapon");
                    ClearWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                    selectedWeapon.transform.parent = null;
                    selectedWeapon.transform.position = _weaponDropper.position;
                    selectedWeapon = null;
                    selectedWeapon = null;
                    _rb.isKinematic = false;
                    _rb.AddForce(gameObject.transform.forward * dropForce, ForceMode.Force);
                }
            }
        }
    }

    public void FireWeapon(Transform _camTransform)
    {

        if (IsWeaponSelected())
        {
            if (selectedWeapon.GetComponent<Weapon>().Shoot(_camTransform))
            {
                Debug.Log("WeaponManager -> Shoot = true");
            }
        }
    }

    public void StopFiringWeapon()
    {
        if (IsWeaponSelected())
        {
            selectedWeapon.GetComponent<Weapon>().StopFiring();
        }
    }

    public bool IsWeaponSelected()
    {
        if (selectedWeapon != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PickupWeaponImage(Image _image, Sprite _sprite)
    {
        Debug.Log("PICKUP WEAPON IMAGE");
        if (_image)
        {
            _image.sprite = _sprite;
            _image.color = Constants.WeaponSpriteSelected;
        }
    }

    public void SelectWeaponImage(Image _image)
    {
        Debug.Log("SELECT WEAPON IMAGE");
        if (_image)
        {
            _image.color = Constants.WeaponSpriteSelected;
        }
    }

    public void ClearWeaponImage(Image _image)
    {
        Debug.Log("CLEAR WEAPON IMAGE");
        if (_image)
        {
            _image.sprite = null;
            _image.color = Constants.WeaponSpriteClear;
        }
    }

    public void DeselectWeaponImage(Image _image)
    {
        Debug.Log("DESELECTING WEAPON IMAGE");
        if (_image)
        {
            _image.color = Constants.WeaponSpriteUnselected;
        }
    }

    public void UpdateBullets()
    {
        if (IsWeaponSelected())
        {
            if (bulletsText && bulletIcon)
            {
                bulletsText.SetText(FormattedBullets(selectedWeapon.GetComponent<Weapon>()));
                bulletIcon.enabled = true;
            }
        }
        else
        {
            if (bulletsText && bulletIcon)
            {
                bulletsText.SetText("");
                bulletIcon.enabled = false;
            }
        }
    }



    public void DropAllWeapons(Vector3 dropVector, Transform _weaponDropper)
    {
        for (int i = 0; i < InventoryWeapons.Length; i++)
        {
            Debug.Log(i);
            if (InventoryWeapons[i])
            {
                selectedWeapon = InventoryWeapons[i];
                Debug.Log(selectedWeapon);
                selectedWeapon.SetActive(true);
                DropWeapon(gameObject.transform.forward, _weaponDropper);
            }
        }
    }

    public string FormattedBullets(Weapon _weapon)
    {
        return _weapon.clip + " / " + _weapon.ammo;
    }

    public int GetWeaponIndex(GameObject _weapon)
    {
        for (int i = 0; i < InventoryWeapons.Length; i++)
        {
            if (_weapon != null && InventoryWeapons[i] != null)
            {
                if (_weapon.Equals(InventoryWeapons[i]))
                {
                    return i;
                }
            }
        }
        return -1;
    }
}
