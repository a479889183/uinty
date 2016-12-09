using UnityEngine;
using System.Collections;
/// <summary>
/// 牌的属性类
/// </summary>
public class Card  {
    /// <summary>
    /// 牌的大小
    /// </summary>
    public int value;
    /// <summary>
    /// 牌的花色
    /// </summary>
    public int color;
    /// <summary>
    /// 图片名称
    /// </summary>
    public string pokername;
    /// <summary>
    /// 牌的值
    /// </summary>
    public int actualValue;
    /// <summary>
    /// 牌的名字
    /// </summary>
    public string name;

    /// <summary>
    /// 设置牌的大小
    /// </summary>
    /// <param name="value"></param>
    public void setValue(int value) 
    {
        this.value = value;
    }
    /// <summary>
    /// 获取牌的大小
    /// </summary>
    /// <returns></returns>
    public int getValue() 
    {
        return this.value;
    }

    public void setColor(int color) 
    {
        this.color = color;
    }

    public int getColor() 
    {
        return this.color;
    }


    public void setPoker(string pokername) 
    {
        this.pokername = pokername;
    }

    public string getPoker() 
    {
        return this.pokername;
    }

    public void setActualValue(int actualValue) 
    {
        this.actualValue = actualValue;
    }

    public int getActualValue() 
    {
        return this.actualValue;
    }

    public void setName(string name) 
    {
        this.name = name;
    }

    public string getName() 
    {
        return name;
    }
    public override string ToString()
    {
        string str = "牌：" + this.actualValue + "--牌的值：" + this.value + "--牌的花色：" + this.color + "--牌的名称：" + this.name+"--图片名字"+pokername+"---牌的value："+value;
        return str;
    }
}
