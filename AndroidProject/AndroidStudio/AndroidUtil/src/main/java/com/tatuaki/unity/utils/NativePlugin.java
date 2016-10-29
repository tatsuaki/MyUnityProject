package com.tatuaki.unity.utils;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.util.Log;
import android.widget.Toast;

import com.tatuaki.unity.MyPushManager;

public class NativePlugin {
    private static final String TAG = NativePlugin.class.getSimpleName();
    public static Context mContext;
    public static Activity mActivity;

    static public void ShowMessage(final Context context, String title, String message, String positiveMessage, String NeutralMessage, String negativeMessage, final String showMessage) {

        new AlertDialog.Builder(context)
                .setTitle(title)
                .setMessage(message)
                .setPositiveButton(positiveMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        ShowToast(context, showMessage);
                    }
                })
                .setNeutralButton(NeutralMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {

                    }
                })
                .setNegativeButton(negativeMessage, new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
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

    public static String GetAndroidToken(Context context, Activity activity) {
        if (null == mContext) {
            setContext(context);
            setActivity(activity);
        }
        Log.w(TAG, "GetAndroidToken");
        return MyPushManager.GetToken();
    }

    public int ChangeValue(int before) {
        Log.w(TAG, "ChangeValue " + before);
        int after = before * 3;
        Log.w(TAG, "ChangeValue after " + after);
        return after;
    }
}
