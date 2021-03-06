﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hook : MonoBehaviour {

    public static Hook Current;

    Transform _playerTransform;
    Transform _rodTransform;
    Transform _t;

    LineRenderer _lr;

    float ReelX;
    float ReelAnimTimer = 0;
    public float ReelAnimPeriod = 20.0f;
    public float ReelAnimAmplitude = 0.1f;

    CircleCollider2D baitCollider;

    [System.NonSerialized]
    public List<FishMouth> HookedFishes = new List<FishMouth>();

    public float Depth = 0.3f;
    public float MinDepth = 0.3f;
    public float MaxDepth = 3.0f;
    public float ReelSpeed = 0.5f;
    public float ReelDownMultiplier = 2.0f;
    public float ReelInSpeed = 3.0f;

    public float MaxInterest = 5.0f;
    public float NibbleChance = 0.0f;

    [System.NonSerialized]
    public int Bait = -1;

    public static void ClearBait()
    {
        Current.Bait = -1;
        Current.MaxInterest = 0;
        Current.baitCollider.radius = 0.00001f;
        Current.NibbleChance = 0.0f;
    }

    public static void SetBait(int baitId, float interest, float radius, float nibbleChance)
    {
        Current.Bait = baitId;
        Current.MaxInterest = interest;
        Current.baitCollider.radius = radius;
        Current.NibbleChance = nibbleChance;
    }

    public static void Nibble()
    {
        if (Random.value <= Current.NibbleChance)
        {
            SoundBoard.PlayEatBait();
            ClearBait();
        }
    }

    void Awake()
    {
        Current = this;
    }

	void Start () {
        _t = transform;
        _playerTransform = transform.parent;
        _rodTransform = _playerTransform.FindChild("Fishing Rod").transform;
        _lr = _playerTransform.FindChild("Fishing Line").GetComponent<LineRenderer>();

        baitCollider = transform.FindChild("Bait").GetComponent<CircleCollider2D>();
        ClearBait();
	}

    public void Reset()
    {
        ClearBait();
    }

    const string VERTICAL = "Vertical";

    Vector3 tmpVector;
    void FixedUpdate () {

        if (Player.Current.State == Player.PlayerState.Fishing && GameState.Current.State == GameState.GlobalState.Playing)
        {
            if (Mathf.Abs(Input.GetAxis(VERTICAL)) > 0)
            {
                SoundBoard.PlayCast(true);
                Player.Current.DoAnimation(Player.ANIM_DEPTH);
                Depth -= ReelSpeed * Input.GetAxis(VERTICAL) * Time.fixedDeltaTime * (Input.GetAxis(VERTICAL) > 0 ? ReelDownMultiplier : 1);
                if (Depth > MaxDepth)
                {
                    Depth = MaxDepth;
                    Player.Current.DoAnimation(Player.ANIM_STAND);
                }
                else
                {
                    Player.Current.DoAnimation(Player.ANIM_DEPTH);
                }
            }
            else
            {
                SoundBoard.PlayCast(false);
                Player.Current.DoAnimation(Player.ANIM_STAND);
            }
            if (Depth <= MinDepth)
            {
                Player.Current.State = Player.PlayerState.Moving;
                Player.Current.DoAnimation(Player.ANIM_STAND);
            }
            DrawFishingLine();
        }
        else if (Player.Current.State == Player.PlayerState.Reeling)
        {
            ReelAnimTimer += Time.fixedDeltaTime;
            Depth -= ReelInSpeed * Time.fixedDeltaTime;
            if (Depth < MinDepth)
            {
                SoundBoard.PlayReel(false);
                Player.Current.State = Player.PlayerState.Moving;
                foreach (FishMouth fish in HookedFishes)
                {
                    Hook.Nibble();
                    GameState.Current.Moneys += fish.Fish.ScoreValue();
                    GameState.Current.Fishes.Remove(fish.Fish);
                    DestroyObject(fish.Fish.gameObject);
                }
                SoundBoard.PlayDing();
                HookedFishes.Clear();
                Player.Current.DoAnimation(Player.ANIM_STAND);
            }

            DrawFishingLine();
        }
        else
        {
            SoundBoard.PlayCast(false);
            SoundBoard.PlayReel(false);
            _lr.enabled = false;

            if (Player.Current.State == Player.PlayerState.Moving && Input.GetAxis(VERTICAL) < 0 && Player.Current.CanFish)
            {
                Depth = MinDepth;
                Player.Current.State = Player.PlayerState.Fishing;
                HookedFishes.Clear();
            }
        }
	}

    void DrawFishingLine()
    {
        // Set this position to the depth
        tmpVector = _t.position;
        tmpVector.y = _rodTransform.position.y - Depth;

        if (Player.Current.State == Player.PlayerState.Reeling)
        {
            // Add some animation for reeling in
            tmpVector.x = _rodTransform.position.x + (ReelAnimAmplitude * Mathf.Sin(ReelAnimTimer * ReelAnimPeriod));
        }
        else
        {
            tmpVector.x = _rodTransform.position.x;
        }
        _t.position = tmpVector;

        // Draw the line
        _lr.SetPosition(0, _rodTransform.position);
        _lr.SetPosition(1, tmpVector);
        _lr.enabled = true;
    }

    const string FIRE = "Fire1";

    void Update()
    {
        if (GameState.Current.State != GameState.GlobalState.Playing)
            return;

        if (Input.GetButtonDown(FIRE))
        {
            if (Player.Current.State == Player.PlayerState.Fishing)
            {
                SoundBoard.PlayReel(true);
                Player.Current.DoAnimation(Player.ANIM_REEL);
                ReelAnimTimer = 0;
                Player.Current.State = Player.PlayerState.Reeling;
                foreach (FishMouth fish in HookedFishes)
                {
                    fish.Fish.ReelingIn();
                    fish.Disable(_t.position);
                    fish.Fish.transform.parent = _t;
                }
            }
        }
    }

}