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

    public string WeaponGFXLayer = "WeaponGFX";
    public string WeaponDefaultLayer = "Weapon";
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


    /// <summary>Changes the selected weapon to the new one(only used by local player).</summary>
    /// <param name="_index">The index of the weapon to change in the inventory.</param>
    public void SelectWeapon(int _index)
    {
        if (InventoryWeapons[_index] != null)
        {
            if (InventoryWeapons[_index] != selectedWeapon)
            {
                ClientSend.ChangeWeapon(_index);
                if (selectedWeapon)
                {
                    DeselectWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                    selectedWeapon.SetActive(false);
                }
                selectedWeapon = InventoryWeapons[_index];
                SelectWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                selectedWeapon.SetActive(true);
            }
        }
    }


    /// <summary>Changes the selected weapon to the new one(used by remote players).</summary>
    /// <param name="_index">The index of the weapon to change in the inventory.</param>
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

    /// <summary>Picks a weapon up, updates the inventory and the HUD, disables its rigidbody and 
    /// sets the layer so only the player sees it over the meshes</summary>
    /// <param name="_itemObj">The item that needs to be picked up.</param>
    /// <param name="_weaponHolder">The position where the weapon will be if picked up.</param>
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
                }

                if (gameObject.GetComponent<PlayerController>())
                {
                    Utils.SetLayerRecursively(WeaponGFXLayer, _itemObj);
                }
                //_itemObj.layer = LayerMask.NameToLayer(WeaponLayer);

                break;
            }
        }


    }

    /// <summary>Selects the last selected weapon if there's one.</summary>
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

    /// <summary>Drops the weapon if possible, clears the inventory and hud, sets the original layers again and makes it affected by gravity.</summary>
    /// <param name="_dropVector">The vector the weapon will follow if dropped.</param>
    /// <param name="_weaponDropper">The position from where the weapon will be dropped.</param>
    public void DropWeapon(Vector3 _dropVector, Transform _weaponDropper)
    {
        if (selectedWeapon != null)
        {
            if (selectedWeapon.activeInHierarchy)
            {
                Rigidbody _rb = selectedWeapon.GetComponent<Rigidbody>();
                if (_rb)
                {
                    ClearWeaponImage(InventoryHudSlots[GetWeaponIndex(selectedWeapon)]);
                    InventoryWeapons[GetWeaponIndex(selectedWeapon)] = null;
                    selectedWeapon.transform.parent = null;
                    selectedWeapon.transform.position = _weaponDropper.position;

                    if (gameObject.GetComponent<PlayerController>())
                    {
                        Utils.SetLayerRecursively(WeaponDefaultLayer, selectedWeapon);
                    }

                    selectedWeapon = null;
                    _rb.isKinematic = false;
                    _rb.AddForce(gameObject.transform.forward * dropForce, ForceMode.Force);
                }
            }
        }
    }

    /// <summary>Fires the weapon if one is selected.</summary>
    /// <param name="_camTransform">The origin of the shoot.</param>
    public void FireWeapon(Transform _camTransform)
    {
        if (IsWeaponSelected())
        {
            selectedWeapon.GetComponent<Weapon>().Shoot(_camTransform);
        }
    }

    /// <summary>Stops firing the weapon.</summary>
    public void StopFiringWeapon()
    {
        if (IsWeaponSelected())
        {
            selectedWeapon.GetComponent<Weapon>().StopFiring();
        }
    }

    /// <summary>Changes the image's color to clear.</summary>
    /// <returns>Returns true if a weapon is selected, false if else</returns>
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

    /// <summary>Simulates a shot from the weapon.</summary>
    public void Shot()
    {
        if (selectedWeapon)
        {
            selectedWeapon.GetComponent<Weapon>().Shot();
        }
    }

    /// <summary>Changes the image's color to clear.</summary>
    /// <param name="_image">The image which sprite's color must be changed.</param>
    /// <param name="_sprite">The sprite to which the weapon sprite should be changed.</param>
    public void PickupWeaponImage(Image _image, Sprite _sprite)
    {
        if (_image)
        {
            _image.sprite = _sprite;
            _image.color = Constants.WEAPON_SPRITE_SELECTED;
        }
    }

    /// <summary>Changes the image's color to selected.</summary>
    /// <param name="_image">The image which sprite's color must be changed.</param>
    public void SelectWeaponImage(Image _image)
    {
        if (_image)
        {
            _image.color = Constants.WEAPON_SPRITE_SELECTED;
        }
    }

    /// <summary>Changes the image's color to clear.</summary>
    /// <param name="_image">The image which sprite's color must be changed.</param>
    public void ClearWeaponImage(Image _image)
    {
        if (_image)
        {
            _image.sprite = null;
            _image.color = Constants.WEAPON_SPRITE_CLEAR;
        }
    }
    
    /// <summary>Changes the image's color to unselected.</summary>
    /// <param name="_image">The image which sprite's color must be changed.</param>
    public void DeselectWeaponImage(Image _image)
    {
        if (_image)
        {
            _image.color = Constants.WEAPON_SPRITE_UNSELECTED;
        }
    }


    /// <summary>Updates the bullets in the hud.</summary>
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

    /// <summary>Drops all the weapons holded by the player.</summary>
    /// <param name="_dropVector">The vector the weapon will follow if dropped.</param>
    /// <param name="_weaponDropper">The position where the weapons will be initially dropped.</param>
    public void DropAllWeapons(Vector3 _dropVector, Transform _weaponDropper)
    {
        for (int i = 0; i < InventoryWeapons.Length; i++)
        {
            if (InventoryWeapons[i])
            {
                selectedWeapon = InventoryWeapons[i];
                selectedWeapon.SetActive(true);
                DropWeapon(gameObject.transform.forward, _weaponDropper);
            }
        }
    }

    /// <summary>Formats bullets and ammo left in a weapon.</summary>
    /// <param name="_weapon">The weapons from which the bullets will be returned.</param>
    /// <returns>Returns bullets in "A / B" format</returns>
    public string FormattedBullets(Weapon _weapon)
    {
        return _weapon.clip + " / " + _weapon.ammo;
    }

    /// <summary>Finds the index of a GameObject in the inventory.</summary>
    /// <param name="_weapon">The GameObject of the weapon from which the id must be found</param>
    /// <returns>Index of the weapon in the inventory or -1 if it doesn't exists in it</returns>
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
