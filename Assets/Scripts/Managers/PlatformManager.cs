using System;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public enum Type { Exit, Trap, Normal, Start }

    [SerializeField] FloorManager myFloor;

    [Header("Materials")]
    [SerializeField] Material trapMaterial;
    [SerializeField] Material normalMaterial;
    [SerializeField] Material startMaterial;

    [Header("From Player cheking")]
    [SerializeField] private Transform center;
    [SerializeField] private float detectionRadius;

    int _damage = 0;
    Type _currentType;

    public static Action<int> BallHit;
    public static Action<Vector3, int> Goal;
    public static Action<Vector3> DestroyPlatform;

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
                GetComponent<Renderer>().material = trapMaterial;
                _damage = damage;
                break;
            case Type.Normal:
                GetComponent<Renderer>().material = normalMaterial;
                _damage = damage;
                break;
            case Type.Start:
                GetComponent<Renderer>().material = startMaterial;
                _damage = 0;
                break;
            default:
                break;
        }
    }

    #region Player
    public bool CheckPlayerFromAbove()
    {
        float distanceToPlayer = Vector3.Distance(PlayerManager.singleton.transform.position, center.position);
        if (distanceToPlayer <= detectionRadius)
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
                DestroyPlatform(transform.position);
                myFloor.DestroyMe();
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
