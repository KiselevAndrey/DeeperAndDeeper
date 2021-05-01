using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    public int difficult;

    [Header("Перереспавн")]
    [SerializeField] private ColumnManager nextManager;
    public float height;

    [Header("Расстановка этажей")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private float startingPosY;
    [SerializeField] private float minPosY;
    [SerializeField, Tooltip("X - min, Y - max")] private Vector2 floorsDistance;
    [SerializeField] private float moveUpSpeed;

    private List<FloorManager> _floors = new List<FloorManager>();
    private int _currentFloorIndex;

    #region Start Awake OnDestroy
    private void Start()
    {        
        CreateFloors();
    }

    private void Awake()
    {
        PlatformManager.DestroyPlatform += DestroyPlatform;
        PlayerManager.CheckState += CheckPlayerPos;
    }

    private void OnDestroy()
    {
        PlatformManager.DestroyPlatform -= DestroyPlatform;
        PlayerManager.CheckState -= CheckPlayerPos;
    }
    #endregion

    #region Starts from event
    private void DestroyPlatform(Vector3 goalPlatformPos)
    {
        if (goalPlatformPos.y < transform.position.y && (goalPlatformPos - nextManager.GetPosFloor(2)).magnitude < 0.3f) NewPosition();

        if (!CurrentFloorsPosY(goalPlatformPos.y)) return;

        CurrentFloor().StartDestroyMe();

        _currentFloorIndex++;
        UpColumnToNextFloor();
    }

    /// <summary> Check over which platform the player is located </summary>
    private void CheckPlayerPos()
    {
        if(_currentFloorIndex < _floors.Count)
            CurrentFloor().CheckPlayersPlatform();
    }
    #endregion

    #region NewPosition
    private void NewPosition()
    {
        print(transform.name + " NewPos");
        Vector3 newPos = nextManager.transform.position;
        newPos.y -= nextManager.height;
        transform.position = newPos;

        difficult = nextManager.difficult + 1;

        _floors.Clear();
        CreateFloors();
    }

    private Vector3 GetPosFloor(int index)
    {
        if (_floors[index])
            return _floors[index].transform.position;
        else
            return transform.position;
    }
    #endregion

    #region CreateFloor
    private void CreateFloors()
    {
        float yPos = startingPosY;
        
        float yDist = floorsDistance.y - difficult * 0.1f;
        yDist = yDist > floorsDistance.x ? yDist : floorsDistance.x;

        while (yPos > minPosY)
        {
            Vector3 instPos = transform.position;
            instPos.y += yPos;
            yPos -= yDist;

            Instantiate(floorPrefab, transform).TryGetComponent(out FloorManager floor);
            floor.transform.position = instPos;
            _floors.Add(floor);
        }

        _floors[0].SetStartPlatform();
        for (int i = 1; i < _floors.Count; i++)
            _floors[i].SetPlatform(difficult);

        _currentFloorIndex = 0;
    }
    #endregion

    #region UpColumnToNextFloor
    private void UpColumnToNextFloor()
    {
        PlayerManager.singleton.ChangeState(PlayerManager.States.Fall);
        if (_currentFloorIndex < _floors.Count) StartCoroutine(MoveToNextFloor());
        else nextManager.UpColumnToNextFloor();
    }

    private IEnumerator MoveToNextFloor()
    {
        while (CurrentFloorsPosY() < PlayerManager.singleton.transform.position.y)
        {
            float upDistance = moveUpSpeed * Time.fixedDeltaTime;
            UpColumn(upDistance);
            nextManager.UpColumn(upDistance);
            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }

        float backDistance = CurrentFloorsPosY() - PlayerManager.singleton.transform.position.y;
        UpColumn(-backDistance);
        nextManager.UpColumn(-backDistance);

        PlayerManager.singleton.ChangeState(PlayerManager.States.Jump);
    }

    private void UpColumn(float upDistance)
    {
        Vector3 newPos = transform.position;
        newPos.y += upDistance;
        transform.position = Vector3.MoveTowards(transform.position, newPos, moveUpSpeed);
    }
    #endregion

    #region CurrentFloor
    private FloorManager CurrentFloor() => _floors[_currentFloorIndex];
    private float CurrentFloorsPosY() => GetPosFloor(_currentFloorIndex).y;
    private bool CurrentFloorsPosY(float y)
    {
        if (_currentFloorIndex >= _floors.Count) return false;
        return CurrentFloorsPosY() == y;
    }
    #endregion
}
