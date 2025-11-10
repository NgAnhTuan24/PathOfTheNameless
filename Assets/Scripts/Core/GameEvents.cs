using System;

public static class GameEvents
{
    public static event Action OnChangedStats;

    public static void ChangedStats()
    {
        OnChangedStats?.Invoke();
    }
}
