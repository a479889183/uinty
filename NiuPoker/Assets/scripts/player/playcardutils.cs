using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
/// <summary>
/// 出牌工具类
/// </summary>
public class Playcardutils {

    private static Playcardutils instance;
    /// <summary>
    /// 获取对象
    /// </summary>
    public static Playcardutils Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Playcardutils();
            }
            return instance;
        }
    }
    /// <summary>
    /// 出牌
    /// </summary>
    /// <param name="list">牌</param>
    public List<Card> playCards(List<Card> list,List<Card> olist)
    {
        List<Card> oCard = new List<Card>();  
        //出的是单牌
        if (olist.Count == 1) 
        {  
            int value = olist[0].value;
            int color = olist[0].color;

            if (color != CardManager.Instance.color && value < 10)
            {
                //是否存在花色
                bool isColor = false;

                foreach (Card item in list)
                {   //花色一样
                    if (item.color == color)
                    {
                        isColor = true;
                        if (item.value > value && item.value < 10)
                        {
                            oCard.Add(item);

                            return oCard;
                        }
                    }
                }
                if (isColor)
                {
                    //有同色没有大的值
                    foreach (Card item in list)
                    {
                        if (item.color == color && item.value < 10)
                        {
                            oCard.Add(item);

                            return oCard;
                        }
                    }

                }
                //没有同色牌 可出主牌
                else
                {
                    foreach (Card item in list)
                    {
                        if (item.color == CardManager.Instance.color)
                        {
                            oCard.Add(item);
                            return oCard;
                        }
                    }
                }
            }
            //主色牌 不包括2以上的常主
            else if (color == CardManager.Instance.color&&value<10) 
            {
                foreach (Card item in list)
                {   //大的牌
                    if ((item.value > value&&item.color==CardManager.Instance.color)||item.value>9)
                    {
                        oCard.Add(item);
                        return oCard;
                    }                  
                }
            }
            //出的是主牌  不是主色
            else if (value > 9 && color != CardManager.Instance.color)
            {
                foreach (Card item in list)
                {
                    if (item.value > value)
                    {
                        oCard.Add(item);
                        return oCard;
                    }
                    else if (item.value == value && item.color == CardManager.Instance.color)
                    {
                        oCard.Add(item);
                        return oCard;
                    }
                }
            }
            //出的是主牌
            else if (value > 9 && color == CardManager.Instance.color)
            {
                foreach (Card item in list)
                {
                    if (item.value > value)
                    {
                        oCard.Add(item);
                        return oCard;
                    }
                }
            }


        }
        //出的是对牌
        else if (olist.Count == 2)
        {
            //桌面上的牌的花色和值
            int value = olist[0].value;
            int color = olist[0].color;
         
            //存符合条件的牌
            List<Card> blist = new List<Card>();
            //出的不是主牌
            if (color != CardManager.Instance.color)
            {
                //得到牌集合中花色个数
                int count = PlayRules.Instance.CountColor(list, color);
                //先找出符合要求的单牌
                foreach (Card item in list)
                {    //有两个以上相同花色的牌    
                    if (item.color == color && item.value < 10)
                    {
                        blist.Add(item);
                    }
                }
                //找出符合要求的对牌
                for (int i = 0; i < blist.Count; i++)
                {
                    //找出对牌
                    for (int j = 0; j < blist.Count; j++)
                    {
                        if (i != j)
                        {   //符合条件牌
                            if (blist[i].value == blist[j].value && blist[i].color == blist[j].color && blist[i].color == color && blist[i].value > value)
                            {  
                                oCard.Add(blist[i]);
                                oCard.Add(blist[j]);
                                return oCard;
                            }
                        }
                    }
                }

                for (int i = 0; i < blist.Count; i++)
                {
                    //没有大的出同色连队
                    for (int j = 0; j < blist.Count; j++)
                    {
                        if (i != j)
                        {   //符合条件牌
                            if (blist[i].value == blist[j].value && blist[i].color == blist[j].color && blist[i].color == color)
                            {
                                oCard.Add(blist[i]);
                                oCard.Add(blist[j]);
                                return oCard;
                            }
                        }
                    }
                }
                    //没有对牌 出同色牌
                    if (count >= 2)
                    {   //遍历找出同色牌
                        foreach (Card item in list)
                        {
                            if (item.color == color && item.value < 10)
                            {
                                oCard.Add(item);
                                if (oCard.Count == 2)
                                {
                                    return oCard;
                                }
                            }
                        }
                    }
                    //没有同色的先做简单处理
                    else
                    {    
                        oCard.Add(list[list.Count - 1]);
                        oCard.Add(list[list.Count - 2]);
                        return oCard;
                    }       
            }
            //出的是主牌 
            else
            {
                int count = PlayRules.Instance.CountMianCard(list);
                //把符合条件的牌加入集合
                foreach (Card item in list) 
                {
                    if (item.color == CardManager.Instance.color || item.value > 10) 
                    {
                        blist.Add(item);
                    }
                }
                //找出符合要求的对牌
                for (int i = 0; i < blist.Count; i++)
                {
                    //找出对牌
                    for (int j = 0; j < blist.Count; j++)
                    {
                        if (i != j)
                        {   //符合条件牌
                            if (blist[i].value == blist[j].value && blist[i].color == blist[j].color &&blist[i].value > value)
                            {
                                oCard.Add(blist[i]);
                                oCard.Add(blist[j]);
                                return oCard;
                            }
                        }
                    }
                }
                //没有大的连对
                for (int i = 0; i < blist.Count; i++)
                {
                    //没有大的出同色连队
                    for (int j = 0; j < blist.Count; j++)
                    {
                        if (i != j)
                        {   //符合条件牌
                            if (blist[i].value == blist[j].value && blist[i].color == blist[j].color)
                            {
                                oCard.Add(blist[i]);
                                oCard.Add(blist[j]);
                                return oCard;
                            }
                        }
                    }
                }

                if (count >= 2) 
                {
                    //遍历找出同色牌
                    foreach (Card item in list)
                    {
                        if (item.color == color || item.value > 9)
                        {
                            oCard.Add(item);
                            if (oCard.Count == 2)
                            {
                                return oCard;
                            }
                        }
                    }
                }
                //没有同色的先做简单处理
                else
                {
                    oCard.Add(list[list.Count - 1]);
                    oCard.Add(list[list.Count - 2]);
                    return oCard;
                } 
            }
        }
        // 出的牌是两张以上的  简单处理直接返回
        else if (olist.Count > 3)
            {
                for (int i = 0; i < olist.Count; i++)
                {
                    oCard.Add(list[list.Count - (i + 1)]);
                }
                return oCard;
            }
        return oCard;
    }
    /// <summary>
    /// 根据name 返回牌
    /// </summary>
    /// <param name="list">牌的集合</param>
    /// <returns></returns>
    public Card getByName(List<Card> list,string name)
    {    
        foreach (Card item in list) 
        {
            if (item.name.Equals(name)) 
            {
                return item;
            }
        }
        return null;
    }
    /// <summary>
    /// 要加的分数
    /// </summary>
    /// <returns></returns>
    public int AddScore(Card card) 
    {   //5
        if (card.value == 1) 
        {
            return 5;
        }
         //K
        else if (card.value == 8) 
        {
            return 10;
        }
        //10
        else if (card.value == 11) 
        {
            return 10;
        }
        return 0;
    }

}
