using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum WeaponID
    {
        Cuchillo = 1,
        Glock,
        M3,
        AK47,
        M4,
        L96
    }

    public enum WeaponTier
    {
        Primary,
        Secondary,
        Cuchillo,
        Equipment
    }

    public enum WeaponShootType
    {
        Semi,
        Automatic
    }

    public static Dictionary<int, GameObject> weapons = new Dictionary<int, GameObject>();

    public WeaponID weaponName = WeaponID.Cuchillo;
    public WeaponTier weaponTier = WeaponTier.Cuchillo;
    public WeaponShootType weaponShootType = WeaponShootType.Automatic;
    private int itemId;
    public string itemName;
    public MeshRenderer model;
    public MeshCollider modelCollider;
    public ParticleSystem shootParticles;
    public Sprite hudSprite;

    #region Weapon Sound

    public AudioSource shootSound;

    #endregion

    #region Weapon Variables

    public Transform _shootOrigin;
    public bool isReloading;
    public bool preparingNextShot;
    public bool isFiring;
    public bool isNextShotReady;
    public float reloadTime = 0;
    public int maxAmmo = 0;
    public int ammo = 0;
    public int maxClip = 0;
    public int clip = 0;
    public float fireRate = 0;
    public float shotsPerSecond = 0;

    #endregion

    /// <summary>Initializes the weapon with the given ID.</summary>
    /// <param name="_itemId">The image which sprite's color must be changed.</param>
    public void Initialize(int _itemId)
    {
        model = GetComponentInChildren<MeshRenderer>();
        modelCollider = GetComponentInChildren<MeshCollider>();
        itemId = _itemId;
        ammo = maxAmmo;
        clip = maxClip;
        isReloading = false;
        isNextShotReady = true;
        preparingNextShot = false;
        isFiring = false;
        shotsPerSecond = 1f / fireRate;
    }

    private void Update()
    {
        if (isFiring)
        {
            Shoot(_shootOrigin);
        }
    }

    /// <summary>Enables the weapon's collider.</summary>
    public void WeaponDropped()
    {
        modelCollider.enabled = true;
    }

    /// <summary>Takes a shot </summary>
    /// <param name="_newShootOrigin">The origin from where the shoots should be casted.</param>
    /// <returns>Returns true if the shot was made, false if else</returns>
    public bool Shoot(Transform _newShootOrigin)
    {
        if (!_shootOrigin)
        {
            _shootOrigin = _newShootOrigin;
        }

        if (isNextShotReady)
        {
            if (clip > 0)
            {
                switch (weaponShootType)
                {
                    case WeaponShootType.Semi:
                        SemiShot(_shootOrigin.forward);
                        Shot();
                        return true;

                    case WeaponShootType.Automatic:
                        AutomaticShot(_shootOrigin.forward);
                        Shot();
                        return true;
                }
            }
            else
            {
                //StartCoroutine(Reload());
                return true;
            }
        }
        else
        {
            if (!preparingNextShot)
            {
                StartCoroutine(PrepareNextShot());
            }
        }

        return true;

    }

    /// <summary>Stops firing if Automatic Mode.</summary>
    public void StopFiring()
    {
        isFiring = false;
    }

    /// <summary>Prepares the next shot in the time specified.</summary>
    IEnumerator PrepareNextShot()
    {
        preparingNextShot = true;
        yield return new WaitForSeconds(shotsPerSecond);
        preparingNextShot = false;
        isNextShotReady = true;
    }

    /// <summary>Takes a shot in automatic mode.</summary>
    /// <param name="_shootDirection">The vector that the shot will travel.</param>
    public void AutomaticShot(Vector3 _shootDirection)
    {
        ClientSend.PlayerShoot(_shootDirection);
        isFiring = true;
        isNextShotReady = false;

        if (!preparingNextShot)
        {
            StartCoroutine(PrepareNextShot());
        }
    }

    /// <summary>Takes a shot in semi-automatic mode.</summary>
    /// <param name="_shootDirection">The vector that the shot will travel.</param>

    public void SemiShot(Vector3 _shootDirection)
    {
        ClientSend.PlayerShoot(_shootDirection);
        isNextShotReady = false;

        if (!preparingNextShot)
        {
            StartCoroutine(PrepareNextShot());
        }
    }

    /// <summary>Simulates a shot from the weapon.</summary>
    public void Shot()
    {
        if (shootParticles && shootSound)
        {
            shootParticles.Play();
            shootSound.Play();
        }
    }

    /// <summary>Update bullets from the weapon.</summary>
    /// <param name="actualClip">The new weapon's clip.</param>
    /// <param name="actualAmmo">The new weapon's ammo.</param>
    public void UpdateBullets(int actualClip, int actualAmmo)
    {
        clip = actualClip;
        ammo = actualAmmo;
    }

    private void OnEnable()
    {
        if (preparingNextShot)
        {
            StartCoroutine(PrepareNextShot());
        }
    }
}
