using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

/// <summary>
/// Manages arena type matches
/// TODO Call OnPlayerDeath on collision
/// </summary>
public class ArenaMatchManager : MonoBehaviour {
    public static ArenaMatchManager Instance;

    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private List<Sprite> playerSkins;

    [SerializeField] private int respawnTimer;
    [SerializeField] private int targetKills;
    [SerializeField] private int invulnTimer;

    private List<PlayerController> players;

    public void Start() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }
    }
    
    public void OnPlayerJoined(PlayerInput playerInput) {
        playerInput.transform.position = spawnPoints[playerInput.playerIndex].position;
        playerInput.gameObject.GetComponent<PlayerController>().SetPlayerSkin(playerSkins[playerInput.playerIndex]);
        players.Add(playerInput.gameObject.GetComponent<PlayerController>());
    }

    public void OnPlayerDeath(PlayerController player, PlayerController killer) {
        if (IsWinner(killer)) {
            // TODO ugly, redo with ui
            Debug.Log("Player " + player.GetComponent<PlayerInput>().playerIndex + " wins");
            // Replace with async, loading indicator
            SceneManager.LoadScene("Scenes/main_menu");
        }
        
        killer.AddKill();
        
        player.EnableSpectator();
        RespawnAfter(respawnTimer, player);
        
        // TODO player invulnerable for x seconds
    }

    private void RespawnAfter(int seconds, PlayerController player) {
        StartCoroutine(WaitThen(seconds, () => {
            player.transform.position = spawnPoints[Random.Range(0, spawnPoints.Count)].position;
            player.DisableSpectator();
        }));
    }

    private IEnumerator WaitThen(int seconds, Action then) {
        yield return new WaitForSeconds(seconds);
        then();
    }

    private bool IsWinner(PlayerController player) {
        return player.GetKillCount() >= targetKills;
    }
}
