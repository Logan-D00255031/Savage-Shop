using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    public static IsometricCamera Instance;

    [ReadOnly, SerializeField]
    bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        isActive = gameObject.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckActive(0.5f));

        if (Cursor.lockState == CursorLockMode.Locked && IsActive())
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private IEnumerator CheckActive(float interval)
    {
        yield return new WaitForSeconds(interval);
        isActive = this.gameObject.activeSelf;
    }

    public static bool IsActive()
    {
        return Instance.gameObject.activeSelf;
    }
}
