using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public InputField usernameField;
    public InputField ipPortField;

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
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        string[] ipport = GetIpAndPort();

        if (ipport != null)
        {
            startMenu.SetActive(false);
            usernameField.interactable = false;
            Client.instance.ConnectToServer(ipport);
        }


    }

    public string[] GetIpAndPort()
    {
        string text = ipPortField.text;

        string[] ipport = new string[2];

        int colonIndex = text.IndexOf(':');
        if (colonIndex == -1)
        {
            // Or whatever
            return null;
        }
        else
        {
            ipport[0] = text.Substring(0, colonIndex);
            ipport[1] = text.Substring(colonIndex + 1);

            try
            {
                int.Parse(ipport[1]);
            }
            catch (Exception ex)
            {
                return null;
            }
            return ipport;
        }
    }
}