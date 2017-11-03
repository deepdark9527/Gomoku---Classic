using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    // 棋子的模板
    public GameObject WhitePrefab;
    public GameObject BlackPrefab;

    // 结果窗口
    public GameObject PanelUpper;     //上方初始面板（悔棋，认输）
    public GameObject WinPanel;       //胜利信息面板
    public Image winPic;
    public Sprite winBlack;
    public Sprite winWhite;

    //游戏类型(1为单人游戏，2为双人游戏)
    int gametype;

    public Button RegButton;

    //存储棋子的历史记录
    int _lastPlayerX, _lastPlayerY;
    ArrayList lastPlayerX = new ArrayList();
    ArrayList lastPlayerY = new ArrayList();
    int count = 0;

    enum State
    {
        BlackGo, // 黑方(玩家)走
        WhiteGo, // 白方(电脑)走
        Over,    // 结束
    }

    // 当前状态
    State _state;
    ChessBoard _board;

    // 棋盘数据
    BoardModel _model;

    // 人工智能
    AI _ai;

    bool CanPlace(int gridX, int gridY)
    {
        // 如果这个地方可以下棋子
        return _model.Get(gridX, gridY) == ChessType.None;
    }

    // base 1
    bool PlaceChess(Cross cross, bool isblack)
    {
        if (cross == null)
            return false;

        // 创建棋子
        var newChess = Instantiate(isblack ? BlackPrefab : WhitePrefab);
        newChess.transform.SetParent(cross.gameObject.transform, false);
        // 设置数据
        _model.Set(cross.GridX, cross.GridY, isblack ? ChessType.Black : ChessType.White);

        var ctype = isblack ? ChessType.Black : ChessType.White;

        var linkCount = _model.CheckLink(cross.GridX, cross.GridY, ctype);

        return linkCount >= BoardModel.WinChessCount;
    }

    //黑方（玩家1）点击事件
    public void OnClick(Cross cross)
    {
        if (_state != State.BlackGo)
            return;

        // 不能在已经放置过的棋子上放置
        if (CanPlace(cross.GridX, cross.GridY))
        {
            lastPlayerX.Add(cross.GridX);
            lastPlayerY.Add(cross.GridY);
            count++;
            _lastPlayerX = (int)lastPlayerX[count - 1];
            _lastPlayerY = (int)lastPlayerY[count - 1];

            if (PlaceChess(cross, true))
            {
                // 黑方胜利
                _state = State.Over;
                PanelUpper.SetActive(false);  //上方面板切换至胜利面板（禁用悔棋和认输）
                WinPanel.SetActive(true);
                winPic.sprite = winBlack;   //加载黑方胜利图片
            }
            else
            {
                // 换白方（AI或玩家2）走
                _state = State.WhiteGo;
            }
        }
    }

    //白方（玩家2）点击事件
    public void OnClickWhite(Cross cross)
    {
        if (_state != State.WhiteGo)
            return;

        // 不能在已经放置过的棋子上放置
        if (CanPlace(cross.GridX, cross.GridY))
        {
            lastPlayerX.Add(cross.GridX);
            lastPlayerY.Add(cross.GridY);
            count++;
            _lastPlayerX = (int)lastPlayerX[count - 1];
            _lastPlayerY = (int)lastPlayerY[count - 1];


            if (PlaceChess(cross, false))
            {
                // 白方胜利
                _state = State.Over;
                PanelUpper.SetActive(false);  //上方面板切换至胜利面板（禁用悔棋和认输）
                WinPanel.SetActive(true);
                winPic.sprite = winWhite;   //加载白方胜利图片
            }
            else
            {
                // 换黑方（玩家1）走
                _state = State.BlackGo;
            }
        }
    }

    void Start()
    {
        _board = GetComponent<ChessBoard>();
        gametype = PlayerPrefs.GetInt("StartGame");   //读取游戏类型参数(1为单人游戏，2为双人游戏)
        Restart();
        if (gametype == 1)
        {
            RegButton.interactable = false;
        }
    }


    //void ShowResult(ChessType winside)
    //{
    //    ResultWindow.gameObject.SetActive(true);
    //    ResultWindow.Show(winside);
    //}

    // Update is called once per frame
    void Update()
    {
        //如果参数为单人游戏
        if (gametype == 1)
        {
            switch (_state)
            {
                // 白方(电脑)走
                case State.WhiteGo:
                    {
                        // 计算电脑下的位置
                        int gridX, gridY;
                        _ai.ComputerDo(_lastPlayerX, _lastPlayerY, out gridX, out gridY);

                        lastPlayerX.Add(gridX);
                        lastPlayerY.Add(gridY);
                        count++;
                        _lastPlayerX = (int)lastPlayerX[count - 1];
                        _lastPlayerY = (int)lastPlayerY[count - 1];

                        if (PlaceChess(_board.GetCross(gridX, gridY), false))
                        {
                            // 白方胜利
                            _state = State.Over;
                            PanelUpper.SetActive(false);  //上方面板切换至胜利面板（禁用悔棋和认输）
                            WinPanel.SetActive(true);
                            winPic.sprite = winWhite;   //加载白方胜利图片
                        }
                        else
                        {
                            // 换黑方（玩家1）走
                            _state = State.BlackGo;
                        }

                    }
                    break;
            }
        }

    }

    //悔棋函数
    public void RegretChess()
    {
        int gametype = PlayerPrefs.GetInt("StartGame");

        //单人游戏，一次悔两步（双方）
        if (gametype == 1)
        {
            //人机对战悔棋需要回复AI记录（待完成）
            //for (int i = 0; i < 2; i++)
            //{
            //    if (count > 0)
            //    {
            //        Cross cross = _board.GetCross(_lastPlayerX, _lastPlayerY);
            //        // 删除棋盘上的所有对象
            //        foreach (Transform child in cross.transform)
            //        {
            //            Destroy(child.gameObject);
            //        }
            //        _model.Set(_lastPlayerX, _lastPlayerY, ChessType.None);
            //        lastPlayerX.RemoveAt(count - 1);
            //        lastPlayerY.RemoveAt(count - 1);
            //        count--;
            //        if (count > 0)
            //        {
            //            _lastPlayerX = (int)lastPlayerX[count - 1];
            //            _lastPlayerY = (int)lastPlayerY[count - 1];
            //        }
            //    }
            //}
        }
        //双人游戏，一次悔一步（单方），并修正出棋顺序
        else if (gametype == 2)
        {
            if (count > 0)
            {
                Cross cross = _board.GetCross(_lastPlayerX, _lastPlayerY);
                // 删除棋盘上的所有对象
                foreach (Transform child in cross.transform)
                {
                    Destroy(child.gameObject);
                }
                _model.Set(_lastPlayerX, _lastPlayerY, ChessType.None);
                lastPlayerX.RemoveAt(count - 1);
                lastPlayerY.RemoveAt(count - 1);
                count--;
                if (count > 0)
                {
                    _lastPlayerX = (int)lastPlayerX[count - 1];
                    _lastPlayerY = (int)lastPlayerY[count - 1];
                }

                //切换玩家
                if (_state == State.BlackGo)
                {
                    _state = State.WhiteGo;
                }
                else if (_state == State.WhiteGo)
                {
                    _state = State.BlackGo;
                }
            }

        }

    }

    //认输函数
    public void LoseGame()
    {
        PanelUpper.SetActive(false);  //上方面板切换至胜利面板（禁用悔棋和认输）
        WinPanel.SetActive(true);

        if (_state == State.BlackGo)  //如果本回合为黑方回合
        {
            winPic.sprite = winWhite;   //加载白方胜利图片
        }
        else
        {
            winPic.sprite = winBlack;   //加载黑方胜利图片
        }
    }

    //重置游戏
    public void Restart()
    {
        _state = State.BlackGo;
        _model = new BoardModel();
        _ai = new AI();
        _board.Reset();

        PanelUpper.SetActive(true);  //恢复上方初始面板（悔棋，认输）
        WinPanel.SetActive(false);
    }

    //返回主菜单
    public void ReturnMenu()
    {
        SceneManager.LoadScene("0-MainMenu");                             //加载游戏场景
    }

}
