using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewSystem : MonoBehaviour
{
    [SerializeField]
    private float yOffset = 0.05f;

    [SerializeField]
    private GameObject cellIndicator;
    private GameObject previewObject;

    [SerializeField]
    private Material prefabPreviewMaterial;
    private Material previewMaterialInstance;

    private Renderer cellIndicatorRenderer;

    private void Start()
    {
        previewMaterialInstance = new Material(prefabPreviewMaterial); 
        cellIndicator.SetActive(false);  // Hide Indicator
        cellIndicatorRenderer = cellIndicator.GetComponentInChildren<Renderer>();   // Instantiate Indicator Renderer
    }

    public void BeginPreview(GameObject prefab, Vector2Int size)
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
            Material[] materials = renderer.materials;  // Get all renderer materials
            for (int i = 0; i < materials.Length; i++)  // For every material in array
            {
                materials[i] = previewMaterialInstance; // Set material to instance material
            }
            renderer.materials = materials; // Set new materials to Renderer
        }
    }

    public void EndPreview()
    {
        cellIndicator.SetActive(false); // Hide Indicator
        Destroy(previewObject); // Remove Prefab preview from scene
    }

    public void UpdatePosition(Vector3 position, bool valid)
    {
        MovePreview(position);
        MoveCursor(position);
        ApplyFeedbackColour(valid);
    }

    private void ApplyFeedbackColour(bool valid)
    {
        Color color = valid ? Color.white : Color.red;  // Set color based on valid bool
        cellIndicatorRenderer.material.color = color;   // Set indicator color
        color.a = 0.5f; // Set color opacity
        previewMaterialInstance.color = color;  // Set Preview Material color
    }

    private void MoveCursor(Vector3 position)
    {
        cellIndicator.transform.position = position;    // Update indicator position
    }

    private void MovePreview(Vector3 position)
    {
        previewObject.transform.position = position + new Vector3(0, yOffset, 0);   // Update prefab preview position with y offset
    }
}
