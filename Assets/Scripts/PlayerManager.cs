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
    public GameObject weaponEquipped;

    [Header("Transforms")]
    public Transform weaponHolder;
    public Transform weaponDropper;

    [Header("Health")]
    public RectTransform healthBar;
    public TextMeshProUGUI healthText;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (healthBar && healthText)
        {
            healthBar.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, health);
            healthText.SetText(health.ToString());
        }

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
