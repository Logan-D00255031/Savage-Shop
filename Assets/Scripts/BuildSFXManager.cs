using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildSFXManager : MonoBehaviour
{
    [BoxGroup("Audio Clips")]
    public AudioClip placeObject;
    [BoxGroup("Audio Clips")]
    public AudioClip placeItem;
    [BoxGroup("Audio Clips")]
    public AudioClip removeObject;
    [BoxGroup("Audio Clips")]
    public AudioClip removeItem;
    [BoxGroup("Audio Clips")]
    public AudioClip selectType;

    [BoxGroup("Audio Source")]
    public AudioSource audioSource = new();

    public enum BuildSFX {PlaceObject, PlaceItem, RemoveObject, RemoveItem, SelectType}

    public void PlaySFX(BuildSFX buildSFX)
    {
        switch (buildSFX)
        {
            case BuildSFX.PlaceObject:
                { audioSource.clip = placeObject; }
                break;
            case BuildSFX.PlaceItem:
                { audioSource.clip = placeItem; }
                break;
            case BuildSFX.RemoveObject:
                { audioSource.clip = removeObject; }
                break;
            case BuildSFX.RemoveItem:
                { audioSource.clip = removeItem; }
                break;
        }
        // Set random pitch from range
        float pitch = UnityEngine.Random.Range(-1f, 1f);
        audioSource.pitch += pitch;

        Debug.Log($"Playing {audioSource.clip.name} at pitch {audioSource.pitch}...");
        audioSource.Play();
    }
}
