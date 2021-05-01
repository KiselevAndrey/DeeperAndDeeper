using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public enum Type { Exit, Trap, Normal, Start }

    [Header("Main component")]
    [SerializeField] private FloorManager _myFloor;

    [Header("Materials")]
    [SerializeField] private Material _trapMaterial;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _startMaterial;

    [Header("From Player cheking")]
    [SerializeField] private Transform _center;
    [SerializeField] private float _detectionRadius;

    private int _damage = 0;
    private Type _currentType;

    public static Action<int> BallHit;              // from Player
    public static Action<Vector3, int> Goal;        // from Player
    public static Action<Vector3> DestroyPlatform;  // from Columnanager

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
            default:
                break;
        }
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
        switch (_currentType)
        {
            case Type.Exit:
                Goal(transform.position, _damage); // damade instead of score
                DestroyPlatform(_myFloor.transform.position);
                //myFloor.DestroyMe();
                break;

            case Type.Trap:
            case Type.Normal:
                BallHit(_damage);
                SetType(Type.Start);
                break;

            case Type.Start:
                break;

            default:
                break;
        }
    }
    #endregion
}
