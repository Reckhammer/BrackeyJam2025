using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public RewindGauge rewindGauge;
    public GameObject rewindUI;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowRewindUI()
    {
        rewindUI.SetActive(true);
    }

    public void HideRewindUI()
    {
        rewindUI.SetActive(false);
    }
}
