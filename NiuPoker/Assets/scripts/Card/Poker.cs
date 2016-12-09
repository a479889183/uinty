using UnityEngine;
using System.Collections;

public class Poker {
    /// <summary>
    /// 扑克牌的名字
    /// </summary>
    public string pname { get; set; }
    /// <summary>
    /// 1为10 2为红色的2
    /// </summary>
    public int type { get; set; }

    /// <summary>
    /// 存颜色 0为黑1红2梅3方
    /// </summary>
    public int color { get; set; }

    public Poker(string name,int type,int color) 
    {
        this.pname = name;
        this.type = type;
        this.color=color;
    }
}
