using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NetController : MonoBehaviour
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
        Restart();
    }

    // Update is called once per frame
    void Update()
    {

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
