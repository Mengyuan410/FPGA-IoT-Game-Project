using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Leaderboard : MonoBehaviour
{
    public int MaxScore = 6;
    public TMPro.TextMeshProUGUI[] Entries_MVP;
    public TMPro.TextMeshProUGUI[] Entries_HIGHEST;
    public TMPro.TextMeshProUGUI[] Entries_RANK;
    public Mvpcount[] mvpcount_table;
    public HighestKillNo[] highestKillNo_table;
    public Ranked[] ranked_table;
    MyNetworkManger DataBaseTable;
    // Start is called before the first frame update
    void Start()
    {
        DataBaseTable = GameObject.Find("MyNetworkManger").GetComponent<MyNetworkManger>();
    }

    // Update is called once per frame
    void Update()
    {
        if(DataBaseTable.mvpcount_table != null)
        {
            mvpcount_table = DataBaseTable.mvpcount_table;
            MVP();
        }
        if (DataBaseTable.highestKillNo_table != null)
        {
            highestKillNo_table = DataBaseTable.highestKillNo_table;
            HIGH();
        }

       //        mvp 1 | hig 1 | rank 1
       //        mvp 2 | hig 2 | rank 2
    }

    public void MVP()
    {
        for (int i = 0; i < mvpcount_table.Length && i < 6; i++)
        {
            Entries_MVP[i].text = (mvpcount_table[i].userId + ".     " + mvpcount_table[i].score);
        }
        //if (mvpcount_table.Length < MaxScore)
        //{
        //    for (int i = mvpcount_table.Length; i < MaxScore; i++)
        //    {
        //        Entries_MVP[i].text = ".     none";
        //    }
        //}
    }
    public void HIGH()
    {
        for (int i = 0; i < highestKillNo_table.Length && i <6; i++)
        {
            Entries_HIGHEST[i].text = (highestKillNo_table[i].userId + ".     " + highestKillNo_table[i].score);
        }
        //if (highestKillNo_table.Length < MaxScore)
        //{
        //    for (int i = highestKillNo_table.Length; i < MaxScore; i++)
        //    {
        //        Entries_HIGHEST[i].text = ".     none";
        //    }
        //}
    }
    public void RANK()
    {
        for (int i = 0; i < ranked_table.Length; i++)
        {
            Entries_RANK[i].text = (ranked_table[i].gameID +   ".     "
                            + ranked_table[i].startTime + ".     "
                            + ranked_table[i].endTime + ".     "
                            + ranked_table[i].MVPUserID + ".     "
                            + ranked_table[i].Team + ".     "
                            + ranked_table[i].teamAUserID + ".     "
                            + ranked_table[i].teamBUserID + ".     "
                            );
        }
        if (ranked_table.Length < MaxScore)
        {
            for (int i = ranked_table.Length; i < MaxScore; i++)
            {
                Entries_RANK[i].text = ".     none";
            }
        }
    }
}
