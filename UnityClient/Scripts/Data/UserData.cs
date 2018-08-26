using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public string userName { get;private set; }
    public int totalCount { get;private set; }
    public int winCount { get;private set; }
    public int userId { get;private set; }

    public UserData(string name, int total, int win)
    {
        this.userName = name;
        this.totalCount = total;
        this.winCount = win;
    }

    public UserData(int id,string name,int total,int win)
    {
        this.userId = id;
        this.userName = name;
        this.totalCount = total;
        this.winCount = win;
    }

    public UserData(string data)
    {
        string[] strs = data.Split(',');
        this.userId = int.Parse(strs[0]);
        this.userName = strs[1];
        this.totalCount = int.Parse(strs[2]);
        this.winCount = int.Parse(strs[3]);
    }
}
