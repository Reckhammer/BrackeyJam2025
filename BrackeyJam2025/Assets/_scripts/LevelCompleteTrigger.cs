using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteTrigger : MonoBehaviour
{
    public float levelChangeDelay = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
            LevelCompleted();
    }

    private void LevelCompleted()
    {
        StartCoroutine(LevelCompleteCoroutine());
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        PlayerManager.instance.playerMovement.EnablePlayerMovement(false);
        PlayerManager.instance.playerMovement.ClearPlayerInput();
        PlayerManager.instance.animator.SetTrigger("IsHappy");
        
        yield return new WaitForSeconds(levelChangeDelay);

        int sceneToLoad = SceneManager.GetActiveScene().buildIndex + 1;
        if (sceneToLoad >= SceneManager.sceneCountInBuildSettings)
            sceneToLoad = 0; // Main Menu

        SceneManager.LoadScene(sceneToLoad);
    }
}
