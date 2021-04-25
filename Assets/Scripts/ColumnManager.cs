using System.Collections.Generic;
using UnityEngine;

public class ColumnManager : MonoBehaviour
{
    public int difficult;

    [Header("Перереспавн")]
    [SerializeField] ColumnManager nextManager;
    public float height;

    [Header("Расстановка этажей")]
    [SerializeField] GameObject floorPrefab;
    [SerializeField] float startingPosY;
    [SerializeField] float minPosY;
    [SerializeField, Tooltip("X - min, Y - max")] Vector2 floorsDistance;

    List<FloorManager> _floors = new List<FloorManager>();

    #region Start Awake OnDestroy
    private void Start()
    {        
        CreateFloors();
    }

    private void Awake()
    {
        PlatformManager.DestroyPlatform += NewPosition;
    }

    private void OnDestroy()
    {
        PlatformManager.DestroyPlatform -= NewPosition;
    }
    #endregion

    #region NewPosition
    public void NewPosition(Vector3 goalPos)
    {
        if (goalPos.y > transform.position.y || goalPos != nextManager.GetPosFloor(2)) return;

        Vector3 newPos = nextManager.transform.position;
        newPos.y -= nextManager.height;
        transform.position = newPos;

        difficult = nextManager.difficult + 1;

        _floors.Clear();
        CreateFloors();
    }

    public Vector3 GetPosFloor(int index) 
    {
        if (_floors[index])
            return _floors[index].transform.position;
        else
            return transform.position;
    }
    #endregion

    #region CreateFloor
    void CreateFloors()
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
    }
    #endregion


}
