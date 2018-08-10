using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using thelab.mvc;

public class ViewSoundSource : View<SnakeApplication> {

    public AudioSource audioSource { get { return m_audioSource = Assert<AudioSource>(m_audioSource); } }
    private AudioSource m_audioSource;

    public bool pause = false;

    public void Play(AudioClip audioClip, bool oneshot, bool loop, System.Action OnEnd, float endDelay) {
        Stop();
        if (oneshot) {
            audioSource.PlayOneShot(audioClip);
        }
        else {
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
            if (OnEnd != null) {
                StartCoroutine(SoundTimer(audioClip, OnEnd, endDelay));
            }
        }
    }

    public void Play(AudioClip audioClip, bool loop, System.Action OnEnd, float endDelay = 0f) {
        Play(audioClip, false, loop, OnEnd, endDelay);
    }

    public void Play(AudioClip audioClip, bool loop) {
        Play(audioClip, false, loop, null, 0f);
    }

    public void PlayOneshot(AudioClip audioClip) {
        Play(audioClip, true, false, null, 0f);
    }

    public void Pause() {
        pause = true;
        audioSource.Pause();
    }

    public void Unpause() {
        pause = false;
        audioSource.UnPause();
    }

    public void Stop() {
        pause = false;
        StopAllCoroutines();
        audioSource.Stop();
    }

    IEnumerator SoundTimer(AudioClip audioClip, System.Action OnEnd, float endDelay) {
        float length = audioClip.length + endDelay;
        float timer = 0f;
        while (timer < length) {
            if (!pause) {
                timer += Time.deltaTime;
            }
            yield return null;
        }
        OnEnd();
    }
}
