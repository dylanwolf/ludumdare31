using UnityEngine;
using System.Collections;
using System.Linq;

public class IceFloe : MonoBehaviour {

    public bool IsCleared = false;
    public float MinTimer = 2.0f;
    public float MaxTimer = 5.0f;
    float Timer = 0;
    Renderer _r;

    float RandomFloat(float min, float max)
    {
        return ((max - min) * Random.value) + min;
    }

    void Start()
    {
        _r = renderer;
    }

	void FixedUpdate () {
        if (IsCleared)
        {
            if (Timer <= 0)
            {
                Timer = RandomFloat(MinTimer, MaxTimer);
            }
            else if (GameState.Current.State == GameState.GlobalState.Playing)
            {
                Timer -= Time.fixedDeltaTime;
                if (Timer < 0 && !((Player.Current.State == Player.PlayerState.Fishing || Player.Current.State == Player.PlayerState.Reeling) && Player.Current.PickTarget.Contains(this)))
                {
                    IsCleared = false;
                }
            }
        }
	}

    void Update()
    {
        _r.enabled = !IsCleared;
    }
}
