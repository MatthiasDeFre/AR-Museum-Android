using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
   Experience,
   Riddles,
   Scanned,
   Stories
}
public static class StatTypeStringify
{
    public static string Stringify(this StatType statType)
    {
        switch(statType)
        {
            case StatType.Experience:
                return "Ervaring";
            case StatType.Riddles:
                return "Voltooide raadsels";
            case StatType.Scanned:
                return "Gescande schilderijen";
            case StatType.Stories:
                return "Levensverhalen";
            default:
                return "Not Found";
        }
    }
}
