using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Over : MonoBehaviour {

    public Button close;

    public Text score;

	void Start () {

        close.onClick.AddListener(delegate() 
        {
            this.gameObject.SetActive(false);
        });
	}
	
	// Update is called once per frame
	void Update () {
   
        if (score != null)
        {
          
            score.text = "分数：" + CardManager.Instance.Score + "";
        }
	}
}
