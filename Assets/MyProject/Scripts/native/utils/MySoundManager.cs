using UnityEngine;
using System.Collections;

public class MySoundManager : MonoBehaviour {

	//音声ファイル格納用変数
	public AudioClip sound01;
	public AudioClip sound02;

	private AudioSource audioSource;

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
		audioSource = getAudioSource();
		audioSource.clip = sound01;
		audioSource.PlayOneShot(sound01);
	}

	public void playSound2() {
		audioSource = getAudioSource();
		audioSource.clip = sound02;
		audioSource.PlayOneShot(sound02);
		// GetComponent<AudioSource>().PlayOneShot(sound02);
	}

	private AudioSource getAudioSource() {
		if (null == audioSource) {
			audioSource = gameObject.GetComponent<AudioSource>();
		}
		return audioSource;
	}
}