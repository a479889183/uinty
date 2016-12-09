using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class mainStart : MonoBehaviour {
    /// <summary>
    /// 进入房间(1)、创建房间(2)、金币场按钮(0)
    /// </summary>
    public Button []array;
 
	void Start () {

        addOnclik();
	}
    /// <summary>
    /// 添加点击事件
    /// </summary>
    void addOnclik() 
    {
        //添加点击事件
        array[0].onClick.AddListener(delegate()
        {
            ButtonOnclick(0);

        });
        array[1].onClick.AddListener(delegate()
        {
            ButtonOnclick(1);

        });
        array[2].onClick.AddListener(delegate()
        {
            ButtonOnclick(2);

        });

        array[3].onClick.AddListener(delegate()
        {
            ButtonOnclick(0);

        });
        array[4].onClick.AddListener(delegate()
        {
            ButtonOnclick(1);

        });
        array[5].onClick.AddListener(delegate()
        {
            ButtonOnclick(2);

        });

    
    }
	
	void Update () {
	
	}
    /// <summary>
    /// 点击事件处理
    /// </summary>
    /// <param name="x"></param>
    void ButtonOnclick(int x) 
    {
        switch (x) 
        {
            case 0:
                print("dianjile :"+x);
                break;
            case 1:
            SceneManager.LoadScene("player");
                break;
            case 2:
                print("dianjile :" + x);
                break;
        }
      
    }
}
