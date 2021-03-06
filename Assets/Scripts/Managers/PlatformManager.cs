using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public enum Type { Exit, Trap, Normal, Start, BonusPunch, BonusLife }

    [Header("Main component")]
    [SerializeField] private FloorManager _myFloor;

    [Header("Materials")]
    [SerializeField] private Material _trapMaterial;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _startMaterial;
    [SerializeField] private Material _bonusMaterial;

    [Header("From Player cheking")]
    [SerializeField] private Transform _center;
    [SerializeField] private float _detectionRadius;

    private int _damage = 0;
    private Type _currentType;

    public static Action<int, Type> Collision;         // from Player
    public static Action<Vector3> DestroyPlatform;              // from ColumnManager

    #region From FloorManager
    public void SetType(Type type, int damage = 1)
    {
        _currentType = type;

        switch (type)
        {
            case Type.Exit:
                GetComponent<MeshRenderer>().enabled = false;
                _damage = damage; // damade instead of score
                break;
            case Type.Trap:
                GetComponent<Renderer>().material = _trapMaterial;
                _damage = damage;
                break;
            case Type.Normal:
                GetComponent<Renderer>().material = _normalMaterial;
                _damage = damage;
                break;
            case Type.Start:
                GetComponent<Renderer>().material = _startMaterial;
                _damage = 0;
                break;
        }
    }

    public void AddBonus(int difficult)
    {
        int bonuses = UnityEngine.Random.Range(0, 2);

        switch (bonuses)
        {
            case 0:
                _currentType = Type.BonusLife;
                break;

            case 1:
                _currentType = Type.BonusPunch;
                break;
        }

        _damage = difficult;
        GetComponent<Renderer>().material = _bonusMaterial;
    }
    #endregion

    #region Player
    public bool CheckPlayerFromAbove()
    {
        float distanceToPlayer = Vector3.Distance(PlayerManager.singleton.transform.position, _center.position);
        if (distanceToPlayer <= _detectionRadius)
        {
            HittingTreatment();
            return true;
        }

        return false;
    }

    private void HittingTreatment()
    {
        Collision(_damage, _currentType);

        switch (_currentType)
        {
            case Type.Exit:
                DestroyPlatform(_myFloor.transform.position);
                break;

            case Type.Trap:
            case Type.Normal:
            case Type.BonusLife:
            case Type.BonusPunch:
                SetType(Type.Start);
                break;

            case Type.Start:
                break;
        }
    }
    #endregion

    #region Another Functions
    public bool ImExit() => _currentType == Type.Exit;
    #endregion
}
