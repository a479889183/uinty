using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 判断是否能够亮庄
/// </summary>
public class RobShow : MonoBehaviour {
    //扑克管理类
    private CardManager cm;
    /// <summary>
    /// 显示亮庄类
    /// </summary>
    private Robpoker rp;
    //是否运行
    public bool isRun;
    /// <summary>
    /// 亮庄的牌
    /// </summary>
    public List<Poker> robList = new List<Poker>();

    /// <summary>
    /// 是否是第一次
    /// </summary>
    private bool isfrist=true;

	void Start () {
        cm = CardManager.Instance;
        rp = gameObject.GetComponent<Robpoker>();
        isRun = true;   
	}
	
	void LateUpdate () {

        if (isRun) 
        {
            if (cm ==null) 
            {
                cm = CardManager.Instance;
            } 
            //自己可以亮庄的牌
            if (cm.mRob!=null&&cm.mRob.Count > 0) 
            {
              List<Poker> mlist= robPoker(cm.mRob);
              if (mlist != null)
              {
                  foreach (Poker item in mlist)
                  {   //没人抢庄
                      if (CardManager.Instance.zhuang == -1)
                      {
                          print("花色："+item.color);
                          rp.SendMessage("showPoker", item.color);
                      }
                      else
                      {
                          if (isfrist)
                          {   //庄不是自己的 把花色恢复默认
                              if (CardManager.Instance.zhuang != 0) 
                              {
                                  rp.SendMessage("resetDefault");
                                  isfrist = false;
                              }
                         
                          }
                          List<int> rlist = AnitZhuang(mlist);

                          if (rlist.Count > 0) 
                          {
                              foreach (int i in rlist) 
                              {
                                  rp.SendMessage("showPoker", i);
                              }
                          }
                          //判断是否有3个红2
                         List<int> twoList= AnitZhuang1(cm.mRob);
                         if (twoList.Count > 0)
                         {
                             foreach (int i in twoList)
                             {
                                 rp.SendMessage("showPoker", i);
                             }
                         }
                      }
                    
                  }
              }
            }
            //第一个人亮庄
            if (cm.fRob.Count > 0)
            {
                if (CardManager.Instance.zhuang == -1)
                {
                    List<Poker> mlist = robPoker(cm.fRob);
                    if (mlist != null)
                    {    
                        foreach (Poker item in mlist)
                        {
                            robList.Add(item);
                            object[] value = new object[2];
                            value[0] = item.color;
                            value[1] =1;
                            CardManager.Instance.robZhuang = 1;
                            //设置两边的队友 默认一打三
                            CardManager.Instance.banker.Add(1);
                            CardManager.Instance.civilians.Add(0);
                            CardManager.Instance.civilians.Add(2);
                            CardManager.Instance.civilians.Add(3);

                            rp.SendMessage("showRobPoker", value);
                            CardManager.Instance.zhuang = 1;
                       
                        }
                    }
                }
            }
            //第二个人亮庄
           if (cm.sRob.Count > 0)
            {
                if (CardManager.Instance.zhuang == -1)
                {
                    List<Poker> mlist = robPoker(cm.sRob);
                    if (mlist != null) 
                    { 
                    foreach (Poker item in mlist)
                    {
                        robList.Add(item);
                        object[] value = new object[2];
                        value[0] = item.color;
                        value[1] = 2;

                        CardManager.Instance.robZhuang = 2;
                        //设置两边的队友 默认一打三
                        CardManager.Instance.banker.Add(2);
                        CardManager.Instance.civilians.Add(0);
                        CardManager.Instance.civilians.Add(1);
                        CardManager.Instance.civilians.Add(3);

                        rp.SendMessage("showRobPoker", value);

                        CardManager.Instance.zhuang = 2;
             
                    }
                   }
                }
            }
            //第三个人亮庄
           if (cm.tRob.Count > 0)
            {
                if (CardManager.Instance.zhuang == -1)
                {
                    List<Poker> mlist = robPoker(cm.tRob);
                    if (mlist != null)
                    {
                        foreach (Poker item in mlist)
                        {
                            robList.Add(item);
                            object[] value = new object[2];
                            value[0] = item.color;
                            value[1] = 3;

                            CardManager.Instance.robZhuang = 3;
                            //设置两边的队友  默认一打三
                            CardManager.Instance.banker.Add(3);
                            CardManager.Instance.civilians.Add(0);
                            CardManager.Instance.civilians.Add(1);
                            CardManager.Instance.civilians.Add(2);

                            rp.SendMessage("showRobPoker", value);
                            CardManager.Instance.zhuang = 3;
                        }
                    }
                }
            }
        }
	
	}
    /// <summary>
    /// 判断可以亮庄的牌
    /// </summary>
   List<Poker> robPoker(List<Poker> list) 
    {   
        int twoCount=0;

        
        List<Poker> clist=new List<Poker>();
        
        foreach(Poker item in list)
        {
            if (item.type == 2) 
            {
                twoCount++;
            }
            else if (item.type == 1) 
            {  
                clist.Add(item);
            }
        }
        //抢庄
        if (twoCount > 0) 
        {
            if (clist.Count > 0)
            {
                return clist;
            }
        }   
        return null;
    }
    /// <summary>
    /// 判断能否抢庄
    /// </summary>
    /// <param name="list">能抢庄的牌</param>
List<int>  AnitZhuang(List<Poker> list) 
   {
       List<int> Alist = new List<int>();
       //抢庄的是一个10
       if (robList.Count == 1) 
       {

           int twoCount = 0;
           List<Poker> clist = new List<Poker>();

           foreach (Poker item in list)
           {
               if (item.type == 2)
               {
                   twoCount++;
               }
               else if (item.type == 1)
               {
                   clist.Add(item);
               }
           }
           List<int> color = new List<int>();
           foreach (Poker item in clist) 
           {
               color.Add(item.color);           
           }
           // 判断可以反庄的牌花色
           for (int i = 0; i < color.Count; i++)
           {
               for (int k = i + 1; k < color.Count; k++)
               {
                   if (color[i] == color[k])
                   {
                       Alist.Add(color[k]);
                       color.RemoveAt(k);
                       k--;
                   }
               }
           }
       }
       return Alist;
   }
    /// <summary>
    /// 三个红2可以反任意花色
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
List<int> AnitZhuang1(List<Poker> list) 
{    List<int> Alist=new List<int>();
    int twoCount = 0;
    foreach (Poker item in list)
    {
        if (item.type == 2)
        {
            twoCount++;
        }
    }

    if (twoCount >= 3) 
    {
        Alist.Add(0);
        Alist.Add(1);
        Alist.Add(2);
        Alist.Add(3);
    }
    return Alist;
}
    /// <summary>
    /// 设置抢庄的牌
    /// </summary>
    /// <param name="list"></param>
public void setRobList(List<Poker> list) 
{
    this.robList.Clear();
    this.robList = list;
} 

}
