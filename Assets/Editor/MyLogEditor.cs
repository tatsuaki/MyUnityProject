using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MyLogEditor : EditorWindow
{
    [MenuItem("MyTools/Log")]
    public static void Open()
    {
		MyLogEditor window = EditorWindow.GetWindow<MyLogEditor>();
        window.Initialize();
        window.ShowUtility();
    }

    public void Initialize()
    {
		EditorWindow.GetWindow<MyLogEditor>("MyLog");
    }

    void Update()
    {
        Repaint();
    }

    void OnGUI()
    {
        BeginWindows();
		MyLog.DrawLogWindow(new Rect(0, 0, position.width * 2, position.height), true);
        EndWindows();
    }
}