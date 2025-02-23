using UnityEngine;
using UnityEngine.UI;

public class RewindGauge : MonoBehaviour
{
    public Image gaugeImage;
    public float maxRewindTime = 10f;
    public float currentRewindTime;
    public RewindObject playerRewind;

    private bool isRewinding;

    void Start()
    {
        currentRewindTime = maxRewindTime;
    }

    void Update()
    {
        if (playerRewind != null && playerRewind.isRewinding)
        {
            currentRewindTime -= Time.deltaTime;
            if (currentRewindTime <= 0)
            {
                currentRewindTime = 0;
                playerRewind.StopRewind();
            }
        }
        else
        {
            currentRewindTime = Mathf.Clamp(currentRewindTime, 0, maxRewindTime);
        }

        gaugeImage.fillAmount = currentRewindTime / maxRewindTime;
    }

    public void UseRewind(float amount)
    {
        currentRewindTime -= amount;
        if (currentRewindTime <= 0)
        {
            currentRewindTime = 0;
        }
    }

    public void RefundRewind(float amount)
    {
        currentRewindTime += amount;
        currentRewindTime = Mathf.Clamp(currentRewindTime, 0, maxRewindTime);
    }

    public bool CanRewind()
    {
        return currentRewindTime > 0;
    }
}
