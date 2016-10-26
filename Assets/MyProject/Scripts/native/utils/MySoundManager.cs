using UnityEngine;
using System.Collections;

public class MySoundManager : MonoBehaviour {

	//音声ファイル格納用変数
	public AudioClip sound01;
	public AudioClip sound02;

	void Update () {

		//指定のキーが押されたら音声ファイルの再生をする
		if(Input.GetKeyDown(KeyCode.K)) {
			GetComponent<AudioSource>().PlayOneShot(sound01);
		}
		if(Input.GetKeyDown(KeyCode.L)) {
			GetComponent<AudioSource>().PlayOneShot(sound02);
		}
	}

	public void playSound1() {
		GetComponent<AudioSource>().PlayOneShot(sound01);
	}
	public void playSound2() {
		GetComponent<AudioSource>().PlayOneShot(sound02);
	}

}