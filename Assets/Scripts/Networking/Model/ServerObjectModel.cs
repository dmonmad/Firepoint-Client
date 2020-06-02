using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ServerObjectModel : MonoBehaviour
{
    public TextMeshProUGUI servername;
    public TextMeshProUGUI ip;
    public TextMeshProUGUI ping;

    /// <summary>Initalizes the object with values.</summary>
    /// <param name="_name">Server's name.</param>
    /// <param name="_ip">Server's ip.</param>
    /// <param name="_ping">Server's ping.</param>
    public void Initialize(string _name, string _ip, string _ping)
    {
        servername.SetText(_name);
        ip.SetText(_ip);
        ping.SetText(_ping);
    }
}
