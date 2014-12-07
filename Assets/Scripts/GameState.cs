using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {

    [System.NonSerialized]
    public int Moneys;

    [System.NonSerialized]
    public float Times;

    public Transform FishPrefab;
    public int InitialFish = 5;

    [System.NonSerialized]
    public List<IceFloe> IceFloes = new List<IceFloe>();

    [System.NonSerialized]
    public List<Fish> Fishes = new List<Fish>();

    public Transform IcefloePrefab;
    public float MinIcefloeX = -4.25f;
    public float MaxIcefloeX = 2.25f;
    public float IcefloeWidth = 0.15f;

    float FishTimer = 0;
    public float MinFishTimer = 1.0f;
    public float MaxFishTimer = 3.0f;
    public int MaxFish = 10;

    public static GameState Current;

    public GlobalState State;

    public enum GlobalState
    {
        Playing,
        GameOver,
        Paused
    }

    void Awake()
    {
        Current = this;
    }

	void Start () {
        SpawnIceFloes();
        SpawnFish(InitialFish);
        SetStartingValues();
	}

    void SetStartingValues()
    {
        Moneys = 3;
        Times = 0.15f * 60;
        State = GlobalState.Paused;
    }

    public void Reset()
    {
        foreach (Fish f in Fishes)
        {
            DestroyObject(f.gameObject);
        }
        Fishes.Clear();
        SpawnFish(InitialFish);

        foreach (IceFloe f in IceFloes)
        {
            f.Reset();
        }

        Hook.Current.Reset();
        Player.Current.Reset();
        Store.Current.Reset();
        BaitIndicator.Current.Reset();

        SetStartingValues();
    }

    float x;
    Transform tmpObj;
    Vector3 tmpVector;
    void SpawnIceFloes()
    {
        x = MinIcefloeX;
        while (x < MaxIcefloeX)
        {
            tmpObj = (Transform)Instantiate(IcefloePrefab);
            tmpVector = tmpObj.position;
            tmpVector.x = x;
            tmpObj.position = tmpVector;

            x += IcefloeWidth;

            IceFloes.Add(tmpObj.GetComponent<IceFloe>());
        }
    }

    void SpawnFish(int number)
    {
        for (int i =0; i < number; i++)
        {
            Fishes.Add(((Transform)Instantiate(FishPrefab)).GetComponent<Fish>());
        }
    }

    float RandomFloat(float min, float max)
    {
        return ((max - min) * Random.value) + min;
    }

    void FixedUpdate()
    {
        if (State == GlobalState.Playing)
        {
            Times -= Time.fixedDeltaTime;
            if (Times < 0)
            {
                SoundBoard.PlayMusic(false);
                SoundBoard.PlayTimeUp();
                Player.Current.DoAnimation(Player.ANIM_STAND);
                BaitIndicator.Current.Reset();
                State = GlobalState.GameOver;
            }
        }
    }

    const string FIRE = "Fire1";

    void Update()
    {
        if (Fishes.Count < MaxFish && State == GlobalState.Playing)
        {
            FishTimer -= Time.deltaTime;
            if (FishTimer < 0)
            {
                SpawnFish(1);
                FishTimer = RandomFloat(MinFishTimer, MaxFishTimer);
            }
        }
        else if (State == GlobalState.Paused)
        {
            if (Input.GetButtonUp(FIRE))
            {
                SoundBoard.PlayStartup();
                SoundBoard.PlayMusic(true);
                State = GlobalState.Playing;
            }
        }
    }
}
