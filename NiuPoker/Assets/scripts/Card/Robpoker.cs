using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
/// <summary>
/// 抢庄类
/// </summary>
public class Robpoker : MonoBehaviour {
    //显示亮庄的控件
    public GameObject rob;
    // 显示主色控件
    public GameObject robPoker;

    private Transform robot;
    /// <summary>
    /// 亮庄类
    /// </summary>
    private RobShow robshow;
    /// <summary>
    /// 出牌类
    /// </summary>
    private PlayCard pcard; 
    /// <summary>
    /// 设置1秒钟执行点击事件 避免重复调用
    /// </summary>
    public float timer = 0;

    public float time = 1;
    //牌发完后停留10秒倒计时
    public float  Countdown = 10;

    public float timeTemp = 0;

    public Text text;
    /// <summary>
    /// 让出牌函数只调用一次
    /// </summary>
    public bool isPlay=false;
    /// <summary>
    /// 计时控件
    /// </summary>
    public GameObject CountObject;
    /// <summary>
    /// 亮庄花色控件
    /// </summary>
    public GameObject[] cObject;
    /// <summary>
    /// 数字控件
    /// </summary>
    [SerializeField]
    private GameObject snumber;
    /// <summary>
    /// 数字图片
    /// </summary>
    [SerializeField]
    private Sprite[] snum;
  /// <summary>
  /// 抢庄控件
  /// </summary>
    public  GameObject[] robObject;



    void Awake()
    {
        robshow = GetComponent<RobShow>();
        pcard = GetComponent<PlayCard>();
       
    }
    /// <summary>
    /// 显示亮庄控件
    /// 
    /// </summary>
    void showPoker(int color) 
    {
        switch (color) 
        {
            case 0:
                cObject[1].SetActive(true);
                break;
            case 1:
                cObject[2].SetActive(true);
                break;
            case 2:
                cObject[3].SetActive(true);
                break;
            case 3:
                cObject[4].SetActive(true);
                break;
        }
     // robPoker=GameObject.Instantiate(rob, new Vector2(0, -1), Quaternion.identity) as GameObject;
    }
    void LateUpdate() 
    {    //显示抢庄控件 
        if (CardManager.Instance.isShowRob) 
        {
            
            CardManager.Instance.isShowRob = false;
           //显示抢庄控件
           rob.SetActive(true);
           AddOnClick();             
        }

        timer += Time.deltaTime;
      
        if (isPlay) return;
        //牌发完后等5秒 不反庄就执行出牌动作
        if (CardManager.Instance.isSend) 
        {   
            timeTemp += Time.deltaTime;
            //显示倒计时控件
           
            //等10秒 
            if (timeTemp > Countdown)
            {    
                //显示倒计时控件
                CountObject.GetComponent<SpriteRenderer>().sortingOrder = -1;
                snumber.GetComponent<SpriteRenderer>().sortingOrder = -1;
                removeRArray();
                //把庄存起来
                CardManager.Instance.robZhuang = CardManager.Instance.zhuang;
                if (CardManager.Instance.zhuang == 0) 
                {  //庄是自己 设置拿底牌
                    pcard.SendMessage("IsShow", false);
                }
                //庄不是自己
                pcard.SendMessage("PlayCards", CardManager.Instance.zhuang);

                isPlay = true;                    
            }
            else
            {   //显示倒计时控件
                CountObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
                snumber.GetComponent<SpriteRenderer>().sortingOrder = 1;
                snumber.GetComponent<SpriteRenderer>().sprite = snum[9-(int)timeTemp];
            }
          
        }
    }
    /// <summary>
    /// 给抢庄按钮添加点击事件
    /// </summary>
    void AddOnClick() 
    {
        cObject[0].GetComponent<Button>().onClick.AddListener(delegate()
        {
            robOnClick(cObject[0].name);
        });
        cObject[1].GetComponent<Button>().onClick.AddListener(delegate()
        {
            robOnClick(cObject[1].name);
        });
        cObject[2].GetComponent<Button>().onClick.AddListener(delegate()
        {
            robOnClick(cObject[2].name);
        });
        cObject[3].GetComponent<Button>().onClick.AddListener(delegate()
        {
            robOnClick(cObject[3].name);
        });
        cObject[4].GetComponent<Button>().onClick.AddListener(delegate()
        {
            robOnClick(cObject[4].name);
        });
    }
    /// <summary>
    /// 删除桌面上的rArray
    /// </summary>
    void removeRArray() 
    {   //把抢庄控件隐藏
        rob.SetActive(false);

        foreach (GameObject item in robObject)
        {
            if (item.name.Equals("frist"))
            {
                item.SetActive(false);      
            }
            else if (item.name.Equals("second"))
            {
                item.SetActive(false); 
            }
            else if (item.name.Equals("thrid"))
            {
                item.SetActive(false);
            }
        }
    }
    /// <summary>
    /// 抢庄的点击事件
    /// </summary>
    void robOnClick(string name) 
    {
        switch (name) 
        {   //没有花色
            case "no": 
                
                break;
             //黑桃
            case "black":
                  CardManager.Instance.color = 0;
                  CardManager.Instance.zhuang = 0;
                //找到图片资源
                 Sprite tempType = new Sprite();
                 tempType = Resources.Load("rob/bg_small_suit_4", tempType.GetType()) as Sprite;
                //替换主牌花色
                 foreach (GameObject item in robObject)
                 {
                     
                     if (item.name.Equals("color"))
                     {   
                         item.SetActive(true);
                         item.transform.GetComponent<Image>().sprite = tempType;
                     }
                 }
                
                break;
            //红桃
            case "red":
                CardManager.Instance.color = 1;
                CardManager.Instance.zhuang = 0;
                //找到图片资源
                Sprite tempType1 = new Sprite();
                tempType1 = Resources.Load("rob/bg_small_suit_3", tempType1.GetType()) as Sprite;
                //替换主牌花色
                 foreach (GameObject item in robObject)
                 {
              
                     if (item.name.Equals("color"))
                     {
                         item.SetActive(true);
                         item.transform.GetComponent<Image>().sprite = tempType1;
                     }
                 }
                break;
            //梅花
            case "meihua":
                CardManager.Instance.color = 2;
                CardManager.Instance.zhuang = 0;
                //找到图片资源
                Sprite tempType2 = new Sprite();
                tempType2 = Resources.Load("rob/bg_small_suit_2", tempType2.GetType()) as Sprite;
                //替换主牌花色
                foreach (GameObject item in robObject)
                {               
                    if (item.name.Equals("color"))
                    {
                        print("meihua11");
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = tempType2;
                    }
                }
                break;
            //方块
            case "fangkuai":
                CardManager.Instance.color = 3;
                CardManager.Instance.zhuang = 0;
                //找到图片资源
                Sprite tempType3 = new Sprite();
                tempType3 = Resources.Load("rob/bg_small_suit_1", tempType3.GetType()) as Sprite;
                //替换主牌花色
                foreach (GameObject item in robObject)
                {

                    if (item.name.Equals("color"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = tempType3;
                    }
                }
                break;
        
        }
    }

    /// <summary>
    /// 恢复默认的精灵
    /// </summary>
    void resetDefault()
    {
        print("恢复默认花色");
        foreach (GameObject item in cObject)
        {      
                item.SetActive(false);    
        }

    }
    /// <summary>
    /// 显示机器亮庄的花色
    /// </summary>
    void showRobPoker(object[] value)
    {  
        int x = (int)value[0];

        int num = (int)value[1];

        //黑桃
        if (x == 0)
        {      //获取图片精灵
            Sprite tempType = new Sprite();
            tempType = Resources.Load("rob/bg_small_suit_4", tempType.GetType()) as Sprite;
            showRop(tempType, num);
            CardManager.Instance.color = 0;
          
        }
        //红桃 
        else if (x == 1)
        {   //获取图片精灵
            Sprite tempType = new Sprite();
            tempType = Resources.Load("rob/bg_small_suit_3", tempType.GetType()) as Sprite;
            showRop(tempType, num);
            CardManager.Instance.color = 1;
            // cgame.getcomponent<spriterenderer>().sprite = spoker[1];
        }
        //梅花
        else if (x == 2)
        {         //获取图片精灵
            Sprite tempType = new Sprite();
            tempType = Resources.Load("rob/bg_small_suit_2", tempType.GetType()) as Sprite;
            showRop(tempType, num);
            CardManager.Instance.color = 2;
            // cgame.getcomponent<spriterenderer>().sprite = spoker[2];
        }
        //方块
        else if (x == 3)
        {    //获取图片精灵
            Sprite tempType = new Sprite();
            tempType = Resources.Load("rob/bg_small_suit_1", tempType.GetType()) as Sprite;
            showRop(tempType, num);
            CardManager.Instance.color = 3;
            //cgame.getcomponent<spriterenderer>().sprite = spoker[3];
        }
    }

    /// <summary>
    /// 显示其他人亮庄的花色
    /// </summary>
    /// <param name="sprite"></param>
    void showRop(Sprite sprite,int num) 
    {   //显示亮庄的控件
        switch (num) 
        {
            case 1:
                foreach (GameObject item in robObject) 
                {
                    if (item.name.Equals("frist"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                    if (item.name.Equals("color"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                }
                break;

            case 2:
                foreach (GameObject item in robObject)
                {
                    if (item.name.Equals("second"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                    if (item.name.Equals("color"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                }
                
                break;

            case 3:
                foreach (GameObject item in robObject)
                {
                    if (item.name.Equals("thrid"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                    if (item.name.Equals("color"))
                    {
                        item.SetActive(true);
                        item.transform.GetComponent<Image>().sprite = sprite;
                    }
                }
                
                break;
        }
    }
}
