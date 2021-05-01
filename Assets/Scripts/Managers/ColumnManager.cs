using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    [SerializeField] PlayerManager player;
    public int difficult;

    [Header("Перереспавн")]
    [SerializeField] private ColumnManager nextManager;
    public float height;

    [Header("Расстановка этажей")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private float startingPosY;
    [SerializeField] private float minPosY;
    [SerializeField, Tooltip("X - min, Y - max")] private Vector2 floorsDistance;

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
        if (goalPlatformPos == nextManager.GetPosFloor(2)) NewPosition();

        if (goalPlatformPos.y != _floors[_currentFloorIndex].transform.position.y) return;

        _floors[_currentFloorIndex].StartDestroyMe();
        _currentFloorIndex++;

        UpColumnToNextFloor();
    }

    /// <summary> Check over which platform the player is located </summary>
    private void CheckPlayerPos()
    {
        _floors[_currentFloorIndex].CheckPlayersPlatform();
    }
    #endregion

    #region NewPosition
    private void NewPosition()
    {
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

    #region Player
    private void UpColumnToNextFloor()
    {

    }
    #endregion
}
