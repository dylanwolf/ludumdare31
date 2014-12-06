using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Hook : MonoBehaviour {

    Transform _playerTransform;
    Transform _rodTransform;
    Transform _t;

    LineRenderer _lr;

    [System.NonSerialized]
    public List<Collider2D> _fish = new List<Collider2D>();

    public float Depth = 0.3f;
    public float MinDepth = 0.3f;
    public float MaxDepth = 3.0f;
    public float ReelSpeed = 0.5f;
    public float ReelDownMultiplier = 2.0f;
    public float ReelInSpeed = 3.0f;

    float ReelX;
    float ReelAnimTimer = 0;
    public float ReelAnimPeriod = 20.0f;
    public float ReelAnimAmplitude = 0.1f;

	void Start () {
        _t = transform;
        _playerTransform = transform.parent;
        _rodTransform = _playerTransform.FindChild("Fishing Rod").transform;
        _lr = _playerTransform.FindChild("Fishing Line").GetComponent<LineRenderer>();
	}

    const string VERTICAL = "Vertical";

    Vector3 tmpVector;
    void FixedUpdate () {

        if (Player.CurrentState == Player.PlayerState.Fishing)
        {
            if (Mathf.Abs(Input.GetAxis(VERTICAL)) > 0)
            {
                Depth -= ReelSpeed * Input.GetAxis(VERTICAL) * Time.fixedDeltaTime * (Input.GetAxis(VERTICAL) < 0 ? ReelDownMultiplier : 1);
                if (Depth <= MinDepth)
                    Player.CurrentState = Player.PlayerState.Moving;
                else if (Depth > MaxDepth)
                    Depth = MaxDepth;
            }
            DrawFishingLine();
        }
        else if (Player.CurrentState == Player.PlayerState.Reeling)
        {
            ReelAnimTimer += Time.fixedDeltaTime;
            Depth -= ReelInSpeed * Time.fixedDeltaTime;
            if (Depth < MinDepth)
            {
                Player.CurrentState = Player.PlayerState.Moving;
                foreach (Collider2D c in _fish)
                {
                    Fish f = c.transform.parent.GetComponent<Fish>();
                    GameState.Fish.Remove(f);
                    DestroyObject(f.gameObject);
                }
                _fish.Clear();
            }

            DrawFishingLine();
        }
        else
        {
            _lr.enabled = false;

            if (Player.CurrentState == Player.PlayerState.Moving && Input.GetAxis(VERTICAL) < 0 && !Player.PickTarget.Any(pt => !pt.IsCleared))
            {
                Depth = MinDepth;
                Player.CurrentState = Player.PlayerState.Fishing;
                _fish.Clear();
            }
        }
	}

    void DrawFishingLine()
    {
        // Set this position to the depth
        tmpVector = _t.position;
        tmpVector.y = _rodTransform.position.y - Depth;
        _t.position = tmpVector;

        if (Player.CurrentState == Player.PlayerState.Reeling)
        {
            // Add some animation for reeling in
            tmpVector.x = tmpVector.x + (ReelAnimAmplitude * Mathf.Sin(ReelAnimTimer * ReelAnimPeriod));
        }

        // Draw the line
        _lr.SetPosition(0, _rodTransform.position);
        _lr.SetPosition(1, tmpVector);
        _lr.enabled = true;
    }

    const string FIRE = "Fire1";

    void Update()
    {
        if (GameState.CurrentGlobal != GameState.GlobalState.Playing)
            return;

        if (Input.GetButtonDown(FIRE))
        {
            if (Player.CurrentState == Player.PlayerState.Fishing)
            {
                ReelAnimTimer = 0;
                Player.CurrentState = Player.PlayerState.Reeling;
                foreach (Collider2D fish in _fish)
                {
                    fish.GetComponent<FishMouth>().Disable(_t.position);
                    fish.transform.parent.parent = _t;
                }
            }
        }
    }

}