using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 每个交叉点逻辑
public class Cross : MonoBehaviour
{

    // 位置
    public int GridX;
    public int GridY;

    public GameController gc;

    // Use this for initialization
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            gc.OnClick(this);
            gc.OnClickWhite(this);
        });
    }

    // Update is called once per frame
    void Update()
    {

    }
}
