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
    public int selectedIndex;
    public int SelectedWeapon = -1;
    public int lastSelectedIndex = -1;
    public float dropForce = 30f;

    public void Start()
    {
        selectedIndex = -1;
    }

    public void Update()
    {
        UpdateBullets();
    }

    public void SelectWeapon(int _index)
    {
        if (InventoryWeapons[_index] != null)
        {
            if (_index != selectedIndex)
            {
                ClientSend.ChangeWeapon(_index);
                DeselectWeaponImage(InventoryHudSlots[selectedIndex]);
                InventoryWeapons[selectedIndex].SetActive(false);
                selectedIndex = _index;
                SelectWeaponImage(InventoryHudSlots[selectedIndex]);
                InventoryWeapons[selectedIndex].SetActive(true);
            }
        }
    }

    public void ChangeWeapon(int _index)
    {
        if (InventoryWeapons[_index] != null)
        {
            if (_index != selectedIndex)
            {
                InventoryWeapons[selectedIndex].SetActive(false);
                selectedIndex = _index;
                InventoryWeapons[selectedIndex].SetActive(true);
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

                if (selectedIndex != -1)
                {
                    _itemObj.SetActive(false);
                    DeselectWeaponImage(InventoryHudSlots[i]);
                }
                else
                {
                    selectedIndex = i;
                    SelectWeaponImage(InventoryHudSlots[i]);
                    Debug.Log("Setting image");
                }

                Rigidbody _rb = _itemObj.GetComponent<Rigidbody>();
                if (_rb)
                {
                    Debug.Log("DESACTIVANDO RB FROM " + _rb.name);
                    _rb.isKinematic = true;
                    break;
                }
            }
        }


    }

    public void SelectLastSelectedWeapon()
    {
        if (lastSelectedIndex != -1)
        {
            if (InventoryWeapons[lastSelectedIndex] != null)
            {
                SelectWeapon(lastSelectedIndex);
            }
            else
            {
                lastSelectedIndex = -1;
            }
        }
    }

    public void DropWeapon(Vector3 _dropVector, Transform _weaponDropper)
    {
        if (InventoryWeapons[selectedIndex] != null)
        {
            Debug.Log("Has selected weapon");
            if (InventoryWeapons[selectedIndex].activeInHierarchy)
            {
                Debug.Log("Getting rb");
                Rigidbody _rb = InventoryWeapons[selectedIndex].GetComponent<Rigidbody>();
                if (_rb)
                {
                    Debug.Log("Throwing weapon");
                    InventoryWeapons[selectedIndex].transform.parent = null;
                    InventoryWeapons[selectedIndex].transform.position = _weaponDropper.position;
                    InventoryWeapons[selectedIndex] = null;
                    ClearWeaponImage(InventoryHudSlots[selectedIndex]);
                    selectedIndex = -1;
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
            if (InventoryWeapons[selectedIndex].GetComponent<Weapon>().Shoot(_camTransform))
            {
                Debug.Log("WeaponManager -> Shoot = true");
            }
        }
    }

    public void StopFiringWeapon()
    {
        if (IsWeaponSelected())
        {
            InventoryWeapons[selectedIndex].GetComponent<Weapon>().StopFiring();
        }
    }

    public bool IsWeaponSelected()
    {
        if (selectedIndex != -1)
        {
            if (InventoryWeapons[selectedIndex])
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public void PickupWeaponImage(Image _image, Sprite _sprite)
    {
        if (_image)
        {
            _image.sprite = _sprite;
            _image.color = Constants.WeaponSpriteSelected;
        }
    }

    public void SelectWeaponImage(Image _image)
    {
        if (_image)
        {
            _image.color = Constants.WeaponSpriteSelected;
        }
    }

    public void ClearWeaponImage(Image _image)
    {
        if (_image)
        {
            _image.sprite = null;
            _image.color = Constants.WeaponSpriteClear;
        }
    }

    public void DeselectWeaponImage(Image _image)
    {
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
                bulletsText.SetText(FormattedBullets(InventoryWeapons[selectedIndex].GetComponent<Weapon>()));
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
            if (InventoryWeapons[i])
            {
                selectedIndex = i;
                DropWeapon(gameObject.transform.forward, _weaponDropper);
            }
        }
    }

    public string FormattedBullets(Weapon _weapon)
    {
        return _weapon.clip + " / " + _weapon.ammo;
    }
}
