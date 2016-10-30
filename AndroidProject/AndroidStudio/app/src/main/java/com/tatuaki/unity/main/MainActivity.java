package com.tatuaki.unity.main;

import android.app.Activity;
import android.os.Bundle;
import android.util.Log;
import android.widget.Toast;

import com.tatuaki.unity.utils.NativePlugin;

public class MainActivity extends Activity {
    private static final String TAG = MainActivity.class.getSimpleName();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(com.tatuaki.unity.main.R.layout.activity_main);

        // TODO デバッグ
        NativePlugin.init(this.getApplicationContext());
        NativePlugin.setActivity(this);
        String token = NativePlugin.GetAndroidToken(getApplicationContext());
        Log.w(TAG, "GetAndroidToken token = " + token);
        Toast.makeText(this, token, Toast.LENGTH_SHORT).show();
    }
}