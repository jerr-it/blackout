using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapButton : MonoBehaviour {
    [SerializeField] private Image previewImage;
    [SerializeField] private TMPro.TextMeshProUGUI nameTag;
    [SerializeField] private Button button;
    
    public void Apply(MapDef def) {
        previewImage.sprite = def.preview;
        nameTag.text = def.mapName;
        button.onClick.AddListener(() => SceneManager.LoadScene(def.mapScene));
    }
}
