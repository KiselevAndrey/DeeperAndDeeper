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