using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CardUtils{
    /// <summary>
    /// 牌的集合
    /// </summary>
    public List<Card> list = new List<Card>();


    private static CardUtils _instance;

    public static CardUtils GetInstance()
    {
        if (_instance == null)
        {
            _instance = new CardUtils();
        }
        return _instance;
    }

   /// <summary>
   /// 获取牌
   /// </summary>
   /// <param name="num">几副牌</param>
   /// <param name="size">每一付牌的大小</param>
   /// <param name="type">游戏类型</param>
    public List<Card> getPoker(int num,int  size,int type) 
    {
        List<Card> pCard = new List<Card>();
        list.Clear();
        switch (type) 
        {   //巴十
            case 1:
                for (int i = 0; i < num; i++) 
                {
                    initCard(size);
                }
              pCard=getBsPoker();
                break;
            default :

                break;
        }
        return pCard;
    }
    /// <summary>
    /// 获取牌
    /// Size  牌有多少个
    /// </summary>
    void initCard(int size) 
    {
        for (int i = 0; i < size; i++) 
        {
            Card card = new Card();
            card.name = "p" + (i + 1);
            card.pokername = "p" + (i + 1);
            card.actualValue = i %13+1;
         
            card.color = i / 13;
            //小王
            if (i == 52) 
            {
                card.actualValue = 14;
                card.color = -1;
            }
             //大王
            else if (i == 53) 
            {
                card.actualValue = 15;
                card.color = -1;
            }

            list.Add(card);

        }
    }
    /// <summary>
    /// 获取巴十的扑克牌
    /// </summary>
    List<Card> getBsPoker() 
    {   
        List<Card> bslist=new List<Card>();
        //去掉三和四
        if (list.Count > 0&&list!=null) 
        {
            foreach (Card item in list) 
            {
                switch (item.actualValue) 
                {  //A
                    case 1:
                        item.value = 9;
                        bslist.Add(item);
                        break;
                   //2
                    case 2:
                        item.value = 10;
                        bslist.Add(item);
                        break;
                    //巴士 去掉3和4
                    case 3:
                        break;
                    case 4:
                        break;

                    case 5:
                        item.value = 1;
                        bslist.Add(item);
                        break;
                    case 6:
                         item.value = 2;
                        bslist.Add(item);
                        break;
                    case 7:
                         item.value = 3;
                        bslist.Add(item);
                        break;
                    case 8:
                         item.value = 4;
                        bslist.Add(item);
                        break;
                    case 9:
                         item.value = 5;
                        bslist.Add(item);
                        break;
                    case 10:
                         item.value = 11;
                        bslist.Add(item);
                        break;
                    case 11:
                         item.value = 6;
                        bslist.Add(item);
                        break;
                    case 12:
                         item.value = 7;
                        bslist.Add(item);
                        break;
                    case 13:
                        item.value = 8;
                        bslist.Add(item);
                        break;
                        //大王
                    case 14:
                        item.value = 12;
                        bslist.Add(item);
                        break;
                        //小王
                    case 15:
                        item.value =13;
                        bslist.Add(item);
                        break;
                }
            } 
        }
        return bslist;
    }
    /// <summary>
    /// 洗牌
    /// </summary>
    public List<Card> Shuffle(List<Card> cardList)
    {
        if (cardList.Count!=null)
        {
            System.Random random = new System.Random();
            List<Card> newList = new List<Card>();
            foreach (Card item in cardList)
            {
                newList.Insert(random.Next(newList.Count + 1), item);
            }
            cardList.Clear();

            foreach (Card item in newList)
            {
                cardList.Add(item);
                // print(item.name+":花色：" + item.color + "  值：" + item.value);
            }
            newList.Clear();
        }

        return cardList;
    }
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="list">扑克集合</param>
    /// <param name="color">主色调</param>
    /// <returns></returns>
    public List<Card> sortlist(List<Card> list, int color)
    {

        List<Card> mplist = new List<Card>();
        //黑桃
        List<Card> wlist = new List<Card>();
        //红桃
        List<Card> rlist = new List<Card>();
        //梅花
        List<Card> mlist = new List<Card>();
        //方块
        List<Card> flist = new List<Card>();

        Card[] pk = new Card[list.Count];


        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].value > 9)
            {
                mplist.Add(list[i]);
            }
            if (list[i].value < 10)
            {   //黑
                if (list[i].color == 0)
                {
                    wlist.Add(list[i]);
                }
                //红
                if (list[i].color == 1)
                {
                    rlist.Add(list[i]);
                }
                //梅花
                if (list[i].color == 2)
                {
                    mlist.Add(list[i]);
                }
                //方块
                if (list[i].color == 3)
                {
                    flist.Add(list[i]);
                }
            }
        }
        //对常主进行排序
        Card[] bpoker = mainSortingBig(mplist.ToArray(), color);
        //把常主加进去
        for (int i = 0; i < bpoker.Length; i++)
        {
            pk[i] = bpoker[i];
        }
        //黑色
        Card[] wpokers = sortingBig(wlist.ToArray());
        //红色
        Card[] rpokers = sortingBig(rlist.ToArray());
        //梅花
        Card[] mpokers = sortingBig(mlist.ToArray());
        //方块
        Card[] fpokers = sortingBig(flist.ToArray());
        //主色为黑色
        if (color == 0)
        {
            //把主色的追加到主牌后面
            for (int i = 0; i < wpokers.Length; i++)
            {
                pk[mplist.Count + i] = wpokers[i];
            }

            // 把红色牌追加到牌后面

            for (int i = 0; i < rpokers.Length; i++)
            {
                pk[mplist.Count + wpokers.Length + i] = rpokers[i];
            }

            //把梅花牌加到后面
            for (int i = 0; i < mpokers.Length; i++)
            {
                pk[mplist.Count + wpokers.Length + rpokers.Length + i] = mpokers[i];
            }

            //把方块加到后面

            for (int i = 0; i < fpokers.Length; i++)
            {
                pk[mplist.Count + wpokers.Length + rpokers.Length + mpokers.Length + i] = fpokers[i];
            }

        }
        //主色为红色
        else if (color == 1)
        {
            // 把红色牌追加到牌后面

            for (int i = 0; i < rpokers.Length; i++)
            {
                pk[mplist.Count + i] = rpokers[i];
            }

            //把黑色加进去

            for (int i = 0; i < wpokers.Length; i++)
            {
                pk[mplist.Count + rpokers.Length + i] = wpokers[i];
            }

            //把方块加进去
            for (int i = 0; i < fpokers.Length; i++)
            {
                pk[mplist.Count + wpokers.Length + rpokers.Length + i] = fpokers[i];
            }
            //把梅花牌加到后面
            for (int i = 0; i < mpokers.Length; i++)
            {
                pk[mplist.Count + wpokers.Length + rpokers.Length + fpokers.Length + i] = mpokers[i];
            }

        }
        //主色为梅花
        else if (color == 2)
        {
            //把梅花牌加到后面
            for (int i = 0; i < mpokers.Length; i++)
            {
                pk[mplist.Count + i] = mpokers[i];
            }
            // 把红色牌追加到牌后面
            for (int i = 0; i < rpokers.Length; i++)
            {
                pk[mplist.Count + mpokers.Length + i] = rpokers[i];
            }
            //把黑色加进去
            for (int i = 0; i < wpokers.Length; i++)
            {
                pk[mplist.Count + mpokers.Length + rpokers.Length + i] = wpokers[i];
            }
            //把方块加进去
            for (int i = 0; i < fpokers.Length; i++)
            {
                pk[mplist.Count + mpokers.Length + wpokers.Length + rpokers.Length + i] = fpokers[i];
            }
        }
        else if (color == 3)
        {   //把方块加进去
            for (int i = 0; i < fpokers.Length; i++)
            {
                pk[mplist.Count + i] = fpokers[i];
            }

            //把黑色加进去
            for (int i = 0; i < wpokers.Length; i++)
            {
                pk[mplist.Count + fpokers.Length + i] = wpokers[i];
            }
            // 把红色牌追加到牌后面
            for (int i = 0; i < rpokers.Length; i++)
            {
                pk[mplist.Count + fpokers.Length + wpokers.Length + i] = rpokers[i];
            }
            //把梅花加进去
            for (int i = 0; i < mpokers.Length; i++)
            {
                pk[mplist.Count + fpokers.Length + wpokers.Length + rpokers.Length + i] = mpokers[i];
            }
        }
        List<Card> clist = new List<Card>(pk);

        return clist;
    }
    /// <summary>
    /// 对常主进行排序
    /// </summary>
    /// <param name="pokers"></param>
    /// <returns></returns>
    public Card[] mainSortingBig(Card[] pokers, int color)
    {
        //Card[] pk = new Card[pokers.Length];
        //先按大小排序
        for (int i = 0; i < pokers.Length; i++)
        {
            for (int j = 0; j < pokers.Length; j++)
            {
                if (pokers[i].value > pokers[j].value)
                {
                    Card temp = pokers[i];
                    pokers[i] = pokers[j];
                    pokers[j] = temp;
                }
            }
        }

        //先按大小排序
        for (int i = 0; i < pokers.Length; i++)
        {
            for (int j = 0; j < pokers.Length; j++)
            {
                if (pokers[i].value == pokers[j].value && pokers[i].value < 12)
                {  //主色是黑桃
                    if (color == 0)
                    {
                        if (pokers[i].color < pokers[j].color)
                        {
                            Card temp = pokers[i];
                            pokers[i] = pokers[j];
                            pokers[j] = temp;
                        }
                    }
                    //主色是红桃
                    else if (color == 1)
                    {

                        if (pokers[i].color == color)
                        {
                            Card temp = pokers[i];
                            pokers[i] = pokers[j];
                            pokers[j] = temp;
                        }
                        else
                        {
                            if (pokers[i].color < pokers[j].color && pokers[i].color != color && pokers[j].color != color)
                            {
                                Card temp = pokers[i];
                                pokers[i] = pokers[j];
                                pokers[j] = temp;
                            }
                        }
                    }
                    //主色是梅花
                    else if (color == 2)
                    {
                        if (pokers[i].color == color)
                        {
                            Card temp = pokers[i];
                            pokers[i] = pokers[j];
                            pokers[j] = temp;
                        }
                        else
                        {
                            if (pokers[i].color < pokers[j].color && pokers[i].color != color && pokers[j].color != color)
                            {
                                Card temp = pokers[i];
                                pokers[i] = pokers[j];
                                pokers[j] = temp;
                            }
                        }
                    }
                    //主色是方块
                    else if (color == 3)
                    {
                        if (pokers[i].color > pokers[j].color)
                        {
                            Card temp = pokers[i];
                            pokers[i] = pokers[j];
                            pokers[j] = temp;
                        }
                    }
                }
            }
        }
        return pokers;
    }

    /// <summary>
    /// 排序从大到小
    /// </summary>
    /// <param name="pokers"></param>
    /// <returns></returns>
    public Card[] sortingBig(Card[] pokers)
    {
        for (int i = 0; i < pokers.Length; i++)
        {
            for (int j = 0; j < pokers.Length; j++)
            {
                if (pokers[i].value > pokers[j].value)
                {
                    Card temp = pokers[i];
                    pokers[i] = pokers[j];
                    pokers[j] = temp;
                }
            }
        }
        return pokers;
    }
}
