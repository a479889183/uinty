using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 出牌规则
/// </summary>
public class PlayRules {


    private static PlayRules instance;
    /// <summary>
    /// 获取对象
    /// </summary>
    public static PlayRules Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayRules();
            }
            return instance;
        }
    }
/// <summary>
/// 判断是否大于桌面上的牌
/// </summary>
/// <param name="list">出的牌</param>
/// <returns></returns>
   public bool isSize(List<Card> list) 
    {
        List<Card> olist = CardManager.Instance.olist;
        //出的是单牌
        if (olist.Count == 1&&olist!=null&&list!=null&&list.Count==1)
        {
            return isPokerSize(Convert.ToInt32(olist[0].value),Convert.ToInt32(olist[0].color), Convert.ToInt32(list[0].value), Convert.ToInt32(list[0].color));
        }
        //出的是双牌
        else if (olist.Count == 2)
        {   //两张牌不一样
            if (list[0].color != list[1].color || list[1].value != list[0].value) 
            {
                return false;
            }
           
            return isPokerSize(Convert.ToInt32(olist[0].value), Convert.ToInt32(olist[0].color), Convert.ToInt32(list[0].value),Convert.ToInt32( list[0].color));      
        }
        //出的是连对
        else if (olist.Count > 3&&olist.Count%2==0) 
        {   //桌面上的牌
            HashSet<int> value = new HashSet<int>();
            HashSet<int> color = new HashSet<int>();
            foreach (Card item in olist) 
            {
                value.Add(item.value);
                color.Add(item.color);
            }
            //自己的牌
            HashSet<int> value1 = new HashSet<int>();
            HashSet<int> color1 = new HashSet<int>();

            foreach (Card item in list)
            {
                value1.Add(item.value);
                color1.Add(item.color);
            }

            if (color.Count == color1.Count && color1.Count==1) 
            {   //排序
                List<int> vlist = new List<int>(value);
                List<int> clist = new List<int>(color);
                vlist.Sort();
                clist.Sort();
                //排序
                List<int> vlist1 = new List<int>(value1);
                List<int> clist1 = new List<int>(color1);
                vlist1.Sort();
                clist1.Sort();

                return isPokerSize(vlist[0], clist[0], vlist1[0], clist1[0]);  
            }
           }  
        return false;
    }

  /// <summary>
  /// 判断牌的大小 大返回true
  /// </summary>
  /// <param name="value">比较值</param>
  /// <param name="color">比较值得花色</param>
  /// <param name="value1">自己牌值</param>
  /// <param name="color1">牌花色</param>
  /// <returns></returns>
    private bool isPokerSize(int value,int color,int value1,int color1) 
    {
        //桌面上的牌不是主色牌  不是主
        if (color != CardManager.Instance.color && value < 10)
        {   //花色一样
            if (color1 == color)
            {
                if (value1 > value)
                {
                    return true;
                }
            }
            else if (color1 == CardManager.Instance.color)
            {
                return true;
            }
            else if (value1 > 9)
            {
                return true;
            }
        }
        //桌面上的牌是主色牌
        else if (color == CardManager.Instance.color || value > 9)
        { 
            //出的不是常主
            if (value < 10)
            {
                if (color1 == CardManager.Instance.color)
                {
                    if (value1 > value)
                    {
                        return true;
                    }
                }
                else if (value1 > 9)
                {
                    return true;
                }
            }
            //出的是常主牌
            else if (value > 9)
            {   //出的不是主色
                if (color != CardManager.Instance.color)
                {
                    if (value1 > value || (value1 == value && color1 == CardManager.Instance.color))
                    {
                        return true;
                    }
                }
                else
                {
                    if (value1 > value)
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }
    /// <summary>
    /// 判断是否符合出牌规则
    /// </summary>
    /// <returns></returns>
    public bool isPlayRoles(List<Card> list) 
    {   //单牌
        if (list.Count == 1) return true;
        //双牌
        if (list.Count==2) 
        {
           return isDouble(list);
        }
        //连对
        if (list.Count > 3)
        {
            Debug.Log("listCount:"+list.Count);
            if (list.Count % 2 == 0) 
            {
               return IsDoubleStraight(list); 
            }    
        }
        return false;
    }
    /// <summary>
    /// 判断牌是否能出
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
   public bool isPlayCard(List<Card> list,List<Card> mlist) 
    {   //自己出牌
        if (CardManager.Instance.folist.Count == 0) return true;
        //桌面上的牌是单牌
        if (CardManager.Instance.folist.Count == 1) 
        {   //花色一样  出的不是主牌
            if (CardManager.Instance.folist[0].value < 10&&CardManager.Instance.folist[0].color!=CardManager.Instance.color)
            {
                if (CardManager.Instance.folist[0].color == list[0].color&&list[0].value<10)
                {
                    return true;
                }
                else
                { 
                    int Count = CountColor(mlist, list[0].color);
                    //整副牌中没有与桌面上面上相同的牌 可出其他牌
                    if (Count == 0)
                    {
                        return true;
                    }
                }
            }
            else 
            {   //出的是主牌
                int num = CountMianCard(mlist);
                if (num > 0)
                {
                    if (list[0].value > 9||list[0].color==CardManager.Instance.color)
                    {
                        return true;
                    }
                }
                else 
                {
                    return true;
                }               
            }
        }
        //桌面上出的牌是2个
        else if (CardManager.Instance.folist.Count == 2) 
        {
            int color = CardManager.Instance.folist[0].color;
            //出的牌不是主色牌
            if (CardManager.Instance.folist[0].value < 10&&color!=CardManager.Instance.color) 
            {
                //花色与出的牌花色一致 
                if (list[0].value < 10 && list[1].value < 10)
                {
                    if (list[0].color == color && list[1].color == color &&list[0].value==list[1].value)
                    {
                        return true;
                    }
                    else
                    {   //先找出牌中花色
                        int count = CountColor(mlist, color);
                        bool flag = false;
                        switch (count)
                        {      //没有花色 可出任意牌
                            case 0:
                                flag = true;
                                break;
                            case 1:
                                //有一个与桌面上花色相同即可
                                if ((list[0].color == color ) || (list[1].color == color))
                                {
                                    flag = true;
                                }
                                break;
                            //花色大于2的 两个牌的花色必须与出的牌花色保持一致
                            default:
                                if ((list[0].color == color ) && (list[1].color == color))
                                {
                                    flag = true;
                                }
                                break;
                        }
                        return flag;
                    }

                }
               
            }
            //桌面上是主牌
            else
            {   //主牌个数
                int num = CountMianCard(mlist);
                //没有主牌
                if (num == 0) { return true; }
                else if (num == 1)
                {
                    if ((list[0].value > 10 || list[0].color == CardManager.Instance.color) || (list[1].value > 10 || list[1].color == CardManager.Instance.color))
                    {
                        return true;
                    }
                }
                else if (num == 2)
                {
                    if ((list[0].value > 10 || list[0].color == CardManager.Instance.color) && (list[1].value > 10 || list[1].color == CardManager.Instance.color))
                    {
                        return true;
                    }
                }
            }
        }
        else if (CardManager.Instance.folist.Count > 3) 
        {
            int color = CardManager.Instance.folist[0].color;
            int count= CountColor(mlist,color);
            return isPlayPoker(CardManager.Instance.folist.Count,count, color, list);
        }
        return false;
    }
    /// <summary>
    /// 返回list中 主牌的个数
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
   public int CountMianCard(List<Card> list) 
    {
        int count = 0;
        foreach (Card item in list) 
        {
            if (item.value > 9 || item.color == CardManager.Instance.color) 
            {
                count++;
            }
        }
        return count;
    }
    /// <summary>
    /// 判断能否出牌
    /// </summary>
    /// <param name="size">桌面上牌的大小</param>
    /// <param name="count">与桌面牌花色相同的牌的个数</param>
    /// <param name="color">桌面上牌的颜色</param>
    /// <param name="list">出的牌</param>
    /// <returns></returns>
    bool isPlayPoker(int size,int count,int color,List<Card> list) 
    {
        int numer = 0;
        //出的不是主牌
        if (CardManager.Instance.folist[0].value < 10 && CardManager.Instance.folist[0].color != CardManager.Instance.color)
        {
            foreach (Card item in list)
            {
                if (item.color == color)
                {
                    numer++;
                }
            }
            //出的牌的花色
            if (numer >= count)
            {
                return true;
            }
        }
        else 
        {   //出的是主牌
            foreach (Card item in list)
            {
                if (item.color == CardManager.Instance.color||item.value>9)
                {
                    numer++;
                }
            }
            //出的牌的花色
            if (numer >= count)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 返回花色的个数
    /// list:牌 color：花色
    /// </summary>
    /// <returns></returns>
    public int CountColor(List<Card> list,int color)
    {
        int count = 0;
        foreach (Card item in list) 
        {
            if (item.value < 10)
            {
                if (item.color == CardManager.Instance.olist[0].color)
                {
                    count++;
                }
            }    
        }
        return count;
    }

    /// <summary>
    /// 是不是对子
    /// </summary>
    private bool isDouble(List<Card> list)
    {
        bool f=false;
        //判断花色和值是不是一样
        if (list[0].color == list[1].color && list[0].value == list[1].value) 
        {
            f= true;
        }
        return f;
    }
    /// <summary>
    /// 判断是否是连对
    /// </summary>
    /// <returns></returns>
    private bool IsDoubleStraight(List<Card> list) 
    {
        bool f = false;
        //存储值
        HashSet<int> value = new HashSet<int>();
        //存储花色
        HashSet<int> color = new HashSet<int>();
        //使用set 清除重复数据
        foreach (Card item in list) 
        {
            value.Add(item.value);
            color.Add(item.color);
        }
        //都是对子
        if (value.Count == list.Count / 2) 
        {  //都是同一种花色
          if(color.Count==1)
            {  //把值从小到大排序
                List<int> vlist = new List<int>(value);
                vlist.Sort();
                for (int i = 0; i < vlist.Count-1; i++) 
                {
                    if (vlist[i] + 1 == vlist[i + 1])
                    {
                        f = true;
                    }
                    else 
                    {
                        f = false;
                    }

                }
            }
        }
        return f;
    }
    /// <summary>
    /// 判断是否有此类型的牌
    /// </summary>
    /// <returns></returns>
    public bool isExist(List<Card> list)
    {   //先获取第一次出牌的花色与值
       // int num = CardManager.Instance.folist.Count;
        int value = CardManager.Instance.folist[0].value;
        int color = CardManager.Instance.folist[0].color;

        //int value1 = CardManager.Instance.olist[0].value;
        //int color1 = CardManager.Instance.olist[0].color;

        bool flag = false;
        int number = 0;

        //不是主牌
        if (value < 10 && color != CardManager.Instance.color) 
        {   //桌面上的牌不是主牌  桌面上的牌的类型与第一次出牌的类型一样
            //if (value1 < 10 && color1 != CardManager.Instance.color) 
            //{
            //    if (color == color1) 
            //    {
            //        return false;
            //    }  
            //}
            //遍历集合 是否有该类型的牌
            foreach (Card item in list) 
            {
                if (item.color == color) 
                {
                    number++;
                }
            }
            if (number > 0)
            {
                flag = true;
            }
        }
        //出的是主牌
        else if (value > 9 || color == CardManager.Instance.color) 
        {    //桌面上的牌 跟第一次出的牌类型一样
            //if (value1 > 9 || color1 == CardManager.Instance.color) 
            //{
            //    return false;
            //}
            //遍历是否有该类型的牌 
            foreach (Card item in list) 
            {
                if (item.value > 9 || item.color == CardManager.Instance.color) 
                {
                    number++;
                }
            }
            if (number > 0) { flag = true; }
        }
        return flag;
    }
    /// <summary>
    /// 找朋友
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    public int findFriends(List<Card> list) 
    {
        int count = 0;
        foreach (Card item in list) 
        {   //找主色6
            if (item.color == CardManager.Instance.color && item.value == 2) 
            {
                count++;
            }
        }
        return count;
    }
}
