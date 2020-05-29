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

    public void WeaponDropped()
    {
        modelCollider.enabled = true;
    }

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

    public void StopFiring()
    {
        isFiring = false;
    }

    IEnumerator PrepareNextShot()
    {
        preparingNextShot = true;
        yield return new WaitForSeconds(shotsPerSecond);
        preparingNextShot = false;
        isNextShotReady = true;
    }

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

    public void SemiShot(Vector3 _shootDirection)
    {
        ClientSend.PlayerShoot(_shootDirection);
        isNextShotReady = false;

        if (!preparingNextShot)
        {
            StartCoroutine(PrepareNextShot());
        }
    }

    public void Shot()
    {
        if (shootParticles && shootSound)
        {
            shootParticles.Play();
            shootSound.Play();
        }
    }

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
