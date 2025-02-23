using System.Collections.Generic;
using UnityEngine;

public class RewindManager : MonoBehaviour
{
    public static RewindManager Instance;
    private List<RewindObject> rewindObjects = new List<RewindObject>();

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

    public void RegisterRewindObject(RewindObject rewindObject)
    {
        if (!rewindObjects.Contains(rewindObject))
        {
            rewindObjects.Add(rewindObject);
        }
    }

    public void PauseAllRewindObjects()
    {
        foreach (var obj in rewindObjects)
        {
            obj.PauseRewindRecording();
        }
    }

    public int GetMaxRewindIndex()
    {
        int maxIndex = 0;
        foreach (var obj in rewindObjects)
        {
            maxIndex = Mathf.Max(maxIndex, obj.GetRewindTickCount());
        }
        return maxIndex;
    }
}
