using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour {

    public enum PlayerState
    {
        Moving,
        Fishing,
        Reeling
    }

    public static Player Current;

    public PlayerState State;

    public float MinX = -4.75f;
    public float MaxX = 2.25f;

    float WalkTimer;
    public float WalkTimerMax = 0.25f;
    public float WalkStep = 0.2f;

    Transform _t;

    void Awake()
    {
        Current = this;
    }

    void Start()
    {
        _t = transform;
    }

    [System.NonSerialized]
    public List<IceFloe> PickTarget = new List<IceFloe>();

    public void SetIcePick(Collider2D collider)
    {
        PickTarget.Add(collider.GetComponent<IceFloe>());
    }

    public void ClearIcePick(Collider2D collider)
    {
        PickTarget.Remove(collider.GetComponent<IceFloe>());
    }

    const string HORIZONTAL = "Horizontal";
    const string FIRE = "Fire1";

    private Vector3 tmpPos;
    private Vector3 tmpScl;
	void Update () {
        if (GameState.Current.State != GameState.GlobalState.Playing)
            return;

        if (State == PlayerState.Moving)
        {
            if (WalkTimer >= 0)
            {
                WalkTimer -= Time.deltaTime;
            }

            if (WalkTimer <= 0)
            {
                if (Mathf.Abs(Input.GetAxis(HORIZONTAL)) > 0)
                {
                    WalkTimer = WalkTimerMax;
                    tmpPos = _t.position;
                    tmpScl = _t.localScale;
                    tmpPos.x += Mathf.Sign(Input.GetAxis(HORIZONTAL)) * WalkStep;

                    if (tmpPos.x > MaxX)
                        tmpPos.x = MaxX;
                    if (tmpPos.x < MinX)
                        tmpPos.x = MinX;

                    tmpScl.x = Mathf.Abs(tmpScl.x) * -Mathf.Sign(Input.GetAxis(HORIZONTAL));

                    _t.localScale = tmpScl;
                    _t.position = tmpPos;
                }
            }

            if (PickTarget.Count > 0 && Input.GetButtonDown(FIRE))
            {
                foreach (IceFloe f in PickTarget)
                {
                    f.IsCleared = true;
                }
            }
        }
	}
}
