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

    public void SetType(Type type, int damage = 1)
    {
        switch (type)
        {
            case Type.Exit:
                GetComponent<MeshRenderer>().enabled = false;
                GetComponent<MeshCollider>().isTrigger = true;
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
        if(transform.position.y > Ball.singleton.transform.position.y)
            myFloor.DestroyMe();
    }
    #endregion
}
