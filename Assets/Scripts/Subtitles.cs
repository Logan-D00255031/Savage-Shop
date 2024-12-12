
using UnityEngine;
using TMPro;

public class Subtitles : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI subtitleText = default;

    public static Subtitles instance;

    private void Awake()
    {
        instance = this;
    }
    public void SetSubtitle(string subtitle)
    {
        subtitleText.text = subtitle;
    }

}
