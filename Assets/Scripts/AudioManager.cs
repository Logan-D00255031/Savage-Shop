using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Learned from tutorial: https://www.youtube.com/watch?v=N8whM1GjH4w 
public class AudioManager : MonoBehaviour
{
    [Header("------------ Audio Source ------------")]
    [SerializeField] AudioSource musicSource;

    [Header("------------ Audio Clip ------------")]
    public AudioClip background;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }



}
