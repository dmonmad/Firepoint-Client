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

    public void Initialize(string _name, string _ip, string _ping)
    {
        servername.SetText(_name);
        ip.SetText(_ip);
        ping.SetText(_ping);
    }
}
