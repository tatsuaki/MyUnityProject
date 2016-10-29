package com.tatuaki.unity;

import android.content.Context;
import android.util.Log;
import android.widget.Toast;

import com.google.firebase.iid.FirebaseInstanceId;
import com.google.firebase.messaging.FirebaseMessaging;

public class MyPushManager {
    private static final String TAG = MyPushManager.class.getSimpleName();

    static public void ShowTopic(final Context context) {
        FirebaseMessaging.getInstance().subscribeToTopic("news");
        // [END subscribe_topics]

        // Log and toast
//        String msg = getString(R.string.msg_subscribed);
//        Log.d(TAG, msg);
//        Toast.makeText(context, msg, Toast.LENGTH_SHORT).show();
    }

    // NoClassDefFoundError: Failed resolution of: Lcom/google/firebase/iid/FirebaseInstanceId;
    public static String GetToken() {
        String token = FirebaseInstanceId.getInstance().getToken();

        Log.d(TAG, "GetToken = " + token);
        return token;
    }

    public static String GetToken(final Context context) {
        // FirebaseMessaging.getInstance().subscribeToTopic("news");
        String token = FirebaseInstanceId.getInstance().getToken();

        Log.d(TAG, "GetToken = " + token);
        return token;
    }
}
