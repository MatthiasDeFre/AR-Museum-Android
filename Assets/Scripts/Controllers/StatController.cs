using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class StatController 
{
    public static StatsWithLevel StatsWithLevel { get; private set; }
    private static string pathName = $"{Application.persistentDataPath}/stats.json";
    public static void InitDict()
    {
        try
        {
            var json = File.ReadAllText(pathName);
            StatsWithLevel = JsonConvert.DeserializeObject<StatsWithLevel>(json);
        } catch (Exception e)
        {
            StatsWithLevel = new StatsWithLevel();
            StatsWithLevel.NeededExp = 100;
            StatsWithLevel.CurrentLevel = 1;
            List<Stat> statList = new List<Stat> { new Stat { Type = StatType.Experience }, new Stat { Type = StatType.Riddles }, new Stat { Type = StatType.Scanned }, new Stat { Type = StatType.Stories } };
            StatsWithLevel.stats = new Dictionary<StatType, Stat>();
            statList.ForEach(s => StatsWithLevel.stats.Add(s.Type, s));
            PersistDict();
        }
     
        Debug.Log("NOT PERSIST " + pathName);
    }
    public static void PersistDict()
    {
        var json = JsonConvert.SerializeObject(StatsWithLevel);
        Debug.Log("PERSIST " + pathName);
        File.WriteAllText(pathName, json);
    }

    public static void IncrementStat(StatType type, double value) 
    {
        Stat stat;
        if (StatsWithLevel.stats.TryGetValue(type, out stat))
            stat.Value += value;
        if(type == StatType.Experience && stat.Value >= StatsWithLevel.NeededExp)
        {
            stat.Value -= StatsWithLevel.NeededExp;
            StatsWithLevel.CurrentLevel++;
            StatsWithLevel.NeededExp = (long)(StatsWithLevel.NeededExp * 1.1) + Mathf.FloorToInt(StatsWithLevel.NeededExp / 1000);
        }

    }
}
