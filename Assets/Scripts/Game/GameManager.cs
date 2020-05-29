using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, Weapon> items = new Dictionary<int, Weapon>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject[] decalPrefabs;
    public GameObject[] weaponPrefabs;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }

        PopulateDictionaryWithWeapons();
    }

    /// <summary>Spawns a player.</summary>
    /// <param name="_id">The player's ID.</param>
    /// <param name="_name">The player's name.</param>
    /// <param name="_position">The player's starting position.</param>
    /// <param name="_rotation">The player's starting rotation.</param>
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    /// <summary>Apply a decal in a given position.</summary>
    /// <param name="_position">The decal's location.</param>
    /// <param name="_rotation">The decal's rotation.</param>
    public void ApplyDecal(Vector3 _position, Quaternion _rotation)
    {
        Instantiate(decalPrefabs[0], _position, _rotation);
    }

    /// <summary>Spawns a weapon in the specified location.</summary>
    /// <param name="_weaponId">Network's item's id.</param>
    /// <param name="_position">The decal's location.</param>
    /// <param name="_weaponDataId">The decal's rotation.</param>
    public void SpawnWeapon(int _weaponId, Vector3 _position, int _weaponDataId)
    {
        
    }

    private void PopulateDictionaryWithWeapons()
    {

        foreach (GameObject weapon in weaponPrefabs)
        {
            Weapon.weapons.Add((int)weapon.GetComponent<Weapon>().weaponName, weapon);
        }
    }

    public void CreateItem(int _weaponId, int _itemId, Vector3 _position, int _heldBy, int _bullets, int _ammo)
    {
        GameObject prefab = GetWeapon(_weaponId);
        GameObject _item = Instantiate(prefab, _position, prefab.transform.rotation);
        _item.GetComponent<Weapon>().Initialize(_itemId);
        items.Add(_itemId, _item.GetComponent<Weapon>());

        if(_heldBy != -1)
        {
            players[_heldBy].GetComponent<WeaponManager>().PickupWeapon(_item, players[_heldBy].weaponHolder);
        }
    }

    private GameObject GetWeapon(int _weaponId)
    {
        return Weapon.weapons[_weaponId];
    }
}