using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class player : MonoBehaviour {
    /// <summary>
    /// 准备按钮
    /// </summary>
    public Button read;
    //扑克存放目录
    private Transform pokerTrans;

    private CardManager cm;
    /// <summary>
    /// 是否开始发牌
    /// </summary>
    public bool isSend = false;
    //计时器
    private float time = 0f;
    //发牌速度
    private float speed = 0.1f;
    //发牌的数量标识
    private int pnumber = 0;
    [SerializeField]
    private GameObject pt;
    /// <summary>
    /// 扑克模型
    /// </summary>
    [SerializeField]
    private GameObject poker;
    /// <summary>
    /// 准备按钮
    /// </summary>
    private GameObject[] ok;

    private List<Card> plist = new List<Card>();
	void Start () {
        ok = GameObject.FindGameObjectsWithTag("ok");
        pokerTrans = pt.transform;
        cm = CardManager.Instance;
        read.onClick.AddListener(delegate() 
        {
            read.gameObject.SetActive(false);
            readButtonOnClick();
        });
      
	}
	
	// Update is called once per frame
	void Update () {
        //发牌标识
        if (isSend)
        {
            time += Time.deltaTime;

            if (time > speed)
            {
                time = 0f;
                //巴十发牌
                if(cm.type==1)
                {
                    dealBs();  
                }
                pnumber++; 
            }
        }
	}
    /// <summary>
    /// 巴十 发牌
    /// </summary>
    /// <param name="number"></param>
    void dealBs() 
    {
        if (pnumber >= plist.Count - 8)
        {
            //对牌进行排序
            CardManager.Instance.fList = CardUtils.GetInstance().sortlist(CardManager.Instance.fList, CardManager.Instance.color);

            CardManager.Instance.sList = CardUtils.GetInstance().sortlist(CardManager.Instance.sList, CardManager.Instance.color);

            CardManager.Instance.tList = CardUtils.GetInstance().sortlist(CardManager.Instance.tList, CardManager.Instance.color);

            //添加底牌
            for (int i = plist.Count - 8; i < plist.Count; i++)
            {
                //print("值:" +list[i].value + "--花色：" + list[i].color+"---i="+i);
                CardManager.Instance.blist.Add(plist[i]);
            } 

            isSend = false;
            //标记牌已经发完
            CardManager.Instance.isSend = true;
            return;
        }
        //自己的牌
        if (pnumber % 4 == 0) 
        {
            addPoker(plist[pnumber]);
        }
         //第二个人的牌
        else if (pnumber % 4 == 1)
        {
            cm.fList.Add(plist[pnumber]);
            //把抢庄的牌加入集合
            if (plist[pnumber].value == 10 && (plist[pnumber].color == 1 || plist[pnumber].color == 3))
            {
                cm.fRob.Add(new Poker(plist[pnumber].name, 2, plist[pnumber].color));
            }
            else if (plist[pnumber].value == 11)
            {
                cm.fRob.Add(new Poker(plist[pnumber].name, 1, plist[pnumber].color));
            }
        }
        //第三个人的牌
        else if (pnumber % 4 == 2)
        {
            cm.sList.Add(plist[pnumber]);
            //把抢庄的牌加入集合
            if (plist[pnumber].value ==10 && (plist[pnumber].color == 1 || plist[pnumber].color == 3))
            {
                cm.sRob.Add(new Poker(plist[pnumber].name, 2, plist[pnumber].color));
            }
            else if (plist[pnumber].value == 11)
            {
                cm.sRob.Add(new Poker(plist[pnumber].name, 1, plist[pnumber].color));
            }
        }
        //第四个人的牌
        else if (pnumber % 4 == 3)
        {
            cm.tList.Add(plist[pnumber]);
            //把抢庄的牌加入集合
            if (plist[pnumber].value == 10 && (plist[pnumber].color == 1 || plist[pnumber].color == 3))
            {
                cm.tRob.Add(new Poker(plist[pnumber].name, 2, plist[pnumber].color));
            }
            else if (plist[pnumber].value == 11)
            {
                cm.tRob.Add(new Poker(plist[pnumber].name, 1, plist[pnumber].color));
            }
        }

        print(plist[pnumber].ToString());
    }

    /// <summary>
    /// 准备按钮点击事件
    /// </summary>
    void readButtonOnClick()
    {
        CardManager.Instance.isShowRob = true;
        foreach (GameObject item in ok) 
        {
            item.SetActive(false);
        }
        //获取牌
        plist = CardUtils.GetInstance().getPoker(2, 54, 1);
        //洗牌
        plist= CardUtils.GetInstance().Shuffle(plist);
        //标记开始发牌
        isSend = true;
    }
    /// <summary>
    /// 把牌加入到桌面上
    /// </summary>
    void addPoker(Card card)
    {   //克隆poker模板
        GameObject game = GameObject.Instantiate(poker, new Vector2(0, -4f), Quaternion.identity) as GameObject;
        game.name = "poke" + CardManager.Instance.mList.Count;
       //获取图片精灵
        Sprite tempType = new Sprite();
        tempType = Resources.Load("poker/" + card.pokername, tempType.GetType()) as Sprite;
        // 添加poker的图片
        game.transform.GetComponent<SpriteRenderer>().sprite =tempType;
        //把克隆出来的牌对象放到同一目录下
        game.transform.SetParent(pokerTrans);
        //把牌加到集合中
        card.name = "poke" + CardManager.Instance.mList.Count;
        //把牌加到集合中
        cm.mList.Add(card);
        //把抢庄的牌加到集合中
        if (card.value == 10 && ( card.color == 1 || card.color == 3))
        {
            cm.mRob.Add(new Poker(card.name, 2, card.color));

        }
        else if (card.value == 11)
        {
            cm.mRob.Add(new Poker(card.name, 1, card.color));
        }
        sortPoker();
    }
    /// <summary>
    /// 显示的牌进行排序
    /// </summary>
    void sortPoker()
    {
        List<Card> cl = CardUtils.GetInstance().sortlist(cm.mList, CardManager.Instance.color);
        // print("原来大小："+cl.Count);
        HashSet<Card> set = new HashSet<Card>(cl);

        // print("去除重复数据大小：" + set.Count);
        cm.mList.Clear();
        cm.mList = cl;
        int index = cm.mList.Count / 2;
        //计算牌的位置
        for (int x = 0; x < cm.mList.Count; x++)
        {

            float h = 0;
            if (x < index)
            {
                h = (index - x) * (-0.6F);
            }
            else if (x == index)
            {
                h = 0;
            }
            else if (x > index)
            {
                h = (x - index) * 0.6F;
            }

            // print("x:  " + x + "  h:" + h+"   ---index:"+index+"---name:"+cm.mList[x].name);
            //计算牌的位置
            pokerTrans.FindChild(cm.mList[x].name).transform.position = new Vector2(h, -4);

            pokerTrans.FindChild(cm.mList[x].name).GetComponent<SpriteRenderer>().sortingOrder = x + 1;

        }
    }
   /// <summary>
   /// 重新开始
   /// </summary>
    void RestartGame()
    {  //把牌恢复默认
        CardManager.Instance.Restore();
        pnumber = 0;   
     SceneManager.LoadScene("player");
    }
}
