using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

static class MaximiseOnPlay
{
    #if UNITY_EDITOR
    [MenuItem("Window/Maximize Current Window _F11")]
    static void ToggleCurrentWindowMaximized()
    {
        var window = EditorWindow.focusedWindow;
        if (window == null)
            return; 
        window.maximized = !window.maximized;
    }
    #endif
}
