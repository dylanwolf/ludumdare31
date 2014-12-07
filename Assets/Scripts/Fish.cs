using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {

    private Rigidbody2D _r;
    private Transform _t;

    public float MinSpawnX = -3;
    public float MaxSpawnX = 1;
    public float MinSpawnY = -0.5f;
    public float MaxSpawnY = 1.5f;

    public float MinX = -4.25f;
    public float MaxX = 2.25f;

    public float MinTimer = 1.0f;
    public float MaxTimer = 2.0f;
    float Timer = 0;

    public float MinScale = 0.5f;
    public float MaxScale = 2.0f;

    public float MinSpeed = 0.1f;
    public float MaxSpeed = 0.5f;

    Vector3 FrozenSpeed;
    bool IsFrozen = false;
    bool IsBaited = false;

    public float MinInterest = -5.0f;
    public float Interest;

    Animator anim;

    public int ScoreValue()
    {
        if (_t.localScale.x <= 1f || _t.localScale.x >= 2.25f)
            return 3;
        else if (_t.localScale.x <= 1.25f || _t.localScale.x >= 2.0f)
            return 2;
        return 1;
    }

	void Start () {
        _r = rigidbody2D;
        _t = transform;
        anim = GetComponent<Animator>();

        Interest = Hook.Current.MaxInterest;
        RandomizeFish();
	}

    float RandomFloat(float min, float max)
    {
        return ((max - min) * Random.value) + min;
    }

    private Vector3 tmpVector;
    void RandomizeFish()
    {
        tmpVector.y = tmpVector.x = RandomFloat(MinScale, MaxScale);
        tmpVector.z = 1;
        _t.localScale = tmpVector;

        tmpVector.x = RandomFloat(MinSpawnX, MaxSpawnX);
        tmpVector.y = RandomFloat(MinSpawnY, MaxSpawnY);
        tmpVector.z = 0;
        _t.position = tmpVector;

        RandomizeMovement();
    }

    public void ReelingIn()
    {
        anim.speed = 3.0f;
    }

    float speed;
    public void RandomizeMovement()
    {
        speed = RandomFloat(MinSpeed, MaxSpeed);
        _r.velocity = speed * Random.onUnitSphere;

        anim.speed = speed;

        tmpVector = _t.localScale;
        tmpVector.x = Mathf.Abs(tmpVector.x) * -Mathf.Sign(_r.velocity.x);
        _t.localScale = tmpVector;

        Timer = RandomFloat(MinTimer, MaxTimer);
        IsBaited = false;

        // Only reset interest if the fish isn't disinterested--otherwise, run out the timer
        if (Interest >= 0)
        {
            Interest = Hook.Current.MaxInterest;
        }
    }
	
	void Update () {
        if (GameState.Current.State == GameState.GlobalState.Playing)
        {
            if (IsFrozen)
            {
                _r.velocity = FrozenSpeed;
                IsFrozen = false;
            }

            // Count down timer if the fish is on the hook, or if it's disinterested
            if (IsBaited || Interest <= 0)
            {
                Interest -= Time.deltaTime;
                if (IsBaited && Interest < 0)
                {
                    RandomizeMovement();
                }
                if (Interest <= MinInterest)
                {
                    Interest = Hook.Current.MaxInterest;
                    IsBaited = false;
                }
            }

            if (!IsBaited)
            {
                if (Timer >= 0)
                {
                    Timer -= Time.deltaTime;
                }
                else
                {
                    RandomizeMovement();
                }
            }
        }
        else if (GameState.Current.State != GameState.GlobalState.GameOver)
        {
            if (!IsFrozen)
            {
                FrozenSpeed = _r.velocity;
                _r.velocity = Vector3.zero;
                IsFrozen = true;
            }
        }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag != this.tag)
            RandomizeMovement();
    }

    public void Bait(Vector3 bait, Vector3 mouth)
    {
        if (Player.Current.State != Player.PlayerState.Fishing)
        {
            IsBaited = false;
            return;
        }

        // Only bait until the fish is no longer interested
        if (Interest >= 0)
        {
            tmpVector = (bait - mouth).normalized * MaxSpeed;
            tmpVector.z = 0;
            _r.velocity = tmpVector;
            anim.speed = MaxSpeed;

            // If we haven't yet been baited, flip to the direction of the hook
            if (!IsBaited)
            {
                tmpVector = _t.localScale;
                tmpVector.x = Mathf.Abs(tmpVector.x) * -Mathf.Sign(_r.velocity.x);
                _t.localScale = tmpVector;
                IsBaited = true;
            }
        }
    }
}
