using UnityEngine;

public static class GameplayGate
{
    public static bool AllowGameplay { get; private set; } = true;

    public static void PauseGameplay()
    {
        AllowGameplay = false;
    }

    public static void ResumeGameplay()
    {
        AllowGameplay = true;
    }
}