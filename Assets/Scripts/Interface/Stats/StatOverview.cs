using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StatOverview : MonoBehaviour
{
    public Image LevelRadial;
    public TextMeshProUGUI Level;
    public TextMeshProUGUI Stories;
    public TextMeshProUGUI Riddles;
    public TextMeshProUGUI Paintings;
    // Start is called before the first frame update
    public void Init()
    {
        var statsWithLevel = StatController.StatsWithLevel;
        Level.text = statsWithLevel.CurrentLevel.ToString();
        Stat exp;
        Stat riddles;
        Stat stories;
        Stat paintings;
       
        if(statsWithLevel.stats.TryGetValue(StatType.Experience, out exp)) {
            LevelRadial.fillAmount = (float)exp.Value / statsWithLevel.NeededExp;
        }
        if (statsWithLevel.stats.TryGetValue(StatType.Riddles, out riddles))
        {
            Riddles.text = riddles.Value.ToString();
        }
        if (statsWithLevel.stats.TryGetValue(StatType.Stories, out stories))
        {
            Stories.text = stories.Value.ToString();
        }
        if (statsWithLevel.stats.TryGetValue(StatType.Scanned, out paintings))
        {
            Paintings.text = paintings.Value.ToString();
        }
    }

  
}
