using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
    [SerializeField] private Image previewImage;
    [SerializeField] private TMPro.TextMeshProUGUI nameTag;
    [SerializeField] private Button button;
    [SerializeField] private TMPro.TextMeshProUGUI typeTag;
    
    public void Apply(MapDef def) {
        previewImage.sprite = def.preview;
        nameTag.text = def.mapName;
        typeTag.text = def.mapType.ToString();
        button.onClick.AddListener(() => SceneManager.LoadScene(def.mapScene));
    }
}
