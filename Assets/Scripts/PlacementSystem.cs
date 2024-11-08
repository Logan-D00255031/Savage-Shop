using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

// Learned from Tutorial: https://youtu.be/rKp9fWvmIww?si=6ueW9PHdiFnlvi5h

public class PlacementSystem : MonoBehaviour
{    
    public static PlacementSystem instance;

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

    public List<GameObject> prefabs;
    [ReadOnly]
    private PlaceableObject desiredObject;

    #region Methods

    private void Awake()
    {
        // Instantiate Variables
        instance = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            InitializeObject(prefabs[0]);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            InitializeObject(prefabs[1]);
        }

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
    }

    public static Vector3 GetMouseInWorld() // Gets the positon in the scene that the mouse is over
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);    // Create ray from MainCamera mouse position
        if(Physics.Raycast(ray, out RaycastHit raycastHit)) // If the raycast hit something
        {
            return raycastHit.point;    // Return hit position in scene
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position); // Convert Vector position to Cell position
        position = grid.GetCellCenterWorld(cellPos);    // Get centre position of cell at Cell position
        return position;
    }

    public void InitializeObject(GameObject prefab)
    {
       Vector3 position = SnapToGrid(Vector3.zero);

       GameObject gameObject = Instantiate(prefab, position, Quaternion.identity);
       desiredObject = gameObject.GetComponent<PlaceableObject>();
       gameObject.AddComponent<ObjectDrag>();
    }

    private bool CanBePlaced(PlaceableObject placeableObject)
    {
        BoundsInt area = new BoundsInt();
        area.position = gridLayout.WorldToCell(desiredObject.GetStartPosition());
        area.size = desiredObject.Size;
        Debug.Log(area.position);

        TileBase[] tilebase = tilemap.GetTilesBlock(area);

        foreach(TileBase t in tilebase)
        {
            if(t == blueTile)
            {
                return false;
            }
        }
        return true;
    }

    public void ClaimArea(Vector3Int start,  Vector3Int size)
    {
        tilemap.BoxFill(start, blueTile, size.x, size.y, start.x + size.x, start.y + size.y);
    }

    public GridLayout GetGridLayout()
    {
        return gridLayout;
    }

    #endregion



}
