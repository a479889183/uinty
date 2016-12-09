using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


/// <summary>
/// 出牌
/// </summary>
public class PlayCard : MonoBehaviour {
    /// <summary>
    /// 出的牌存储位置
    /// </summary>
    private Transform playTranform;
    /// <summary>
    /// 记数
    /// </summary>
    private int pCount=0;
    /// <summary>
    /// 结算对话框
    /// </summary>
    public Image over;
    //设置点击间隔
    private float time = 0f,showtime=0;
    //设置1秒钟点击一次 避免重复调用点击事件
    private float timer = 0.2f,showtimer=2F;
    //出牌按钮 
    public Button playCard;
    /// <summary>
    /// 埋牌按钮
    /// </summary>
    public  Button BuriedCard;
    /// <summary>
    /// 再来一局按钮
    /// </summary>
    public Button restart;
    //一个人打 还是和队友打
    public Button radio, select;
    //发牌管理类
    private player game;
    /// <summary>
    /// 存每次桌面上出的牌
    /// </summary>
    private List<GameObject> tempList=new List<GameObject>();
    /// <summary>
    /// 判断是否执行过
    /// </summary>
    private bool isRun = false;
    //是否显示埋牌按钮
    public  bool isShow=true;
    /// <summary>
    /// 是否获取鼠标移动的坐标点
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// 存储要出的牌
    /// </summary>
    private List<Card> plist=new List<Card>();

    //private Canvas canvas;
    //显示计分
    private GameObject textSocer;
    /// <summary>
    /// 开始触摸的点
    /// </summary>
    private Vector2 startPostion;
    /// <summary>
    /// 手指抬起来的点
    /// </summary>
    private Vector2 endPostion;
    /// <summary>
    /// 存储触摸事件的点
    /// </summary>
    private HashSet<Vector2> hasePostion = new HashSet<Vector2>();
    /// <summary>
    /// 计分牌
    /// </summary>
    public Text scoreText;

    /// <summary>
    /// 扑克模型
    /// </summary>
    [SerializeField]
    private GameObject poker;

    /// <summary>
    /// 设置埋牌按钮是否显示
    /// </summary>
    /// <param name="f"></param>
    public void IsShow(bool f) 
    {
        this.isShow = f;
    }
    /// <summary>
    /// 自己出牌
    /// </summary>
    /// <param name="people">第几个人</param>
    /// <param name="list">牌</param> 
    public void PlayCards(int people)
    {  
        if (playTranform == null) 
        {
            playTranform = new GameObject("playTranfrom").transform;
        }
        if (people ==0)
        {        
            //第一轮出牌 庄是自己 显示埋牌按钮
            if (isShow)
            {
                print("pcount====" + pCount);
               
                playCard.gameObject.SetActive(true);
            }
            
            else 
            {
                BuriedCard.gameObject.SetActive(true);
                AddCard(); 
                isShow = true;
            }       
        }
        else if (people == 1) 
        {
           

            fristCards(CardManager.Instance.fList);
            //pCount++;
        }
        else if (people == 2)
        {
           
            secondCards(CardManager.Instance.sList);
           // pCount++;
        }
        else if (people == 3)
        {     
            thirdCards(CardManager.Instance.tList);
            //pCount++;
        }
    }
    /// <summary>
    /// 出牌
    /// </summary>
    public void PlayPoker()
    {
        //print("谁出牌了：：" + CardManager.Instance.playCardPeople + "---大小：" + CardManager.Instance.olist.Count);
        //plist.Clear();
        //print("谁出牌了：：" + CardManager.Instance.playCardPeople + "---大小：" + CardManager.Instance.olist.Count+"   第几轮："+CardManager.Instance.round+"    出牌的人："+CardManager.Instance.zhuang+"   庄："+CardManager.Instance.robZhuang);
        if (pCount >=4) return;
        switch(CardManager.Instance.playCardPeople)
        {    //自己出牌
            case 0:
                playCard.gameObject.SetActive(true);
               // Playcardutils.Instance.playCards(CardManager.Instance.mList);
                break;
            //第一个人出牌
            case 1:
             //判断是否有此类型的牌
             bool  flag=PlayRules.Instance.isExist(CardManager.Instance.fList);
          
             List<Card> list=new List<Card>();
             if (flag)
             {
                 list= Playcardutils.Instance.playCards(CardManager.Instance.fList, CardManager.Instance.folist);
             }
             else 
             {
                 list = Playcardutils.Instance.playCards(CardManager.Instance.fList, CardManager.Instance.olist);
             }
            
             if (list != null &&list.Count > 0) 
             {
                 //判断牌是否大于桌面上的牌
                 if (PlayRules.Instance.isSize(list))
                 {
                     CardManager.Instance.olist.Clear();
                     foreach (Card item in list)
                     {
                         CardManager.Instance.olist.Add(item);
                     }                    
                     CardManager.Instance.zhuang = 1;
                 }
                 for (int x = 0; x < list.Count;x++ )
                 {
                     clonePoker(new Vector2(5.5f - (x * 0.5f), 0), list[x].pokername, list[x].name, list.Count-x);
                     //把出的牌从集合中删除
                     CardManager.Instance.fList.Remove(list[x]);
                     //添加分
                     CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(list[x]);
                 }   
             }
             else
             {   
                 list = new List<Card>();
                 list.Add(CardManager.Instance.fList[CardManager.Instance.fList.Count-1]);
                 // CardManager.Instance.sList.RemoveAt(CardManager.Instance.sList.Count - 1);
                 for (int x = 0; x < list.Count; x++)
                 {   //添加分到临时变量中
                     CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(list[x]);
                     clonePoker(new Vector2(5.5f - (x * 0.5f), 0), list[x].pokername, list[x].name, list.Count - x);
                     //把出的牌从集合中删除
                     CardManager.Instance.fList.Remove(list[x]);
                 }
             }            
             CardManager.Instance.playCardPeople = 2;
             //等1秒
             StartCoroutine(waitTime());
             //pCount++;
             //PlayPoker();
                break;
            //第二个人出牌
            case 2:
                //判断是否有此类型的牌
                bool sflag = PlayRules.Instance.isExist(CardManager.Instance.sList);

                List<Card> slist = new List<Card>();
                if (sflag)
                {
                    slist = Playcardutils.Instance.playCards(CardManager.Instance.sList, CardManager.Instance.folist);
                }
                else
                {
                    slist = Playcardutils.Instance.playCards(CardManager.Instance.sList, CardManager.Instance.olist);
                }
               
               // print("play:::第二个人出牌");
                if (slist != null && slist.Count > 0)
                {
                    //判断牌是否大于桌面上的牌
                    if (PlayRules.Instance.isSize(slist))
                    {
                        CardManager.Instance.olist.Clear();
                        foreach (Card item in slist)
                        {
                            CardManager.Instance.olist.Add(item);
                        }

                        CardManager.Instance.zhuang = 2;
                    }

                    for (int x = 0; x < slist.Count;x++ )
                    {   
                        clonePoker(new Vector2(0 + (x * 0.5f), 2.5f), slist[x].pokername, slist[x].name, x + 1);
                        //添加分
                        CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(slist[x]);
                        //把出的牌从集合中删除
                        CardManager.Instance.sList.Remove(slist[x]);
                    }     
                }
                else 
                {
                    slist = new List<Card>();
                    slist.Add(CardManager.Instance.sList[CardManager.Instance.sList.Count - 1]);
                    //CardManager.Instance.sList.RemoveAt(CardManager.Instance.sList.Count-1);
                    for (int x = 0; x < slist.Count; x++)
                    {
                        //添加分到临时变量中
                        CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(slist[x]);

                        clonePoker(new Vector2(0 + (x * 0.5f), 2.5f), slist[x].pokername, slist[x].name, x + 1);

                        CardManager.Instance.sList.Remove(slist[x]);
                    }
                }
                //pCount++;
                CardManager.Instance.playCardPeople = 3;
                //等1秒
                StartCoroutine(waitTime());
               // PlayPoker();
              break;
           //第三个人出牌
            case 3:
              //判断是否有此类型的牌
              bool tflag = PlayRules.Instance.isExist(CardManager.Instance.tList);
              
              List<Card> tlist = new List<Card>();
              //有此类型的牌   
              if (tflag)
              {
                  tlist = Playcardutils.Instance.playCards(CardManager.Instance.tList, CardManager.Instance.folist);
              }
              //没有第一次出牌类型的牌
              else
              {
                  tlist = Playcardutils.Instance.playCards(CardManager.Instance.tList, CardManager.Instance.olist);
              }

                if (tlist != null && tlist.Count > 0)
                {
                    for (int x = 0; x < tlist.Count;x++ )
                    {
                        clonePoker(new Vector2(-5.5f + (x * 0.5f), 0), tlist[x].pokername, tlist[x].name, x + 1);
                        //添加分
                        CardManager.Instance.tempScore+=Playcardutils.Instance.AddScore(tlist[x]);
                        //把牌删除
                        CardManager.Instance.tList.Remove(tlist[x]);
                    }
                    //判断牌是否大于桌面上的牌
                    if (PlayRules.Instance.isSize(tlist))
                    {
                        CardManager.Instance.olist.Clear();
                        foreach (Card item in tlist)
                        {
                            CardManager.Instance.olist.Add(item);
                        }

                        CardManager.Instance.zhuang = 3;
                    }
                }
                else 
                {   
                    tlist = new List<Card>();
                    tlist.Add(CardManager.Instance.tList[CardManager.Instance.tList.Count - 1]);
                   // CardManager.Instance.tList.RemoveAt(CardManager.Instance.tList.Count-1);
                    for (int x = 0; x < tlist.Count; x++)
                    {
                        //添加分到临时变量中
                        CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(tlist[x]);
                        clonePoker(new Vector2(-5.5f + (x * 0.5f), 0), tlist[x].pokername, tlist[x].name,x+1);

                        CardManager.Instance.tList.Remove(tlist[x]);
                    }
                }
                //pCount++;
                CardManager.Instance.playCardPeople = 0;
                //等1秒
                StartCoroutine(waitTime());
               // PlayPoker();
                break;
        }       
    }
    /// <summary>
    ///把出的牌添加到桌面上
    /// </summary>
    public void clonePoker(Vector2 vec,string pname,string name,int zord)
    {   //获取图片精灵
        Sprite tempType = new Sprite();
        tempType = Resources.Load("poker/" +pname, tempType.GetType()) as Sprite;

        //克隆poker模板
        GameObject game = GameObject.Instantiate(poker, vec, Quaternion.identity) as GameObject;
        // 添加poker的图片
        game.transform.GetComponent<SpriteRenderer>().sprite = tempType;
       //把牌加到指定目录中
        game.name = name;
        game.transform.GetComponent<SpriteRenderer>().sortingOrder = zord+1;
        game.transform.SetParent(playTranform);
        //把牌添加到临时变量中
        tempList.Add(game);
    } 
    /// <summary>
    /// 第一个人
    /// </summary>
    public void fristCards(List<Card> list)
    {  
        
        //获取图片精灵
        Sprite tempType = new Sprite();
        //集合为空自己返回
        if (list.Count == null || list.Count < 1) 
        {
            return;
        }
        tempType = Resources.Load("poker/" + list[list.Count - 1].pokername, tempType.GetType()) as Sprite;

        //克隆poker模板
        GameObject game = GameObject.Instantiate(poker, new Vector2(5.5f, 0), Quaternion.identity) as GameObject;
        // 添加poker的图片
        game.transform.GetComponent<SpriteRenderer>().sprite = tempType;

        //GameObject game = GameObject.Instantiate(list[list.Count - 1].poker, new Vector2(3, 0), Quaternion.identity) as GameObject;
        game.transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        game.name = list[list.Count - 1].name;
        game.transform.SetParent(playTranform);       
        List<Card> c = new List<Card>();
        c.Add(list[list.Count - 1]);
        //存入出的牌
        CardManager.Instance.olist.Clear();
        //先清空上次的牌
        CardManager.Instance.folist.Clear();
        //把分加入临时变量中  把出的牌存到olist中
        foreach (Card item in c)
        {    //添加分到临时变量中
            CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(item);
            CardManager.Instance.olist.Add(item);
            //把第一次出的牌放进去
            CardManager.Instance.folist.Add(item);
        }

        CardManager.Instance.fList.Remove(list[list.Count - 1]);
        CardManager.Instance.playCardPeople = 2;
        CardManager.Instance.zhuang = 1;
        //把出的牌加入到临时变量中
        tempList.Add(game);
        //等1秒
        StartCoroutine(waitTime());
       // PlayPoker();
    }
    /// <summary>
    /// 第二个人
    /// </summary>
    public void secondCards(List<Card> list)
    {

        //集合为空自己返回
        if (list.Count == null || list.Count < 1)
        {
            return;
        }
        //获取图片精灵
        Sprite tempType = new Sprite();

        tempType = Resources.Load("poker/" + list[list.Count - 1].pokername, tempType.GetType()) as Sprite;

        //克隆poker模板
        GameObject game = GameObject.Instantiate(poker, new Vector2(0, 2.5f), Quaternion.identity) as GameObject;
        // 添加poker的图片
        game.transform.GetComponent<SpriteRenderer>().sprite = tempType;

        //GameObject game=GameObject.Instantiate(list[list.Count - 1].poker, new Vector2(0, 2), Quaternion.identity) as GameObject;
        game.transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        game.name = list[list.Count - 1].name;
        game.transform.SetParent(playTranform);
        List<Card> c = new List<Card>();
        c.Add(list[list.Count - 1]);
        //存入出的牌
        CardManager.Instance.olist.Clear();
        CardManager.Instance.folist.Clear();
        foreach (Card item in c)
        {   //添加分到临时变量中
            CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(item);
            CardManager.Instance.olist.Add(item);
            //把第一次出的牌放进去
            CardManager.Instance.folist.Add(item);
        }
        CardManager.Instance.sList.Remove(list[list.Count - 1]);
        CardManager.Instance.playCardPeople = 3;
        CardManager.Instance.zhuang = 2;
        //把出的牌加入到临时变量中
        tempList.Add(game);
        //等1秒
        StartCoroutine(waitTime());
        //PlayPoker();
    }
    /// <summary>
    /// 第三人
    /// </summary>
    public void thirdCards(List<Card> list)
    {
        //集合为空自己返回
        if (list.Count == null || list.Count < 1)
        {
            return;
        }
        //获取图片精灵
        Sprite tempType = new Sprite();
        tempType = Resources.Load("poker/" + list[list.Count - 1].pokername, tempType.GetType()) as Sprite;

        //克隆poker模板
        GameObject game = GameObject.Instantiate(poker, new Vector2(-5.5f, 0), Quaternion.identity) as GameObject;
        // 添加poker的图片
        game.transform.GetComponent<SpriteRenderer>().sprite = tempType;

       // GameObject game = GameObject.Instantiate(list[list.Count - 1].poker, new Vector2(-3, 0), Quaternion.identity)as  GameObject;
        game.transform.GetComponent<SpriteRenderer>().sortingOrder = 2;
        game.name = list[list.Count - 1].name;
        game.transform.SetParent(playTranform);

        List<Card> c = new List<Card>();
        c.Add(list[list.Count - 1]);
        //存入出的牌
        CardManager.Instance.olist.Clear();
        CardManager.Instance.folist.Clear();
        foreach (Card item in c)
        {   //添加分到临时变量中
            CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(item);
            CardManager.Instance.olist.Add(item);
            //把第一次出的牌放进去
            CardManager.Instance.folist.Add(item);
        }
        CardManager.Instance.tList.Remove(list[list.Count - 1]);
        print("删除出的牌" + CardManager.Instance.tList.Count);
        CardManager.Instance.playCardPeople = 0;
        CardManager.Instance.zhuang = 3;
        //把出的牌加入到临时变量中
        tempList.Add(game);
        //等1秒
        StartCoroutine(waitTime());        
    }
    /// <summary>
    /// 等待1秒
    /// </summary>
    /// <returns></returns>
    IEnumerator waitTime()
    {  

        yield return new WaitForSeconds(2);
        isRun = false;
        pCount++;
        PlayPoker();
        
    }
    public CardUtils cd;
    /// <summary>
    /// 初始化出牌的点击事件
    /// </summary>
    void Start()
    {   //通过标签拿到对象
       // scoreText = GameObject.FindWithTag("score") as Text;
       // canvas = GetComponent<Canvas>();   
        cd = CardUtils.GetInstance();
        game = gameObject.GetComponent<player>();
        //出牌点击事件
        playCard.onClick.AddListener(delegate()
        {        
                playCardOnClick();
                       
        });
        //再来一局按钮
        restart.onClick.AddListener(delegate() 
        {
            restart.gameObject.SetActive(false);
            game.SendMessage("RestartGame");
        });
        //埋牌点击事件
        BuriedCard.onClick.AddListener(delegate() 
        {
            BuriecdCard();       
        });
        //一打三
        radio.onClick.AddListener(delegate() 
        {   //设置两边的队友
            CardManager.Instance.banker.Add(0);
            CardManager.Instance.civilians.Add(1);
            CardManager.Instance.civilians.Add(2);
            CardManager.Instance.civilians.Add(3);
            //隐藏找朋友按钮
            radio.gameObject.SetActive(false);
            select.gameObject.SetActive(false);
            //显示出牌按钮
            playCard.gameObject.SetActive(true);
        });
        //找朋友
        select.onClick.AddListener(delegate() 
        {
          

          List<int> count = new List<int>();
          count.Add(PlayRules.Instance.findFriends(CardManager.Instance.mList)+PlayRules.Instance.findFriends(CardManager.Instance.blist));
          count.Add(PlayRules.Instance.findFriends(CardManager.Instance.fList));
          count.Add(PlayRules.Instance.findFriends(CardManager.Instance.sList));
          count.Add(PlayRules.Instance.findFriends(CardManager.Instance.tList));
          //默认两个6不成对
          bool f=false;
          //2个主6在哪个人手里
          //  int pdouble=-1;
          //查找主6 是否成对
          for (int i = 0; i < count.Count; i++) 
          {
              if (count[i] == 2) 
              {   
                  f = true;
               
              }
          }
         //主6成对
          if (f)
          {
              CardManager.Instance.banker.Add(0);
              CardManager.Instance.banker.Add(2);
              CardManager.Instance.civilians.Add(1);
              CardManager.Instance.civilians.Add(3);
          }
          //主6没成对
          else
          {   //存有6的
              List<int> list = new List<int>();
              //存没有6的
              List<int> nlist = new List<int>();
              //找出6在那个人手里
              for (int i = 0; i < count.Count; i++) 
              {
                  if (count[i] >= 0) 
                  {
                      list.Add(i);
                  } 
                  else
                  {
                      nlist.Add(i);
                  }
              }
              //自己没有6
              if (count[0] == 0) 
              {
                  foreach (int item in nlist) 
                  {
                      CardManager.Instance.banker.Add(item);
                  }

                  foreach (int item in list) 
                  {
                      CardManager.Instance.civilians.Add(item);
                  } 
              }
               //自己有6
              else if(count[0]==1)
              {
                  foreach (int item in list)
                  {
                      CardManager.Instance.banker.Add(item);
                  }

                  foreach (int item in nlist)
                  {
                      CardManager.Instance.civilians.Add(item);
                  } 
              }
          }
          //隐藏找朋友按钮
          radio.gameObject.SetActive(false);
          select.gameObject.SetActive(false);
          //显示出牌按钮
          playCard.gameObject.SetActive(true);
        });
    }
    /// <summary>
    /// 监听触摸事件
    /// </summary>
    void Update()
    {
        //牌已发完
        if (CardManager.Instance.isSend) 
        {   //是否可点击       
                //鼠标按下
              if (Input.GetMouseButtonDown(0)) 
              {
                     isMove = true;
                     startPostion = Input.mousePosition;
                 //print("--" + Input.mousePosition.x + "---y:" + Input.mousePosition.y);
              }
              if (isMove) 
              {    //存储移动的点
                  hasePostion.Add(Input.mousePosition);
                  //print("move--" + Input.mousePosition.x + "---y:" + Input.mousePosition.y);
              }
         
            //鼠标抬起
            if (Input.GetMouseButtonUp(0))
            {
                isMove = false;
                endPostion = Input.mousePosition;
                //按下的点与抬起的点一致 就是单击事件
                if (startPostion.x == endPostion.x && startPostion.y == endPostion.y)
                {
                    CardOnclick(startPostion);
                }
                //移动事件
                else 
                {
                    PokerSelect(hasePostion);
                }
                hasePostion.Clear();
                //print("tai--" + Input.mousePosition.x + "---y:" + Input.mousePosition.y);
               
            }
        }

        //第一轮出牌结束清空桌面上的牌
        if (pCount >= 4)
        {
            time += Time.deltaTime;
            if (time > showtime)
            {
                time = 0;
                StartCoroutine(wait());
            }

        }
    }
    /// <summary>
    /// 扑克牌的单击事件
    /// </summary>
    public void CardOnclick(Vector2 position)
    {   //单击事件
        PokerOnclick(findByPositonOrName(position));  
    }
    /// <summary>
    /// 扑克的移动选牌事件
    /// </summary>
    public void PokerSelect(HashSet<Vector2> hasePostion) 
    {
        HashSet<string> hashName = new HashSet<string>();
        //先找到 碰到牌的name
        foreach (Vector2 item in hasePostion) 
        {
            hashName.Add(findByPositonOrName(item));
        }

        foreach (string item in hashName) 
        {
            PokerOnclick(item);
        }

    }
    /// <summary>
    /// 根据坐标点获取 碰撞的物体name
    /// </summary>
    /// <returns></returns>
    public string findByPositonOrName(Vector2 position)
    {
        string name = "";
        //获取鼠标点击在那个
        Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(position));

        if (col.Length > 0)
        {

            //col只有一个值
            if (col.Length == 1)
            {
                name = col[0].name;
            }
            else
            {
                //取Order调用点击事件
                for (int i = 0; i < col.Length - 1; i++)
                {
                    int z = GameObject.Find(col[i].name).transform.GetComponent<SpriteRenderer>().sortingOrder;
                    int z1 = GameObject.Find(col[i + 1].name).transform.GetComponent<SpriteRenderer>().sortingOrder;
                    if (z > z1)
                    {
                        name = col[i].name;
                    }
                    else
                    {
                        name = col[i + 1].name;
                    }
                }
            }

        }

        return name;
    }
    /// <summary>
    /// 等2秒
    /// </summary>
    /// <returns></returns>
    IEnumerator wait() 
    {
        yield return new WaitForSeconds(1);
        if (!isRun)
        {  
            removeAllLastPoker();
            isRun = true;
            showtime = 0;
            pCount = 0;
            CardManager.Instance.olist.Clear();
            CardManager.Instance.folist.Clear();
            print("第：" + (++CardManager.Instance.round) + "轮结束后----出牌的人：" + CardManager.Instance.zhuang + "   庄：" + CardManager.Instance.robZhuang + "  CardManager.Instance.banker.Count :" + CardManager.Instance.banker.Count);
            //一打三
            if (CardManager.Instance.banker.Count == 1)
            {
                if (CardManager.Instance.zhuang != CardManager.Instance.robZhuang)
                {
                    CardManager.Instance.Score += CardManager.Instance.tempScore;
                }
            }
            //二打二 
            else if (CardManager.Instance.banker.Count == 2)
            {
                //如果平民的牌大 把分加进去
                if (CardManager.Instance.zhuang != 0 || CardManager.Instance.zhuang != 2)
                {
                    CardManager.Instance.Score += CardManager.Instance.tempScore;
                }
            }
            scoreText.text = CardManager.Instance.Score+"";
            CardManager.Instance.tempScore = 0;
            //牌全部出完了
            if (CardManager.Instance.mList == null || CardManager.Instance.mList.Count == 0) 
            {
                restart.gameObject.SetActive(true);
                playCard.gameObject.SetActive(false);
                over.gameObject.SetActive(true);

            }
            PlayCards(CardManager.Instance.zhuang);
            //StartCoroutine(wait());
        }

        
    }

    /// <summary>
    /// 埋牌
    /// </summary>
    public void BuriecdCard()
    {
       // print("点击了埋牌"+plist.Count);
        if (plist.Count == 8)
        {   //先清空底牌
            CardManager.Instance.blist.Clear();
            //把埋的牌添加到底牌中
            foreach (Card item in plist) 
            {
                CardManager.Instance.blist.Add(item);
                Destroy(GameObject.Find(item.name));
                CardManager.Instance.mList.Remove(item);
            }
            //发个消息重新排序
            game.SendMessage("sortPoker");
            plist.Clear();
            BuriedCard.gameObject.SetActive(false);
            //playCard.gameObject.SetActive(true);
            //埋完牌后 显示找朋友的按钮
            radio.gameObject.SetActive(true);
            select.gameObject.SetActive(true);
            isShow = true;
        }     
    }
    /// <summary>
    /// 加入底牌
    /// </summary>
    public void AddCard() 
    {   //把底牌加入自己的牌中
        foreach (Card item in CardManager.Instance.blist) 
        {
            game.SendMessage("addPoker", item);
           
        }
        foreach (Card item in CardManager.Instance.blist)
        { //把底牌往上移
          upCard(GameObject.Find(item.name),-3.5f);
          //把上移的牌加入集合中
          plist.Add(item);
        }
        
    }
    /// <summary>
    /// 把扑克牌往上移
    /// y为上移的坐标点
    /// </summary>
    /// <param name="game"></param>
    void upCard(GameObject game,float y) 
    {   //先获取物体本身的坐标
        float x = game.transform.position.x;
        game.transform.position = new Vector2(x, y);
    }

    /// <summary>
    ///扑克牌的点击事件
    /// </summary>
    /// <param name="name">点击扑克的name</param>
    public void PokerOnclick(string name)
    {   //是否可点击
        bool isOnclick = false;
        //print("dingjile:----::::    "+name);
        foreach (Card item in CardManager.Instance.mList) 
        {
            if (name.Equals(item.name))
            {
                isOnclick = true;
            }
        }
        if (!isOnclick) return;
        //print("pokerOnclick:  "+name);
        if ("".Equals(name)) return;
        float x = GameObject.Find(name).transform.position.x;
        float y = GameObject.Find(name).transform.position.y;
        if (y < -3.5f)
        {   //把牌往上移  
            GameObject.Find(name).transform.position = new Vector2(x, -3.5F);
            //把牌加到集合里面
            plist.Add(Playcardutils.Instance.getByName(CardManager.Instance.mList, name));
        }
        else
        {   //把牌往下移
            GameObject.Find(name).transform.position = new Vector2(x, -4F);
            //把牌从集合中删除
            plist.Remove(Playcardutils.Instance.getByName(CardManager.Instance.mList, name));
        }
        time = 0;
    }
    /// <summary>
    /// 出牌按钮点击事件
    /// </summary>
    public void playCardOnClick() 
    {   
        if (plist.Count == 0 || plist == null) return;      
       // print("点击了出牌:"+pCount+"  plist::"+plist.Count);
        //不是自己出牌 出的牌与桌面上的牌不一致 直接返回
        if (pCount != 0)
        {
            if (plist.Count != CardManager.Instance.olist.Count) return;
        }
        //设置没有运行
        isRun = false;
        //判断是否符合出牌规则
       bool f= PlayRules.Instance.isPlayRoles(plist);
       if (f) 
       {
           bool isPlay=PlayRules.Instance.isPlayCard(plist,CardManager.Instance.mList);
           if (isPlay) 
           {
               bool f1 = PlayRules.Instance.isSize(plist);
               //牌大于桌面上的牌 把桌面上的牌替换成自己的牌
               if (f1)
               {
                   CardManager.Instance.olist.Clear();

                   foreach (Card item in plist)
                   {
                       CardManager.Instance.olist.Add(item);
                   }

                   CardManager.Instance.zhuang = 0;
               }
               else if (pCount == 0)
               {
                   CardManager.Instance.olist.Clear();
                   CardManager.Instance.folist.Clear();
                   foreach (Card item in plist)
                   {  //桌面上的牌
                       CardManager.Instance.olist.Add(item);
                       //存第一次出的牌
                       CardManager.Instance.folist.Add(item);
                   }
                   CardManager.Instance.zhuang = 0;
               }

               //计算出牌的位置
               if (plist.Count > 0)
               {
                   playPosition(plist);
               }
               //发个消息重新排序
               game.SendMessage("sortPoker");
               if (pCount < 3)
               {
                  // print("pCoun:::" + pCount + "--第几轮：" + CardManager.Instance.round);
                   // pCount++;
                   CardManager.Instance.playCardPeople = 1;
                   //等2秒
                   StartCoroutine(waitTime());
               }
               else if (pCount == 3)
               {
                   //print("pCount==3 --第几轮：" + CardManager.Instance.round);
                   pCount++;
               }
               Debug.Log("桌面上的牌大小："+CardManager.Instance.olist.Count+"第一次出牌的大小："+CardManager.Instance.folist.Count);
               playCard.gameObject.SetActive(false);
           }        
       }

       
       
    }
    /// <summary>
    ///计算自己出牌的位置
    /// </summary>
    /// <returns></returns>
    void playPosition(List<Card> list) 
    {
       int index = list.Count / 2;

        for (int x = 0; x < list.Count; x++)
        {
            float h = 0;
            if (x < index)
            {
                h = (index - x) * (-0.3F);
            }
            else if (x == index)
            {
                h = 0;
            }
            else if (x > index)
            {
                h = (x - index) * 0.3F;
            }

            //添加分
            CardManager.Instance.tempScore += Playcardutils.Instance.AddScore(list[x]);
            //计算牌的位置
           GameObject.Find(plist[x].name).transform.position = new Vector2(h, 0);
           
           GameObject.Find(plist[x].name).transform.GetComponent<SpriteRenderer>().sortingOrder = x+1;
           print("2张以上的牌");  
           tempList.Add(GameObject.Find(plist[x].name));
           //把牌从集合中删除
           CardManager.Instance.mList.Remove(plist[x]);
        }
        plist.RemoveAll(it => true);  
    }
    /// <summary>
    /// 清除上一轮出的牌
    /// </summary>
    void removeAllLastPoker()
     {
         print("remoeAll----");
        foreach (GameObject game in tempList) 
        {   //把牌干掉
            Destroy(game);
        }
        tempList.Clear();
    }
}
