using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

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
    [BoxGroup("Audio Clips")]
    public AudioClip invalid;
    [BoxGroup("Audio Clips")]
    public AudioClip menuClick;
    [BoxGroup("Audio Clips")]
    public AudioClip buyItem;
    [BoxGroup("Audio Clips")]
    public AudioClip gunShot;

    [BoxGroup("Audio Source")]
    public AudioSource audioSource;

    private float pitch;

    public enum SFX {PlaceObject, PlaceItem, RemoveObject, RemoveItem, SelectType, Invalid, MenuClick, BuyItem, GunShot}

    public void PlaySFX(SFX buildSFX)
    {
        //audioSource.Stop();
        audioSource.pitch = 1;
        pitch = 0;

        AudioClip selectedClip = null;

        switch (buildSFX)
        {
            case SFX.PlaceObject:
                {
                    selectedClip = placeObject;
                    pitch = UnityEngine.Random.Range(-0.2f, 1.5f);  // Set random pitch from range
                }
                break;
            case SFX.PlaceItem:
                {
                    selectedClip = placeItem;
                    pitch = UnityEngine.Random.Range(-0.2f, 1.5f);  // Set random pitch from range
                }
                break;
            case SFX.RemoveObject:
                {
                    selectedClip = removeObject;
                    pitch = UnityEngine.Random.Range(-0.2f, 1.5f);  // Set random pitch from range
                }
                break;
            case SFX.RemoveItem:
                { 
                    selectedClip = removeItem;
                    pitch = UnityEngine.Random.Range(-0.2f, 1.5f);  // Set random pitch from range
                }
                break;
            case SFX.SelectType:
                { 
                    selectedClip = selectType;
                    pitch = UnityEngine.Random.Range(-0.2f, 1.5f);  // Set random pitch from range
                }
                break;
            case SFX.Invalid:
                { 
                    selectedClip = invalid; 
                }
                break;
            case SFX.MenuClick:
                {
                    selectedClip = menuClick;
                }
                break;
            case SFX.BuyItem:
                {
                    selectedClip = buyItem;
                }
                break;
            case SFX.GunShot:
                {
                    selectedClip = gunShot;
                }
                break;
        }
        audioSource.pitch += pitch;
        
        Debug.Log($"Playing {selectedClip.name} at pitch {audioSource.pitch}...");
        audioSource.PlayOneShot(selectedClip);
    }

    private void Start()
    {
        instance = this;
    }
}
