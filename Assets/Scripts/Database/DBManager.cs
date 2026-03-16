using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DBManager
{
    public static string firstname;
    public static string lastname;
    public static string username;
    public static int citynumber;
    public static int score;
    public static bool LoggedIn { get {return username != null;}}
    public static void LogOut()
    {
        username = null;
        firstname = null;
        lastname = null;
        citynumber = 0;
        score = 0;
    }
}
