using System.Collections.Generic;
using UnityEngine;

// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float yOffset = 0.05f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;
    private GameObject removePreviewObject;

    [SerializeField]
    private Material prefabPreviewMaterial;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private Dictionary<Renderer, Material[]> originalMaterials = new();

    private void Start()
    {
        previewMaterialInstance = new Material(prefabPreviewMaterial); 
        cellIndicator.SetActive(false);  // Hide Indicator
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();   // Instantiate Indicator Renderer
    }

    public void BeginPlacementPreview(GameObject prefab, Vector2Int size)
    {
        previewObject = Instantiate(prefab);
        PreparePreview(previewObject);
        PrepareIndicator(size);
        cellIndicator.SetActive(true);  // Show Indicator
    }

    private void PrepareIndicator(Vector2Int size)
    {
        if (size.x > 0 || size.y > 0)   // If Prefab size if larger than 0x0
        {
            cellIndicator.transform.localScale = new Vector3(size.x, 1, size.y);    // Scale x and y by size x and y
            cellIndicatorRenderer.material.mainTextureScale = size; // Scale Renderer material texture
        }
    }

    private void PreparePreview(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();   // Get all prefab Renderers
        foreach (Renderer renderer in renderers)
        {
            if(!originalMaterials.ContainsKey(renderer))    // If renderer is not already stored in dictionary
            {
                originalMaterials.Add(renderer, renderer.materials);    // Add renderer and its materials to dictionary
            }

            Material[] materials = renderer.materials;  // Get all renderer materials
            for (int i = 0; i < materials.Length; i++)  // For every material in array
            {
                materials[i] = previewMaterialInstance; // Set material to instance material
            }
            renderer.materials = materials; // Set new materials to Renderer
        }
    }

    private void RestorePreviewObjectMaterials(GameObject previewObject)
    {
        Renderer[] renderers = previewObject.GetComponentsInChildren<Renderer>();   // Get all prefab Renderers
        foreach (Renderer renderer in renderers)
        {
            if (originalMaterials.ContainsKey(renderer))    // If renderer is stored in dictionary
            {
                renderer.materials = originalMaterials[renderer];   // Restore materials from dictionary
                originalMaterials.Remove(renderer); // Remove renderer from dictionary
            }
        }
    }

    public void EndPreview()
    {
        EndRemovalPreview();
        cellIndicator.SetActive(false); // Hide Indicator
        if (previewObject != null)  // If there is an active preview object
        {
            Destroy(previewObject); // Remove Prefab preview from scene
            previewObject = null;
        }
    }

    public void EndRemovalPreview()
    {
        if (removePreviewObject != null)  // If there is an active preview object
        {
            RestorePreviewObjectMaterials(removePreviewObject);   // Restores the original materials the object had before being replaced
            removePreviewObject = null;
        }
    }

    public void UpdatePosition(Vector3 position, bool valid)
    {
        if (previewObject != null)  // If there is an active preview object
        {
            MovePreview(position);
            ApplyPreviewFeedbackColour(valid);
        }

        MoveCursor(position);
        ApplyIndicatorFeedbackColour(valid);
    }

    private void ApplyPreviewFeedbackColour(bool valid)
    {
        Color color = valid ? Color.white : Color.red;  // Set color based on valid bool
        color.a = 0.5f; // Set color opacity
        previewMaterialInstance.color = color;  // Set Preview Material color
    }
    private void ApplyIndicatorFeedbackColour(bool valid)
    {
        Color color = valid ? Color.white : Color.red;  // Set color based on valid bool
        color.a = 0.5f; // Set color opacity
        cellIndicatorRenderer.material.color = color;   // Set indicator color
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;    // Update indicator position
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = position + new Vector3(0, yOffset, 0);   // Update prefab preview position with y offset
    }

    public void BeginRemovalPreview()
    {
        cellIndicator.SetActive(true);
        PrepareIndicator(Vector2Int.one);
        ApplyIndicatorFeedbackColour(false);
    }

    public void BeginRemovalPreview(GameObject previewObject)
    {
        removePreviewObject = previewObject;
        PreparePreview(removePreviewObject);
        ApplyPreviewFeedbackColour(false);
    }
}
