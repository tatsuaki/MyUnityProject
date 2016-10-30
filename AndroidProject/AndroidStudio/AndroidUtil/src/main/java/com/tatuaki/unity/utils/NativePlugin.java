package com.tatuaki.unity.utils;

import android.app.Activity;
import android.app.AlertDialog;
import android.app.Application;
import android.content.Context;
import android.content.DialogInterface;
import android.util.Log;
import android.widget.Toast;

import com.google.firebase.analytics.FirebaseAnalytics;
import com.tatuaki.unity.MyPushManager;

public class NativePlugin {
    private static final String TAG = NativePlugin.class.getSimpleName();
    public static Context mContext;
    public static Activity mActivity;

    public static FirebaseAnalytics mFirebaseAnalytics = null;

    static String tokens = null;

    public static void init(Context context) {
        Log.w(TAG, "init");
        // MultiDex.install(context);
        setContext(context);
        mFirebaseAnalytics = FirebaseAnalytics.getInstance(context);
    }

    static public void ShowMessage(final Context context, String title, String message, String positiveMessage, String NeutralMessage, String negativeMessage, final String showMessage) {
        Log.w(TAG, "ShowMessage");
        final Application application = (Application) context.getApplicationContext();
        new AlertDialog.Builder(context)
                .setTitle(title)
                .setMessage(message)
                .setPositiveButton(positiveMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        ShowToast(context, showMessage);
//                        Bundle fireLogBundle = new Bundle();
//                        fireLogBundle.putString("TEST", "NativePlugin PositiveButton is called.");
//                        mFirebaseAnalytics.logEvent(FirebaseAnalytics.Event.APP_OPEN, fireLogBundle);
                    }
                })
                .setNeutralButton(NeutralMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
//                        Log.d(TAG, "GetToken start");
//                        FirebaseInstanceId firebaseInstanceId = FirebaseInstanceId.getInstance();
//                        Log.d(TAG, "GetToken FirebaseInstanceId.getInstance");
//                        tokens = firebaseInstanceId.getToken();
//
//                        Log.d(TAG, "GetToken = " + tokens);
//                        Bundle fireLogBundle = new Bundle();
//                        fireLogBundle.putString("TEST", "NativePlugin NeutralButton is called.");
//                        mFirebaseAnalytics.logEvent(FirebaseAnalytics.Event.APP_OPEN, fireLogBundle);
                    }
                })
                .setNegativeButton(negativeMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
//                        Bundle fireLogBundle = new Bundle();
//                        fireLogBundle.putString("TEST", "NativePlugin NegativeButton is called.");
//                        mFirebaseAnalytics.logEvent(FirebaseAnalytics.Event.APP_OPEN, fireLogBundle);
                    }
                })
                .show();
    }

    /**
     * @param context
     * @param message
     */
    static public void ShowToast(Context context, String message) {
        Log.w(TAG, "ShowToast context message = " + message);
        setContext(context);
        if (null != mContext) {
            Toast.makeText(mContext, message, Toast.LENGTH_SHORT).show();
        }
    }

    static public void ShowToast(final String message) {
        Log.w(TAG, "ShowToast message = " + message);
        if (null != mContext) {
            mActivity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    Toast.makeText(mContext, "push : " + message, Toast.LENGTH_SHORT).show();
                }
            });
        }
    }

    public static void setContext(Context context) {
        Log.w(TAG, "setContext");
        if (null == mContext) {
            mContext = context;
        }
    }

    public static void setActivity(Activity activity) {
        Log.w(TAG, "setActivity");
        mActivity = activity;
    }

    public static String GetAndroidToken(Context context) {
        if (null == mContext) {
            setContext(context);
        }

        Log.w(TAG, "GetAndroidToken to MyPushManager GetToken");
        if (null != mContext) {
            mActivity.runOnUiThread(new Runnable() {
                @Override
                public void run() {
                    tokens = MyPushManager.GetToken();
                }
            });
        }
        return tokens;
    }

    public int ChangeValue(int before) {
        Log.w(TAG, "ChangeValue " + before);
        int after = before * 3;
        Log.w(TAG, "ChangeValue after " + after);
        return after;
    }
}
