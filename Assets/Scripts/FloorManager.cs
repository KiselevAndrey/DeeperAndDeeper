using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] List<PlatformManager> platforms;



    #region Set Platforms
    public void SetStartPlatform()
    {
        foreach (PlatformManager platform in platforms)
        {
            platform.SetType(PlatformManager.Type.Start);
        }

        RandomPlatform().SetType(PlatformManager.Type.Exit, 0); // damade instead of score
    }

    public void SetPlatform(int difficult)
    {
        foreach (PlatformManager platform in platforms)
        {
            platform.SetType(PlatformManager.Type.Normal);
        }

        for (int i = 0; i < 5; i++)
        {
            if (Random.Range(0, Mathf.Pow(10, i)) < difficult)
                RandomPlatform().SetType(PlatformManager.Type.Trap, difficult);
        }

        int exitCount = (31 - difficult) / 10;
        exitCount = Mathf.Max(1, exitCount);

        for (int i = 0; i < exitCount; i++)
        {
            RandomPlatform().SetType(PlatformManager.Type.Exit);
        }
    }

    PlatformManager RandomPlatform() => platforms[Random.Range(0, platforms.Count)];
    #endregion

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}
