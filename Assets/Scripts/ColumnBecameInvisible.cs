using UnityEngine;

public class ColumnBecameInvisible : MonoBehaviour
{
    [SerializeField] ColumnManager columnManager;

    private void OnBecameInvisible()
    {
        columnManager.NewPosition();
    }
}
