using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager{

    //自己的
    public List<Card> mList=new List<Card>();
    //第一个玩家
    public List<Card> fList=new List<Card>();
    //第二个玩家
    public List<Card> sList=new List<Card>();
    //第三个玩家
    public List<Card> tList=new List<Card>();
    /// <summary>
    /// 模式 1为巴十
    /// </summary>
    public int type = 1;
    /// <summary>
    /// 底牌
    /// </summary>
    public List<Card> blist = new List<Card>();
     //自己亮庄的牌
    public List<Poker> mRob = new List<Poker>();
    //第一个人亮庄的牌
    public List<Poker> fRob = new List<Poker>();
    //第二个人亮庄的牌
    public List<Poker> sRob = new List<Poker>();
    //第三个人亮庄的牌
    public List<Poker> tRob = new List<Poker>();
    //主色
    public int color=0;
    /// <summary>
    /// 记录牌是否已经全部发完
    /// </summary>
    public bool isSend=false;
    /// <summary>
    /// 记录谁是庄
    /// 0为自己
    /// 以此类推
    /// </summary>
    public int zhuang=-1;
    /// <summary>
    /// 庄
    /// </summary>
    public int robZhuang = -1;
    /// <summary>
    /// 桌面出的牌
    /// </summary>
    public List<Card> olist = new List<Card>();
    /// <summary>
    /// 桌面上第一次出的牌
    /// </summary>
    public List<Card> folist = new List<Card>();
    /// <summary>
    /// 出牌的人
    /// </summary>
    public int playCardPeople = -1;
    /// <summary>
    /// 记分牌
    /// </summary>
    public int Score=0;
    /// <summary>
    /// 存一轮牌的分数
    /// </summary>
    public int tempScore = 0;
    /// <summary>
    /// 庄家
    /// </summary>
    public List<int> banker=new List<int>();
    /// <summary>
    ///平民 队友
    /// </summary>
    public List<int> civilians=new List<int>();
    /// <summary>
    /// 记录第几轮出牌
    /// </summary>
    public int round = 0;
    /// <summary>
    /// 是否显示亮庄控件
    /// </summary>
    public bool isShowRob=false;

    private static CardManager instance;
    /// <summary>
    /// 获取对象
    /// </summary>
    public static CardManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance =new CardManager();
            }
            return instance;
            
        }
    }
    //清除所有的牌
    void clearAll() 
    {          
        mList.Clear();
        fList.Clear();
        sList.Clear();
        tList.Clear();
    }
    /// <summary>
    /// 恢复默认值+
    /// </summary>
  public  void Restore() 
    {  
        mList.Clear();
        fList.Clear();
        sList.Clear();
        tList.Clear();

        blist.Clear();

        mRob.Clear();
        sRob.Clear();
        fRob.Clear();
        tRob.Clear();

        isShowRob = false;
        color = 0;
        isSend = false;
        zhuang = -1; 
        robZhuang = -1;
   
       olist.Clear();
       folist.Clear();
       playCardPeople = -1;
       Score=0;
       tempScore = 0;
       banker.Clear();
       civilians.Clear();
       round = 0;

    }
  
}
