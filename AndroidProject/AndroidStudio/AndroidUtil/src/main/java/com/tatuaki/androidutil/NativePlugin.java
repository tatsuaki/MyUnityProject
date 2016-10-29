package com.tatuaki.androidutil;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.util.Log;
import android.widget.Toast;

public class NativePlugin {
    private static  final String TAG = NativePlugin.class.getSimpleName();

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

    static public void ShowToast(Context context, String message) {
        Toast.makeText(context, message, Toast.LENGTH_SHORT).show();
    }

    public int ChangeValue(int before) {
        Log.w(TAG, "ChangeValue " + before);
        int after = before * 3;
        Log.w(TAG, "ChangeValue after " + after);
        return after;
    }
}
