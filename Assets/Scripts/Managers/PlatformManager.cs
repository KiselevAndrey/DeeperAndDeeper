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
                GetComponent<MeshCollider>().isTrigger = true;
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

    #region OnTrigger OnCollision
    private void OnTriggerExit(Collider other)
    {
        if (transform.position.y > Ball.singleton.transform.position.y)
        {
            Goal(transform.position, _damage); // damade instead of score
            DestroyPlatform(transform.position);
            myFloor.DestroyMe();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(Ball.singleton.transform.position.y > transform.position.y && _currentType != Type.Start)
        {
            BallHit(_damage);
            SetType(Type.Start);
        }
    }
    #endregion
}