using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{

    public GameObject MainPanel;     //初始面板
    public GameObject StartPanel;    //开始界面面板
    public GameObject InfoPanel;     //游戏信息面板

    // Use this for initialization
    void Start()
    {
        ActiveMainPanel();  //调用ActiveMainPanel函数，启用初始面板，禁用其他面板
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}

    //启用初始面板，禁用“游戏信息”面板
    public void ActiveMainPanel()
    {
        MainPanel.SetActive(true);
        StartPanel.SetActive(false);
        InfoPanel.SetActive(false);
    }

    //启用“开始界面”面板，禁用初始面板
    public void ActiveStartPanel()
    {
        MainPanel.SetActive(false);
        StartPanel.SetActive(true);
        InfoPanel.SetActive(false);
    }

    //启用“游戏信息”面板，禁用初始面板
    public void ActiveInfoPanel()
    {
        MainPanel.SetActive(false);
        StartPanel.SetActive(false);
        InfoPanel.SetActive(true);
    }

    //“单人游戏”按钮调用的函数
    public void StartGameSingle()
    {
        SceneManager.LoadScene("1-MainGame");                             //加载游戏场景
        PlayerPrefs.SetInt("StartGame", 1);                               //设定玩家参数为1（单人），在下一个scene中会调用
    }

    //“双人游戏”按钮调用的函数
    public void StartGameDouble()
    {
        SceneManager.LoadScene("1-MainGame");                             //加载游戏场景
        PlayerPrefs.SetInt("StartGame", 2);                               //设定玩家参数为2（双人），在下一个scene中会调用
    }

    ////声音开关
    //public void SwitchSound()
    //{
    //    if (soundToggle.isOn) PlayerPrefs.SetInt("SoundOff", 0);    //当声音开关开启时，将声音开关设置保存在本地，标识名为“SoundOff”，值为0
    //    else PlayerPrefs.SetInt("SoundOff", 1);                 //当声音开关开启时，将声音开关设置保存在本地，标识名为“SoundOff”，值为1
    //}

    //“退出”按钮调用的函数
    public void ExitGame()
    {
        Application.Quit(); //退出游戏
    }
}
