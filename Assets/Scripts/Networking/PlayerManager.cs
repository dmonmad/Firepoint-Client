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
            Console.GetInstance().Log($"El jugador "+GameManager.players[_attackerId].username+" ha asesinado a "+username);
            Die();
        }
    }

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

    public void Respawn()
    {
        foreach(MeshRenderer _model in objectsSavedState.Keys)
        {
            _model.enabled = objectsSavedState[_model];
        }

        objectsSavedState.Clear();

        SetHealth(maxHealth, -1);
    }
}
