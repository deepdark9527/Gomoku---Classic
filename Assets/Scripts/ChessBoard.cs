using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChessBoard : MonoBehaviour
{
    public GameObject CrossPrefab;

    // 交叉点大小
    const float CrossSize = 47;

    // 15个交叉
    public const int CrossCount = 15;

    // 棋盘大小
    public const float Size = 658;
    public const float HalfSize = Size / 2;

    // 存储每个交叉点按钮信息
    Dictionary<int, Cross> _crossMap = new Dictionary<int, Cross>();

    static int MakeKey(int x, int y)
    {
        return x * 10000 + y;
    }

    public void Reset()
    {
        // 删除棋盘上的所有对象
        foreach (Transform child in gameObject.transform)
        {
            Destroy(child.gameObject);
        }

        var gc = GetComponent<GameController>();

        _crossMap.Clear();

        for (int x = 0; x < CrossCount; x++)
        {
            for (int y = 0; y < CrossCount; y++)
            {
                var crossObj = Instantiate(CrossPrefab);

                // 归属于本层对象下
                crossObj.transform.SetParent(gameObject.transform);

                // 复位缩放
                crossObj.transform.localScale = Vector3.one;

                // 设置位置
                var pos = crossObj.transform.localPosition;
                pos.x = (int)(-HalfSize + x * CrossSize);
                pos.y = (int)(-HalfSize + y * CrossSize);
                pos.z = 1;
                crossObj.transform.localPosition = pos;

                // 记录格子信息
                var cross = crossObj.GetComponent<Cross>();
                cross.GridX = x;
                cross.GridY = y;
                cross.gc = gc;

                _crossMap.Add(MakeKey(x, y), cross);


            }
        }
    }

    public Cross GetCross(int gridX, int gridY)
    {
        Cross cross;
        if (_crossMap.TryGetValue(MakeKey(gridX, gridY), out cross))
        {
            return cross;
        }

        return null;
    }

    // Use this for initialization
    void Start()
    {
        Reset();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
