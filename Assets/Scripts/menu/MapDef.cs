using UnityEngine;

public enum MapType {
    Arena,
    CTF,
    Payload
}

[CreateAssetMenu(menuName = "Map/Map Definition", fileName = "map")]
public class MapDef : ScriptableObject {
    public Sprite preview;
    public string mapName;
    public string mapScene;
    public MapType mapType;
}
