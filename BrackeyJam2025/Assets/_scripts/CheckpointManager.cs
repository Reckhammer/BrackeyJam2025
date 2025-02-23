using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager instance;
    public float respawnDelay = 2f;

    private Checkpoint currentRespawnPoint;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //PlayerManager.instance.playerHealth.PlayerDied += StartRespawnCoroutine;
    }

    private void OnDestroy()
    {
        instance = null;
    }

    public void SetCheckpoint(Checkpoint checkpoint)
    {
        currentRespawnPoint = checkpoint;
    }

    public void RespawnPlayer()
    {
        PlayerManager.instance.playerMovement.transform.SetPositionAndRotation(currentRespawnPoint.transform.position, currentRespawnPoint.transform.rotation);
        PlayerManager.instance.playerMovement.RB.velocity = Vector2.zero;

        PlayerManager.instance.playerHealth.PlayerRevive();
    }

    private void StartRespawnCoroutine()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator RespawnCoroutine()
    {
        yield return new WaitForSeconds(respawnDelay);

        RespawnPlayer();
    }
}
