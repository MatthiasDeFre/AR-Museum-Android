using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsWithLevel
{
    public Dictionary<StatType, Stat> stats;
    public int CurrentLevel { get; set; }
    public long NeededExp { get; set; }
}
