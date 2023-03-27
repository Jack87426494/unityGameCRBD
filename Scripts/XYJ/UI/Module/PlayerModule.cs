using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerModule
{
    private static PlayerModule instance;
    public static PlayerModule Instance
    {
        get
        {
            if(instance==null)
            instance = new PlayerModule();
            instance.GetInfo();
            return instance;
        }
    }

    private int lev=0;
    public int Lev => lev;
    private int hp=15;
    public int Hp => hp;
    private int atk=5;
    public int Atk => atk;
    private int def=1;
    public int Def => def;
    private int crit=1;
    public int Crit => crit;
    private int miss=1;
    public int Miss => miss;
    private int luck=1;
    public int Luck => luck;
    private int money=100;
    public int Money => money;
    private int gem=0;
    public int Gem => gem;
    private int power=0;
    public int Power => power;
    private string name;
    public string Name => name;

    private event UnityAction<PlayerModule> upGradeAction;

    public void UpGrade()
    {
        ++lev;
        hp += lev * 5;
        atk += lev * 2;
        def += lev;
        crit += lev;
        miss += lev;
        power = 100;
        upGradeAction(this);
        EventMgr.Instance.EventTrigger("UpGrade");
        SaveInfo();
    }

    public void AddUpGradeAction(UnityAction<PlayerModule> unityAction)
    {
        upGradeAction += unityAction;
    }
    public void RemoveUpGradeAction(UnityAction<PlayerModule> unityAction)
    {
        upGradeAction -= unityAction;
    }

    public void GetInfo()
    {
        lev=PlayerPrefs.GetInt("lev");
        hp=PlayerPrefs.GetInt("hp");
        atk=PlayerPrefs.GetInt("atk");
        def=PlayerPrefs.GetInt("def");
        crit=PlayerPrefs.GetInt("crit");
        miss=PlayerPrefs.GetInt("miss");
        luck=PlayerPrefs.GetInt("luck");
        money=PlayerPrefs.GetInt("money");
        gem=PlayerPrefs.GetInt("gem");
        power=PlayerPrefs.GetInt("power");
        name=PlayerPrefs.GetString("name");
    }
    public void SaveInfo()
    {
        PlayerPrefs.SetInt("lev", lev);
        PlayerPrefs.SetInt("hp", hp);
        PlayerPrefs.SetInt("atk", atk);
        PlayerPrefs.SetInt("def", def);
        PlayerPrefs.SetInt("crit", crit);
        PlayerPrefs.SetInt("miss", miss);
        PlayerPrefs.SetInt("luck", luck);
        PlayerPrefs.SetInt("money", money);
        PlayerPrefs.SetInt("gem", gem);
        PlayerPrefs.SetInt("power", power);
        PlayerPrefs.SetString("name", name);
    }
}
