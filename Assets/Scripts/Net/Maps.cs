using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Map
{
    public string name;
    public string scene;
    public Sprite image;
    public List<string> availableMode;
    public Vector3[] spanwPoint;
}

public class Maps : MonoBehaviour
{
    [SerializeField] public Map[] maps;

    public Map GetMap(string name)
    {
        foreach (Map m in maps) if (m.name == name) return m;
        return new Map();
    }

    public List<Map> GetMaps(string mode)
    {
        List<Map> maps = new List<Map>();
        foreach (Map m in this.maps) if (m.availableMode.Contains(mode)) maps.Add(m);
        return maps;
    }
}
