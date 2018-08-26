using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : BaseManager
{
    public AudioManager(GameFacade facade):base(facade){}
    public const string pre_path = "Sounds/";
    public const string Sound_Alert = "Alert";
    public const string Sound_ArrowShoot = "ArrowShoot";
    public const string Sound_Bg_Fast = "Bg(fast)";
    public const string Sound_Bg_Moderate = "Bg(moderate)";
    public const string Sound_ButtonClick = "ButtonClick";
    public const string Sound_Miss = "Miss";
    public const string Sound_ShootPerson = "ShootPerson";
    public const string Sound_Timer = "Timer";
    public const string Sound_Time_3 = "Kenny/3";
    public const string Sound_Time_2 = "Kenny/2";
    public const string Sound_Time_1 = "Kenny/1";
    public const string Sound_Time_go = "Kenny/go";
    public const string Sound_GameStart = "GameStart";
    public const string Sound_ArrowFly = "ShootArrow";
    public const string Sound_ArrowBoom = "ArrowBoom";
    public const string Sound_Win = "Kenny/you_win";
    public const string Sound_Lose = "Kenny/you_lose";

    private AudioSource bgAudioSource;
    private AudioSource normalAudioSource;
    public override void OnInit()
    {
        GameObject audioSourceGO = new GameObject("AudioSource(GameObject)");
        bgAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource = audioSourceGO.AddComponent<AudioSource>();
        normalAudioSource.spatialBlend = 0f;

        //default music
        PlaySound(bgAudioSource, LoadSound(Sound_Bg_Moderate), 0.1f,true);
    }

    public void PlayNormalSound(string name)
    {
        normalAudioSource.spatialBlend = 0f;
        PlaySound(normalAudioSource, LoadSound(name), 0.6f, false);
    }

    public void PlayNormalSound(string name,float spatialBlend)
    {
        normalAudioSource.spatialBlend = spatialBlend;
        PlaySound(normalAudioSource, LoadSound(name), 0.6f, false);
    }

    public void PlayBGSound(string name)
    {
        PlaySound(bgAudioSource, LoadSound(name), 0.1f, true);
    }

    private void PlaySound(AudioSource audioSource,AudioClip audio,float volume,bool loop)
    {
        audioSource.volume = volume;
        audioSource.clip = audio;
        audioSource.loop = loop;
        audioSource.Play();
    }

    private AudioClip LoadSound(string name)
    {
        return Resources.Load<AudioClip>(pre_path + name);
    }
}
