using Doozy.Engine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NavigationController 
{
    private static IList<UIView> openWindows = new List<UIView>();

    public static bool HasOpenWindow { get; private set; }

    public static void AddWindow(UIView view)
    {
        openWindows.Add(view);
        HasOpenWindow = true;
        Debug.Log("NAV COUNT " + openWindows.Count);
    }

    public static void RemoveWindow(UIView view)
    {
        openWindows.Remove(view);
        HasOpenWindow = openWindows.Count > 0;
        Debug.Log("NAV COUNT " + openWindows.Count);
    }
}
