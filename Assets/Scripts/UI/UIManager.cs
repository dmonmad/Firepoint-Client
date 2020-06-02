using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject mainMenu;
    public InputField usernameField;
    public InputField ipPortField;
    public Button connectButton;
    public GameObject directConnection;
    public GameObject serverBrowser;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SwitchMainMenu();
        }
    }

    /// <summary>Locks or unlocks the weapon.</summary>
    /// <param name="lockMouse">Should the mouse be locked.</param>
    private void ToggleCursorMode(bool lockMouse)
    {
        if (!Client.instance.isConnected)
        {
            return;
        }

        Cursor.visible = lockMouse;

        if (lockMouse)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>Attempts to connect to the server.</summary>
    public void ConnectToServer()
    {
        string[] ipport = GetIpAndPort();

        if (ipport != null && ipport.Length == 2)
        {
            connectButton.interactable = false;
            Client.instance.ConnectToServer(ipport);
        }
    }

    /// <summary>Proccesses the text to get the ip and the port then tries to connect to the server.</summary>
    /// <param name="ipwithport">Line that should be added to the log.</param>
    public void ConnectToServer(string ipwithport)
    {
        Console.GetInstance().Log("Connecting to server");
        string[] ipport = GetIpAndPort(ipwithport);

        if (ipport != null && ipport.Length == 2)
        {
            connectButton.interactable = false;
            Client.instance.ConnectToServer(ipport);
        }
        else
        {
            Console.GetInstance().Log("Invalid IP");
        }
    }

    /// <summary>Client disconnected from the server.</summary>
    public void DisconnectedFromServer()
    {
        mainMenu.SetActive(true);
        connectButton.interactable = true;
        usernameField.interactable = true;
    }

    /// <summary>Client has connected to a server.</summary>
    public void ConnectedToServer()
    {
        SwitchMainMenu();
        connectButton.interactable = false;
        usernameField.interactable = false;
    }

    /// <summary>Proccesses the text to get the ip and the port(from Direct Connection).</summary>
    public string[] GetIpAndPort()
    {
        string text = ipPortField.text;

        string[] ipport = new string[2];

        int colonIndex = text.IndexOf(':');
        if (colonIndex == -1)
        {
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

    /// <summary>Proccesses the text to get the ip and the port(from Server Browser).</summary>
    /// <param name="text">Line that should be added to the log.</param>
    public string[] GetIpAndPort(string text)
    {
        string[] ipport = new string[2];

        int colonIndex = text.IndexOf(':');

        if (colonIndex == -1)
        {
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
            catch (Exception)
            {
                return null;
            }
            return ipport;
        }
    }

    /// <summary>Enables and disables the Server Browser's object.</summary>
    public void SwitchServerBrowser()
    {
        if (serverBrowser.activeInHierarchy)
        {
            serverBrowser.SetActive(false);
            ToggleCursorMode(false);
        }
        else
        {
            serverBrowser.SetActive(true);
            if (!mainMenu.activeInHierarchy)
            {
                SwitchMainMenu();
            }
            ToggleCursorMode(true);
        }
    }

    /// <summary>Enables and disables the Direct Connection's object.</summary>
    public void SwitchDirectConnection()
    {
        if (directConnection.activeInHierarchy)
        {
            directConnection.SetActive(false);
            ToggleCursorMode(false);
        }
        else
        {
            directConnection.SetActive(true);
            if (!mainMenu.activeInHierarchy)
            {
                SwitchMainMenu();
            }
            ToggleCursorMode(true);
        }
    }

    /// <summary>Enables and disables the Main Menu's object.</summary>
    public void SwitchMainMenu()
    {
        if (Client.instance.isConnected)
        {
            if (mainMenu.activeInHierarchy)
            {
                mainMenu.SetActive(false);
                ToggleCursorMode(false);
            }
            else
            {
                mainMenu.SetActive(true);
                ToggleCursorMode(true);
            }
        }
    }

    /// <summary>Check if the main menu is open.</summary>
    /// <returns>Returns true if MainMenu is open</returns>
    public bool IsUIOpen()
    {
        if (mainMenu.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    /// <summary>Closes the game.</summary>
    public void Exit()
    {
            Application.Quit(0);
    }
}