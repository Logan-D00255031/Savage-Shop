using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

// Learned from Tutorial: https://youtu.be/rKp9fWvmIww?si=6ueW9PHdiFnlvi5h

public class PlacementSystem : MonoBehaviour
{    
    public static PlacementSystem instance;

    [Required]
    public Camera placementCamera;
    [Required]
    public LayerMask placementlayerMask;
    [Required]
    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private TileBase blueTile;
    [SerializeField]
    private TileBase whiteTile;
    [SerializeField]
    private TileBase redTile;

    public GameObject cellIndicator;
    //public List<GameObject> prefabs;
    [ReadOnly]
    private PlaceableObject desiredObject;
    [ReadOnly]
    private Vector3 lastPosition = Vector3.zero;

    [SerializeField]
    private GameObject gridView;
    [SerializeField]
    private PrefabDatabaseSO prefabDatabase;
    private int selectedObjectIndex = -1;

    private GridData groundData;

    private List<GameObject> placedGroundObjects;

    private Renderer previewRenderer;

    public event Action OnClick, OnExit;

    #region Methods

    private void Start()
    {
        EndPlacement();
        groundData = new GridData();
        placedGroundObjects = new List<GameObject>();
        previewRenderer = cellIndicator.GetComponentInChildren<Renderer>();
    }

    public void StartPlacement(int ID)
    {
        EndPlacement();
        selectedObjectIndex = prefabDatabase.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError($"ID not found {ID}");
            return;
        }
        gridView.SetActive(true);
        cellIndicator.SetActive(true);
        OnClick += InitializeObject;
        OnExit += EndPlacement;
    }

    private void EndPlacement()
    {
        selectedObjectIndex = -1;
        gridView.SetActive(false);
        cellIndicator.SetActive(false);
        OnClick -= InitializeObject;
        OnExit -= EndPlacement;
    }

    private void Awake()
    {
        // Instantiate Variables
        instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }

        Vector3Int gridPos = gridLayout.WorldToCell(GetMouseInWorld());
        cellIndicator.transform.position = gridLayout.CellToWorld(gridPos);

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            StartPlacement(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            StartPlacement(2);
        }

        /*
        if(!desiredObject)
        {
            return;
        }
        
        if(Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if(CanBePlaced(desiredObject))
            {
                desiredObject.Place();
                Vector3Int start = gridLayout.WorldToCell(desiredObject.GetStartPosition());
                ClaimArea(start, desiredObject.Size);
                Debug.Log($"Size: {desiredObject.Size}, Start: {start}");
            }
            else
            {
                Destroy(desiredObject.gameObject);
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(desiredObject.gameObject);
        }
        */
        if(selectedObjectIndex < 0)
        {
            return;
        }
        bool validPlacement = CheckValidPlacement(gridPos, selectedObjectIndex);
        previewRenderer.material.color = validPlacement ? Color.white : Color.red;

    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetMouseInWorld() // Gets the positon in the scene that the mouse is over
    {
        Ray ray = placementCamera.ScreenPointToRay(Input.mousePosition);    // Create ray from MainCamera mouse position
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 100, placementlayerMask)) // If the raycast hit something
        {
            lastPosition = raycastHit.point;    // Update last position to hit position in scene
        }
        return lastPosition;    
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position); // Convert Vector position to Cell position
        position = grid.CellToWorld(cellPos);    // Get position of cell at Cell position
        return position;
    }

    public void InitializeObject()
    {
        Vector3 position = SnapToGrid(GetMouseInWorld());
        Vector3Int gridPosition = gridLayout.WorldToCell(position);

        if (!CheckValidPlacement(gridPosition, selectedObjectIndex)) 
        { 
            return; 
        }

        GameObject newObject = Instantiate(prefabDatabase.objectsData[selectedObjectIndex].Prefab, position, Quaternion.identity);
        Debug.Log(newObject.name);
        Debug.Log(newObject.transform.position);
        placedGroundObjects.Add(newObject);

        desiredObject = newObject.GetComponent<PlaceableObject>();
        newObject.AddComponent<ObjectDrag>();

        groundData.AddObjectAt(gridPosition, 
            prefabDatabase.objectsData[selectedObjectIndex].Size,
            prefabDatabase.objectsData[selectedObjectIndex].ID,
            placedGroundObjects.Count - 1);
    }
    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(desiredObject.GetStartPosition());
        area.size = desiredObject.Size;
        //Debug.Log(area.position);

        TileBase[] tilebase = tilemap.GetTilesBlock(area);

        foreach(TileBase t in tilebase)
        {
            Debug.Log("Loop");
            if(t == blueTile)
            {
                return false;
            }
        }
        return true;
    }

    public void ClaimArea(Vector3Int start,  Vector3Int size)
    {
        tilemap.BoxFill(start, blueTile, start.x, start.y, start.x + size.x, start.y + size.y);
    }

    public GridLayout GetGridLayout()
    {
        return gridLayout;
    }

    private bool CheckValidPlacement(Vector3Int gridPosition, int selectedObjectIndex)
    {
        Vector2Int objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
        return groundData.ObjectCanBePlacedAt(gridPosition, objectSize);
    }

    #endregion



}
