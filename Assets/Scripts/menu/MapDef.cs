using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Map/Map Definition", fileName = "map")]
public class MapDef : ScriptableObject {
    public Sprite preview;
    public string mapName;
    public string mapScene;
}
