using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 画面表示可能なデバッグログ
/// </summary>
/// <example>
/// // 各レベルのログ出力（コンソールにも同時に出力）
/// SgpLog.V("verbose log");
/// SgpLog.D("debug log");
/// SgpLog.I("information log");
/// SgpLog.W("warning log");
/// SgpLog.E("error log");
/// 
/// // 画面にのみ出力
/// SgpLog.V("verbose log (not console)", false);
/// SgpLog.D("debug log (not console)", false);
/// 
/// // 画面上にログウィンドウを表示（※ OnGUI()内での呼び出しにのみ対応）
/// SgpLog.DrawLogWindow(new Rect(0, Screen.Height * 0.5f, Screen.Width * 1.0f, Screen.Height * 0.5f));
/// </example>


public static class MyLog
{
	#region ///////////// タイプ /////////////
	public enum Level
	{
		Verbose,
		Debug,
		Information,
		Warning,
		Error,

		Max
	}

	private struct LogData
	{
		public Level Level { get; set; }
		public string Message { get; set; }
		public System.DateTime TimeStamp { get; set; }
	}
	#endregion

	#region ///////////// 定数 /////////////
	/// 表示可能ログ最大数
	private static int LOG_MAX = 512;

	/// 各レベルのログ色
	private static Color[] LOG_COLOR =
	{
		Color.gray,
		Color.white,
		Color.cyan,
		Color.yellow,
		Color.red
	};
	#endregion

	#region ///////////// メンバ /////////////
	private static Level _logLevel = Level.Debug;                           /// 表示対象ログレベル
	private static Queue<LogData> _logQue = new Queue<LogData>(LOG_MAX);    /// ログキュー
	private static Vector2 _scrollPosition = Vector2.zero;                  /// スクロールビュー位置
	private static bool _isNeedScrollReset = false;                         /// スクロール位置リセットフラグ
	#endregion

	#region ///////////// ログ処理 /////////////
	public static void V(string message, bool isConsole = true) { _Push(Level.Verbose, message, isConsole); }
	public static void D(string message, bool isConsole = true) { _Push(Level.Debug, message, isConsole); }
	public static void I(string message, bool isConsole = true) { _Push(Level.Information, message, isConsole); }
	public static void W(string message, bool isConsole = true) { _Push(Level.Warning, message, isConsole); }
	public static void E(string message, bool isConsole = true) { _Push(Level.Error, message, isConsole); }

	/// <summary>
	/// ログを画面上に表示する
	/// OnGUI() 内で呼び出して下さい
	/// </summary>
	/// <param name="drawArea">描画対象領域（左上が0,0）</param>
	/// <param name="isEditor">エディタスクリプトから使用する場合trueにする</param>
	public static void DrawLogWindow(Rect drawArea, bool isEditor = false)
	{
		// エディタ実行中はエディタ側のウィンドウに任せる
		if (Application.isEditor && !isEditor)
		{
			return;
		}

		// 下地
		GUI.Box(drawArea, "");

		GUILayout.BeginArea(drawArea);
		{
			GUILayout.BeginHorizontal();
			/////////////
			// ログレベル切り替えボタン
			if (GUILayout.Button(_logLevel.ToString(), GUILayout.Width(400), GUILayout.Height(60)))
			{
				_logLevel = (Level)((int)(_logLevel + 1) % (int)Level.Max);
				_isNeedScrollReset = true;
			}

			/////////////
			// クリップボードにコピー
			#if UNITY_EDITOR
			if (GUILayout.Button("Copy All", GUILayout.Width(400), GUILayout.Height(60)))
			{
				EditorGUIUtility.systemCopyBuffer = _GetLogString(_logLevel);
			}
			#endif
			GUILayout.EndHorizontal();

			/////////////
			// ログスクロールビュー
			// @todo    メッセージの数だけLabelなりを作る。重い
			//          ただしTextAreaにすると色分けできない
			_scrollPosition = GUILayout.BeginScrollView(_scrollPosition, true, true);
			//			RectTransform rec = GameObject.Find("Scrollbar").GetComponent<RectTransform>();
			//			if (null != rec) {
			//				rec.sizeDelta = new Vector2(100, 200);
			//			}

			int num = 0;
			GUIStyle style = new GUIStyle();
			style.fontSize = 30;
			foreach (LogData d in _logQue)
			{
				// ログレベルによって弾く
				if (d.Level >= _logLevel)
				{
					// 0始まりのログ行数をプレフィックスとして付加しています
					// 時刻にした方がいいかも
					style.normal.textColor = LOG_COLOR[(int)d.Level];
					// エディターウィンドウ用の描画
					if (isEditor)
					{
						#if UNITY_EDITOR
						EditorGUILayout.TextArea(d.TimeStamp.ToString(@"MM/dd HH:mm:ss | ") + d.Message, style);
						#endif
					}
					else
					{
						//GUILayout.Label(num.ToString() + " " + d.Message, style);
						GUILayout.Label(d.Message, style);
					}
					++num;
				}
			}

			// ログ表示位置を先頭にリセットする
			if (_isNeedScrollReset)
			{
				_isNeedScrollReset = false;
				_scrollPosition.y = (num > 0) ? (num - 1) * 20 : 0;
			}

			GUILayout.EndScrollView();

			/////////////
			// クリアボタン
			if (GUILayout.Button("Clear", GUILayout.Width(400), GUILayout.Height(60)))
			{
				_logQue.Clear();
			}
		}
		GUILayout.EndArea();

	}

	/// <summary>
	/// ログをキューに詰める
	/// </summary>
	/// <param name="level"></param>
	/// <param name="message"></param>
	/// <param name="isConsole"></param>
	private static void _Push( Level level, string message, bool isConsole )
	{
		/////////////
		// キューが一杯だったら後ろを削除
		if (_logQue.Count >= LOG_MAX)
		{
			_logQue.Dequeue();
		}

		/////////////
		// キューに詰める
		LogData data = new LogData()
		{
			Level = level,
			Message = message,
			TimeStamp = System.DateTime.Now
		};
		_logQue.Enqueue(data);

		// ログ位置を調整する
		// @todo 出る度に勝手に動く。要らないかも？
		_isNeedScrollReset = true;

		/////////////
		// コンソール出力
		if (isConsole)
		{
			switch (level)
			{
			case Level.Warning: Debug.LogWarning(message); break;
			case Level.Error: Debug.LogError(message); break;
			default: Debug.Log(message); break;
			}
		}
	}
	#endregion

	#region ///////////// 内部処理 /////////////
	/// <summary>
	/// ログを1文字列としてまとめて取得。クリップボードコピー用の処理
	/// </summary>
	private static string _GetLogString(Level minLevel)
	{
		StringBuilder builder = new StringBuilder();

		foreach (LogData d in _logQue)
		{
			// ログレベルによって弾く
			if (d.Level >= minLevel)
			{
				builder.AppendLine(d.TimeStamp.ToString("MM/dd HH:mm:ss \t ") + System.Enum.GetName(typeof(Level), d.Level).Substring(0, 1) + "\t" + d.Message);
			}
		}

		return builder.ToString();
	}
	#endregion
}