using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MatchManager : MonoBehaviour {
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Sprite> playerSkins;

    public void OnPlayerJoined(PlayerInput playerInput) {
        playerInput.transform.position = spawnPoints[playerInput.playerIndex].position;
        playerInput.gameObject.GetComponent<PlayerController>().SetPlayerSkin(playerSkins[playerInput.playerIndex]);
    }
}
