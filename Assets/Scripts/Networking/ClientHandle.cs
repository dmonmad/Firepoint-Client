using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{

    public static int lastUpdateWeaponBulletsPacketId = 0;

    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Console.GetInstance().Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        if (GameManager.players.ContainsKey(_id))
        {
            GameManager.players[_id].transform.position = _position;
        }
        //GameManager.players[_id].GetComponent<PlayerController>().ShiftPositions(_position);
        //GameManager.players[_id].transform.position = Vector3.Lerp(GameManager.players[_id].transform.position, _position, Time.deltaTime * InterpolationMoveFactor);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        WeaponManager wm = GameManager.players[_id].GetComponent<WeaponManager>();
        PlayerManager player = GameManager.players[_id].GetComponent<PlayerManager>();
        if (wm && player)
        {
            wm.DropAllWeapons(player.transform.forward, player.weaponDropper);
        }

        Console.GetInstance().Log($"El jugador "+player.username+" se ha desconectado");
        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _attackerId = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health, _attackerId);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void ApplyDecal(Packet _packet)
    {
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.ApplyDecal(_position, _rotation);
    }

    public static void CreateItemSpawner(Packet _packet)
    {

        int _weaponId = _packet.ReadInt();
        int _itemId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _heldBy = _packet.ReadInt();
        int _bullets = _packet.ReadInt();
        int _ammo = _packet.ReadInt();

        GameManager.instance.CreateItem(_weaponId, _itemId, _position, _heldBy, _bullets, _ammo);
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _itemId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();
        
        WeaponManager wm = GameManager.players[_byPlayer].GetComponent<WeaponManager>();

        if (wm)
        {
            wm.PickupWeapon(GameManager.items[_itemId].gameObject, GameManager.players[_byPlayer].weaponHolder);
        }

    }

    public static void PlayerThrowWeapon(Packet _packet)
    {
        Vector3 _dropVector = _packet.ReadVector3();
        int _byPlayer = _packet.ReadInt();

        WeaponManager wm = GameManager.players[_byPlayer].GetComponent<WeaponManager>();
        PlayerManager player = GameManager.players[_byPlayer].GetComponent<PlayerManager>();

        if (wm && player)
        {
            wm.DropWeapon(_dropVector, player.weaponDropper);
        }
    }

    public static void UpdateWeaponBullets(Packet _packet)
    {
        int _packetId = _packet.ReadInt();
        int _itemId = _packet.ReadInt();
        int _actualClip = _packet.ReadInt();
        int _actualAmmo = _packet.ReadInt();

        if(_packetId > lastUpdateWeaponBulletsPacketId)
        {
            lastUpdateWeaponBulletsPacketId = _packetId;
            GameManager.items[_itemId].UpdateBullets(_actualClip, _actualAmmo);
        }

    }

    public static void ChangeWeaponFromPlayer(Packet _packet)
    {
        int _fromPlayer = _packet.ReadInt();
        int _index = _packet.ReadInt();

        WeaponManager wm = GameManager.players[_fromPlayer].GetComponent<WeaponManager>();
        if (wm)
        {
            wm.ChangeWeapon(_index);
        }
    }
    
    public static void PlayerShot(Packet _packet)
    {
        int _fromPlayer = _packet.ReadInt();

        WeaponManager wm = GameManager.players[_fromPlayer].GetComponent<WeaponManager>();
        if (wm)
        {
            wm.Shot();
        }
    }
}