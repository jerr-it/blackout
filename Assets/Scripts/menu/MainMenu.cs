using System;
using UnityEngine;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {
    [SerializeField] private List<MapDef> maps;
    
    #region UI
    [SerializeField] private GameObject mapSelection;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mapScrollViewContent;
    [SerializeField] private GameObject mapButtonPrefab;
    #endregion

    public void Start() {
        foreach (MapDef mapDef in maps) {
            GameObject mapButton = Instantiate(mapButtonPrefab, mapScrollViewContent.transform);
            mapButton.GetComponent<MapButton>().Apply(mapDef);
        }
    }

    public void OnPlayClicked() {
        mainMenu.SetActive(false);
        mapSelection.SetActive(true);
    }

    public void OnReturnToMainMenu() {
        mainMenu.SetActive(true);
        mapSelection.SetActive(false);
    }

    public void OnCreditsClicked() {

    }

    public void OnQuitClicked() {
        Application.Quit();
    }
}
