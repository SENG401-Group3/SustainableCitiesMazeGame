using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string firstname;
    public static string lastname;
    public static string username;
    public static int score;
    public static bool LoggedIn { get {return username != null;}}
    public static void LogOut()
    {
        username = null;
    }
}
