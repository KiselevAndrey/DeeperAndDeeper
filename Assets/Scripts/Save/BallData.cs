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
