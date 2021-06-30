using System;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using UnityEditor.iOS.Xcode;

[CreateAssetMenu(fileName = "IOSPlistScriptable")]
public class iOSPlistScriptable : ScriptableObject
{
    [SerializeField] public string[] plist_items_to_remove = new[] { "UIApplicationExitsOnSuspend" };
    [SerializeField] public PlistDictionary plist_items_to_add = new PlistDictionary { { "ITSAppUsesNonExemptEncryption", "NO" } };

}

[Serializable]
public class PlistDictionary : Dictionary<string, string> { }
