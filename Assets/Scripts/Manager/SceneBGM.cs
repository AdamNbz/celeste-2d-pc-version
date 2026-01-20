using UnityEngine;
using System.Collections.Generic;

public class SceneBGM : MonoBehaviour
{
    [SerializeField] private List<string> playlist;
    [SerializeField] private bool loopPlaylist = true;

    private int currentIndex = 0;

    void Start()
    {
        if (playlist == null || playlist.Count == 0) return;

        PlayCurrent();
    }

    void Update()
    {
        if (!AudioManager.Instance.IsBGMPlaying())
        {
            Next();
        }
    }

    void PlayCurrent()
    {
        AudioManager.Instance.PlayBGM(playlist[currentIndex], loop: false);
    }

    void Next()
    {
        currentIndex++;

        if (currentIndex >= playlist.Count)
        {
            if (loopPlaylist)
                currentIndex = 0;
            else
                return;
        }

        PlayCurrent();
    }
}
