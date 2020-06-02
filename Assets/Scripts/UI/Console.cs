using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    private static Console instance;
    public GameObject console;
    public Text log;

    public void Start()
    {
        instance = this;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backslash))
        {
            console.SetActive(!console.activeInHierarchy);
        }
    }

    private Console() { }

    public static Console GetInstance()
    {
        if (instance == null)
            instance = new Console();
        return instance;        
    }

    /// <summary>Adds the line to the text log.</summary>
    /// <param name="line">Line that should be added to the log.</param>
    public void Log(string line)
    {
        Debug.Log(line);
        if (log)
        {
            log.text += line+"\n";
        }
    }

    /// <summary>Disables the console's object.</summary>
    public void CloseConsole()
    {
        if (console.activeInHierarchy)
        {
            console.SetActive(false);
        }
    }
}
