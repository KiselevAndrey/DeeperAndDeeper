#region PlayerData
[System.Serializable]
public class PlayerData
{
    readonly int maxHealth;
    readonly int healthBonusPurchased;

    readonly float money;
    readonly float goalMultiplier;
    readonly int goldAddedBonusPurchased;
    readonly int punchedBonusPurchased;

    readonly int bestScore;

    public PlayerData(PlayerManager player)
    {
        maxHealth = player.maxHealth;
        healthBonusPurchased = player.healthBonusPurchased;

        money = player.money;
        goalMultiplier = player.goalMultiplier;
        goldAddedBonusPurchased = player.goldAddedBonusPurchased;
        punchedBonusPurchased = player.punchingBonusPurchased;

        bestScore = player.bestScore;
    }

    public void LoadData(ref PlayerManager player)
    {
        player.maxHealth = maxHealth;
        player.healthBonusPurchased = healthBonusPurchased;

        player.money = money;
        player.goalMultiplier = goalMultiplier;
        player.goldAddedBonusPurchased = goldAddedBonusPurchased;
        player.punchingBonusPurchased = punchedBonusPurchased;

        player.bestScore = bestScore;
    }
}
#endregion

#region BallData
[System.Serializable]
public class BallData
{
    readonly int maxHealth;
    readonly int healthBonusPurchased;

    readonly float money;
    readonly float goalMultiplier;
    readonly int goldAddedBonusPurchased;
    readonly int punchedBonusPurchased;

    readonly int bestScore;

    public BallData(Ball ball)
    {
        maxHealth = ball.maxHealth;
        healthBonusPurchased = ball.healthBonusPurchased;

        money = ball.money;
        goalMultiplier = ball.goalMultiplier;
        goldAddedBonusPurchased = ball.goldAddedBonusPurchased;
        punchedBonusPurchased = ball.punchingBonusPurchased;

        bestScore = ball.bestScore;
    }

    public void LoadData(ref Ball ball)
    {
        ball.maxHealth = maxHealth;
        ball.healthBonusPurchased = healthBonusPurchased;

        ball.money = money;
        ball.goalMultiplier = goalMultiplier;
        ball.goldAddedBonusPurchased = goldAddedBonusPurchased;
        ball.punchingBonusPurchased = punchedBonusPurchased;

        ball.bestScore = bestScore;
    }
}
#endregion

#region ColumnRotateData
[System.Serializable]
public class ColumnRotateData
{
    readonly float rotationSpeed;
    readonly int speedBonusPurchased;

    public ColumnRotateData(ColumnRotate rotate)
    {
        rotationSpeed = rotate.rotationSpeed;
        speedBonusPurchased = rotate.speedBonusPurchased;
    }

    public void LoadData(ref ColumnRotate rotate)
    {
        rotate.rotationSpeed = rotationSpeed;
        rotate.speedBonusPurchased = speedBonusPurchased;
    }
}
#endregion