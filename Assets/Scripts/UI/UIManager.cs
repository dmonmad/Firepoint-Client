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
            Console.GetInstance().Log("IP no valida");
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
        Debug.Log("CONNECTEDTOSERVERCALLED");
        SwitchMainMenu();
        connectButton.interactable = false;
        usernameField.interactable = false;
    }

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

    public string[] GetIpAndPort(string text)
    {
        Console.GetInstance().Log(text);
        Console.GetInstance().Log("GetIpAndPort(text)");
        string[] ipport = new string[2];

        Console.GetInstance().Log("1");
        int colonIndex = text.IndexOf(':');

        Console.GetInstance().Log("2");
        if (colonIndex == -1)
        {
            Console.GetInstance().Log("3");
            return null;
        }
        else
        {
            Console.GetInstance().Log("4");
            ipport[0] = text.Substring(0, colonIndex);
            ipport[1] = text.Substring(colonIndex + 1);

            Console.GetInstance().Log("5");
            try
            {
                Console.GetInstance().Log("6");
                int.Parse(ipport[1]);
                Console.GetInstance().Log("7");
            }
            catch (Exception)
            {
                Console.GetInstance().Log("8");
                return null;
            }
            Console.GetInstance().Log("Returning " + ipport);
            return ipport;
        }
    }

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

    public bool IsUIOpen()
    {
        if (mainMenu.activeInHierarchy)
        {
            return true;
        }
        return false;
    }

    public void Exit()
    {
            Application.Quit(0);
    }
}