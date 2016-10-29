package com.tatuaki.androidstudio;

import android.app.Activity;
import android.os.Bundle;

import com.tatuaki.androidutil.NativePlugin;

public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //  static public void ShowMessage(final Context context,
        //  String title, String message, String positiveMessage, String NeutralMessage,
        //  String negativeMessage, final String showMessage) {

        NativePlugin.ShowMessage(this, "title", "message", "OK", "Normal",
                "NG", "showMessage");
    }
}
