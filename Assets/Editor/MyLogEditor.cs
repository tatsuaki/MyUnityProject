using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyLogEditor : EditorWindow
{
    /// <summary>
    /// ログウィンドウを開く
    /// </summary>
    [MenuItem("MyTools/Log")]
    public static void Open()
    {
		MyLogEditor window = EditorWindow.GetWindow<MyLogEditor>();
        window.Initialize();
        window.ShowUtility();
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
		EditorWindow.GetWindow<MyLogEditor>("MyLog");
    }

    /// <summary>
    /// 更新
    /// </summary>
    void Update()
    {
        Repaint();
    }

    /// <summary>
    /// ウィンドウ内描画
    /// </summary>
    void OnGUI()
    {
        BeginWindows();
		MyLog.DrawLogWindow(new Rect(0, 0, position.width * 2, position.height), true);
        EndWindows();
    }
}