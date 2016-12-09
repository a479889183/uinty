using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJson;
using LitJson;
using UnityEngine.SceneManagement;

public class Launch : MonoBehaviour {
    /// <summary>
    /// 进度条
    /// </summary>
    public Scrollbar scrollbar;

    public Image Login;
    /// <summary>
    /// 登录按钮
    /// </summary>
    public Button commit;

    public InputField username, password;

    private float time = 0f;

    private float timer = 3f;

    private bool isfrist = true;

	void Start () {
        
        commit.onClick.AddListener(delegate() 
        {
            StartCoroutine(initData());
        });
	}
	

	void Update () {

        time += Time.deltaTime;
        if (time < timer)
        {
            scrollbar.GetComponent<Scrollbar>().size = (float)time / 3;
        }
        else if(isfrist) 
        {
            isfrist = false; 
            loadApp();
            return;
        }
	}
    /// <summary>
    ///切换下一个场景
    /// </summary>
    void loadApp() 
    {
        //Application.LoadLevel("start");

        Login.gameObject.SetActive(true);
    }
    /// <summary>
    /// 初始对话框数据
    /// </summary>
    IEnumerator initData() 
    {
      string user= username.text;

      string pwd = password.text;

      string URL = "http://192.168.1.174:8080/Login/Login?";

     string url=URL + "username=" + user + "&password=" + pwd;

     print(url);
      WWW ww = new WWW(url);

      yield return ww;

      if (ww.error != null)
      {

      }
      else
      {
          print(ww.text);
        string code = ww.text;
        JsonData js = JsonMapper.ToObject(code);


        print("string:" + js["code"]);
        if (js["code"].Equals("1"))
          {   
              SceneManager.LoadScene("start");
          }
          else 
          {
          print("登录失败");
          }
      }

    }
    
}
