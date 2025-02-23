using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class QuitAppButton : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(QuitApp);
    }

    private void QuitApp()
    {
        Application.Quit();
        button.interactable = false;
    }
}
