using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotepadManager : MonoBehaviour
{
    public GameObject notepadPanel; // Assign your Notepad Panel in the Inspector

    public void ToggleNotepad()
    {
        // Toggle the visibility of the Notepad Panel
        notepadPanel.SetActive(!notepadPanel.activeSelf);
    }
}
