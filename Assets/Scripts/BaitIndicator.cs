using UnityEngine;
using System.Collections;

public class BaitIndicator : MonoBehaviour {

    public Sprite[] Sprites;

    int lastBait = -1;
    SpriteRenderer _sr;
    MeshRenderer[] messages;

    public float MaxTimer = 1.0f;
    float Timer = 0;

    public static BaitIndicator Current;

    public void Reset()
    {
        lastBait = -1;
        Timer = 0;
    }

    void ToggleMessages(bool active)
    {
        foreach (MeshRenderer r in messages)
        {
            r.enabled = active;
        }
    }

    void Awake()
    {
        Current = this;
    }

    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        messages = GetComponentsInChildren<MeshRenderer>();
        ToggleMessages(false);
    }

    void Update()
    {
        if (Timer > 0)
        {
            Timer -= Time.deltaTime;
            if (Timer < 0)
            {
                ToggleMessages(false);
            }
        }

        if (lastBait != Hook.Current.Bait)
        {
            if (Hook.Current.Bait == -1)
            {
                ToggleMessages(true);
                Timer = MaxTimer;
            }

            _sr.sprite = Sprites[Hook.Current.Bait + 1];
            lastBait = Hook.Current.Bait;
        }
    }
}
