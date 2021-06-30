#if UNITY_EDITOR && UNITY_IOS

using System;

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using UnityEditor.iOS.Xcode;

[CreateAssetMenu(fileName = "IOSPlistScriptable")]
public class iOSPlistScriptable : ScriptableObject
{
    [SerializeField] public string[] plist_items_to_remove = new[] {"UIApplicationExitsOnSuspend"};
    [SerializeField] public List<PlistEntry> plist_items_to_add = new List<PlistEntry>() {new PlistEntry("ITSAppUsesNonExemptEncryption", "NO")};

}

[Serializable]
public class PlistEntry
{
    [SerializeField] string _key;
    [SerializeField] string _value;

    public string Key => _key;
    public string Value => _value;

    public PlistEntry() { }

    public PlistEntry(string key, string value)
    {
        _key = key;
        _value = value;
    }
}

#endif
