using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

// Learned from Tutorial: https://youtu.be/rKp9fWvmIww?si=6ueW9PHdiFnlvi5h
// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

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

    [SerializeField, BoxGroup("Tilemap Properties")]
    private Tilemap tilemap;
    [SerializeField, BoxGroup("Tilemap Properties")]
    private TileBase blueTile;
    [SerializeField, BoxGroup("Tilemap Properties")]
    private TileBase whiteTile;
    [SerializeField, BoxGroup("Tilemap Properties")]
    private TileBase redTile;

    [ReadOnly]
    private Vector3 lastMousePosition = Vector3.zero;

    [SerializeField]
    private GameObject gridView;
    [SerializeField]
    private PrefabDatabaseSO prefabDatabase;

    private GridData groundData;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    IBuildState buildState;

    [SerializeField]
    private PreviewSystem previewSystem;

    [SerializeField]
    private PrefabInventoryManager prefabInventory;

    [ReadOnly, SerializeField]
    private Vector3Int lastGridPosition = Vector3Int.zero;

    [ReadOnly]
    public bool isPlaceState = false, isRemoveState = false;

    public event Action OnClick, OnExit;

    #region Methods

    private void Start()
    {
        EndPlacement();
        // Initialize GridData Variables
        groundData = new GridData();
    }

    public void StartPlacement(int ID)
    {
        EndPlacement();
        gridView.SetActive(true);
        isPlaceState = true;
        // Initialize PlacementState
        buildState = new PlacementState(ID,
                                        grid,
                                        previewSystem,
                                        prefabDatabase,
                                        groundData,
                                        objectPlacer,
                                        prefabInventory);

        OnClick += PlaceObject;
        OnExit += EndPlacement;
    }

    public void StartRemoval()
    {
        EndPlacement();
        gridView.SetActive(true);
        isRemoveState = true;
        // Initialize RemovalState
        buildState = new RemovalState(grid,
                                      previewSystem,
                                      groundData,
                                      objectPlacer,
                                      prefabInventory);

        OnClick += PlaceObject;
        OnExit += EndPlacement;
    }

    private void EndPlacement()
    {
        if (buildState == null) // If no Build State is active
        {
            return;
        }
        gridView.SetActive(false);

        buildState.EndState();
        isPlaceState = false;
        isRemoveState = false;

        OnClick -= PlaceObject;
        OnExit -= EndPlacement;

        lastGridPosition = Vector3Int.zero;
    }

    private void Awake()
    {
        gridView.SetActive(false);
        // Initialize Variables
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
            ExitBuildMode();
        }
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    StartPlacement(1);
        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    StartPlacement(2);
        //}
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    StartPlacement(3);
        //}
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    StartPlacement(4);
        //}
        //if (Input.GetKeyDown(KeyCode.Y))
        //{
        //    StartPlacement(5);
        //}
        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    StartPlacement(6);
        //}
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    StartPlacement(7);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    StartPlacement(8);
        //}
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    StartPlacement(9);
        //}
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    StartRemoval();
        //}


        if (buildState == null) // If not currently in a Build State
        {
            return;
        }
        Vector3Int gridPosition = grid.WorldToCell(GetMouseInWorld());
        if ((lastGridPosition != gridPosition) || (Input.GetAxis("Mouse ScrollWheel") != 0f))   // If grid position has changed or scroll wheel has moved
        {
            buildState.UpdateState(gridPosition);   // Update the Build State position
            lastGridPosition = gridPosition;
        }

    }

    public void ExitBuildMode()
    {
        OnExit?.Invoke();
    }

    public bool ActiveBuildState()
    {
        return isPlaceState || isRemoveState;
    }

    public bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetMouseInWorld() // Gets the positon in the scene that the mouse is over
    {
        Ray ray = placementCamera.ScreenPointToRay(Input.mousePosition);    // Create ray from MainCamera mouse position
        if(Physics.Raycast(ray, out RaycastHit raycastHit, 100, placementlayerMask)) // If the raycast hit something
        {
            lastMousePosition = raycastHit.point;    // Update last position to hit position in scene
        }
        return lastMousePosition;    
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cellPos = grid.WorldToCell(position); // Convert Vector position to Cell position
        position = grid.CellToWorld(cellPos);    // Get position of cell at Cell position
        return position;
    }

    public void PlaceObject()
    {
        if (IsPointerOverUI())  // If Event is Active
        {
            return;
        }
        Vector3 worldPosition = SnapToGrid(GetMouseInWorld());
        Vector3Int gridPosition = grid.WorldToCell(worldPosition);

        buildState.OnAction(gridPosition, true);  // Begin Placement Action
        
    }

    public void DestroyObject(Vector3 position)
    {
        StartRemoval();
        Vector3Int gridPosition = grid.WorldToCell(position);
        buildState.OnAction(gridPosition, false);
        ExitBuildMode();
    }

    // DEPRECATED METHOD
    //private bool CanBePlaced(PlaceableObject placeableObject)
    //{
    //    BoundsInt area = new BoundsInt();
    //    area.position = grid.WorldToCell(desiredObject.GetStartPosition());
    //    area.size = desiredObject.Size;
    //    //Debug.Log(area.position);

    //    TileBase[] tilebase = tilemap.GetTilesBlock(area);

    //    foreach(TileBase t in tilebase)
    //    {
    //        Debug.Log("Loop");
    //        if(t == blueTile)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}

    // DEPRECATED METHOD
    //public void ClaimArea(Vector3Int start,  Vector3Int size)
    //{
    //    tilemap.BoxFill(start, blueTile, start.x, start.y, start.x + size.x, start.y + size.y);
    //}

    public GridLayout GetGridLayout()
    {
        return gridLayout;
    }

    //private bool CheckValidPlacement(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    Vector2Int objectSize = prefabDatabase.objectsData[selectedObjectIndex].Size;
    //    return groundData.ObjectCanBePlacedAt(gridPosition, objectSize);
    //}

    #endregion



}
