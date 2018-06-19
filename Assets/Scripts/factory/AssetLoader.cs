using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.ComponentModel;

/*
  Generic class to load assets of enum T from a specific folder.
*/
public class AssetLoader <T> where T: struct, IConvertible, IComparable, IFormattable {

    // In this version of C#, System.Enum cannot be use as a constraint
    // so we'll check that T is an Enum here in a static constructor
    static AssetLoader() {
        if (!typeof(T).IsEnum) {
        throw new ArgumentException("T must be an enumerated type");
        }
    }

    // Contains the prefabs
    private Dictionary<T, GameObject> prefabs;

    // Folder in the resources folder that contains our prefabs
    private string prefabFolder;

    public AssetLoader(string folderName) {
        prefabFolder = folderName;
        prefabs = new Dictionary<T, GameObject>();
    }

    public void LoadAssets() {
        foreach (T assetType in Enum.GetValues(typeof(T))) {
            string assetTypeString = GetName(assetType);

            Debug.Log("# Loading " + assetTypeString);
            GameObject prefab = Resources.Load(prefabFolder + "/" + assetTypeString) as GameObject;

            if (prefab != null) {
                prefabs.Add(assetType, prefab);
            } else {
                Debug.LogError("No prefab for " + assetTypeString);
            }
        }
    }

    /*
      Will return the loaded gameobject for the given enum value
     */
    public GameObject Get(T assetType) {
        return prefabs[assetType];
    }

    private String GetName(T assetType) {
        FieldInfo fi = assetType.GetType().GetField(assetType.ToString());
        DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        if (attributes.Length > 0)
        {
            return attributes[0].Description;
        }
        else
        {
            return assetType.ToString();
        }
    }
}
