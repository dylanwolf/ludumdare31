using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameState : MonoBehaviour {

    public Transform FishPrefab;
    public int InitialFish = 5;

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
        Playing
    }

    void Awake()
    {
        Current = this;
    }

	void Start () {
        SpawnIceFloes();
        SpawnFish(InitialFish);
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

    void Update()
    {
        if (Fishes.Count < MaxFish)
        {
            FishTimer -= Time.deltaTime;
            if (FishTimer < 0)
            {
                SpawnFish(1);
                FishTimer = RandomFloat(MinFishTimer, MaxFishTimer);
            }
        }
    }
}
