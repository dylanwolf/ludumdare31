using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SoundBoard : MonoBehaviour {

    public static SoundBoard Current;

    AudioSource oneShotter;

    void Awake()
    {
        Current = this;
    }

    void Start()
    {
        oneShotter = GetComponents<AudioSource>().Where(a => a.clip == null).FirstOrDefault();
    }

    static void PlayLoopingSound(AudioSource src, bool playing)
    {
        if (src != null)
        {
            if (playing && !src.isPlaying)
                src.Play();
            else if (!playing && src.isPlaying)
                src.Stop();
        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip == null)
            return;

        if (oneShotter != null)
            oneShotter.PlayOneShot(clip, oneShotter.volume);
    }

    static AudioClip GetRandomClip(AudioClip[] array)
    {
        if (array != null)
        {
            return array[Random.Range(0, array.Length-1)];
        }
        return null;
    }

    public AudioClip[] BuySounds;
    public AudioClip[] CantBuySounds;
    public AudioClip[] DingSounds;
    public AudioClip[] EatBaitSounds;
    public AudioClip[] TimeUpSounds;
    public AudioClip[] IcePickSounds;

    public AudioSource ReelSound;
    public AudioSource CastSound;

    public static void PlayBuy()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.BuySounds));
        }
    }

    public static void PlayCantBuy()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.CantBuySounds));
        }
    }

    public static void PlayDing()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.DingSounds));
        }
    }

    public static void PlayEatBait()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.EatBaitSounds));
        }
    }

    public static void PlayTimeUp()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.TimeUpSounds));
        }
    }

    public static void PlayIcePick()
    {
        if (Current != null)
        {
            Current.PlaySound(GetRandomClip(Current.IcePickSounds));
        }
    }

    public static void PlayCast(bool playing)
    {
        if (Current != null)
        {
            PlayLoopingSound(Current.CastSound, playing);
        }
    }

    public static void PlayReel(bool playing)
    {
        if (Current != null)
        {
            PlayLoopingSound(Current.ReelSound, playing);
        }
    }
}
