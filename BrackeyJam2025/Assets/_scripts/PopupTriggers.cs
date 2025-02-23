using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTriggers : MonoBehaviour
{
    public GameObject[] popups;
    public float popupDuration = 3f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerMovement player))
        {
            StartCoroutine(PopupCoroutine());
        }
    }

    private IEnumerator PopupCoroutine()
    {
        if (popups.Length == 0)
        {
            Debug.LogError("Popups list is empty. Cannot trigger them");
            yield break;
        }

        int ind = 0;

        do
        {
            popups[ind++].SetActive(true);
            yield return new WaitForSeconds(popupDuration);
        }
        while (ind < popups.Length);
    }
}
