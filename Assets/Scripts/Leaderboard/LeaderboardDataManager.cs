using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LeaderboardDataManager
{
    public static void SaveJsonData(LeaderboardData data)
    {

        if (FileManager.WriteToFile("Leaderboard.dat", data.ToJson()))
        {
            Debug.Log("Save successful");
        }
    }
    
    public static void LoadJsonData(LeaderboardData data)
    {
        if (FileManager.LoadFromFile("Leaderboard.dat", out var json))
        {
            data.LoadFromJson(json);

            Debug.Log("Load complete");
        } else {
            SaveJsonData(new LeaderboardData());
        }
    }

    public static void ResetData() {
        FileManager.DeleteFile("Leaderboard.dat");
    }
}
