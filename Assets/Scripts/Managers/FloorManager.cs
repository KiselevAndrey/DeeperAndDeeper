using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    [SerializeField] private List<PlatformManager> platforms;
    [SerializeField] private Animator animator;
    [SerializeField] private List<AnimationClip> deathAnimations;

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

    private PlatformManager RandomPlatform() => platforms[Random.Range(0, platforms.Count)];
    #endregion

    public void StartDestroyMe()
    {
        animator.SetFloat("DieFloat", Random.Range(0, deathAnimations.Count) * 0.1f);
        animator.SetTrigger("Die");

        Destroy(gameObject, 2f);
    }

    #region Player
    /// <summary>
    /// Check over which platform the player is located
    /// </summary>
    public void CheckPlayersPlatform()
    {
        for (int i = 0; i < platforms.Count; i++)
        {
            if (platforms[i].CheckPlayerFromAbove()) return;
        }
    }
    #endregion
}
