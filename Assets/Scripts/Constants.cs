using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    public static Color WEAPON_SPRITE_UNSELECTED = new Color(0, 0, 0, 0.4f);
    public static Color WEAPON_SPRITE_SELECTED = new Color(255, 255, 255, 1f);
    public static Color WEAPON_SPRITE_CLEAR = Color.clear;

    public const string MASTERSERVER_URL = "https://firepointmasterserver.herokuapp.com/server";

}
