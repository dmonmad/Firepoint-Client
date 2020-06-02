using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    [Header("Player")]
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public MeshRenderer model;
    public TextMesh nameMesh;

    [Header("Transforms")]
    public Transform weaponHolder;
    public Transform weaponDropper;

    [Header("Health")]
    public RectTransform healthBar;
    public TextMeshProUGUI healthText;

    private Dictionary<MeshRenderer, bool> objectsSavedState = new Dictionary<MeshRenderer, bool>();

    /// <summary>Initializes the player with the given id and username
    /// <param name="_id">The player's username.</param>
    /// <param name="_username">The player's username.</param>
    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
        if (nameMesh)
        {
            nameMesh.text = username;
        }
    }

    /// <summary>Sets the player's health and, if under 0 after setting it, kills the player with a message
    /// <param name="_health">The player's new health.</param>
    /// <param name="_attackerId">The attacker's id.</param>
    public void SetHealth(float _health, int _attackerId)
    {
        health = _health;

        if (healthBar && healthText)
        {
            healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
            healthText.SetText(health.ToString());
        }

        if (health <= 0f)
        {
            Console.GetInstance().Log($"{GameManager.players[_attackerId].username} killed { username}");
            Die();
        }
    }

    /// <summary>Kills the palyer, setting health to 0 and disabling mesh renderers
    public void Die()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            MeshRenderer _model = gameObject.transform.GetChild(i).GetComponent<MeshRenderer>();
            if (_model)
            {
                objectsSavedState.Add(_model, _model.enabled);
                _model.enabled = false;
            }
        }
        model.enabled = false;
    }

    /// <summary>Respawns the player, setting full health and enabling mesh renderers
    public void Respawn()
    {
        foreach(MeshRenderer _model in objectsSavedState.Keys)
        {
            _model.enabled = objectsSavedState[_model];
        }

        objectsSavedState.Clear();

        SetHealth(maxHealth, -1);

        Console.GetInstance().Log($"{username} has respawned!");
    }
}
