using UnityEngine;
using System.Collections;

public class Store : MonoBehaviour {

    public float LowInterest = 1.0f;
    public float MedInterest = 2.0f;
    public float HighInterest = 3.0f;

    public float LowResilience = 0.5f;
    public float MedResilience = 0.3f;
    public float HighResilience = 0.1f;

    public float LowRadius = 0.2f;
    public float MedRadius = 0.5f;
    public float HighRadius = 1.0f;

    Transform _t;
    Renderer[] childRenderers;
    Transform Cursor;

    readonly int[] Costs = new int[] { 1, 1, 3, 3, 5, 5 };
    public Transform[] MenuOptions;
    public float YOffset = 0.03f;
    public float XOffset = -0.18f;

    public int SelectedItem = 0;

    public float MaxTimer = 0.25f;
    float Timer = 0;

    public static Store Current;

    void Awake()
    {
        Current = this;
    }

    void Start()
    {
        _t = transform;
        Cursor = _t.FindChild("Cursor");
        childRenderers = GetComponentsInChildren<Renderer>();
        Toggle(false);
    }

    public void ResetForShopping()
    {
        SelectedItem = 0;
        SetCursor();
    }

    public void Reset()
    {
        ResetForShopping();
        Toggle(false);
    }

    Vector3 tmpPos;
    public void SetCursor()
    {
        if (MenuOptions.Length > SelectedItem)
        {
            tmpPos = MenuOptions[SelectedItem].position;
            tmpPos.x += XOffset;
            tmpPos.y += YOffset;
            Cursor.transform.position = tmpPos;
        }
    }

    public void Toggle(bool active)
    {
        foreach (Renderer r in childRenderers)
        {
            r.enabled = active;
        }
    }

    void ApplyBonus()
    {
        switch (SelectedItem)
        {
            case 0:
                // Doughball: high interest, medium radius, low resilience
                Hook.SetBait(0, HighInterest, MedRadius, LowResilience);
                break;

            case 1:
                // Worm: med interest, high radius, low resilience
                Hook.SetBait(1, MedInterest, HighRadius, LowResilience);
                break;

            case 2:
                // Minnow: high interest, low radius, med resilience
                Hook.SetBait(2, HighInterest, LowRadius, MedResilience);
                break;

            case 3:
                // Plastic bait: low interest, high radius, med resilience
                Hook.SetBait(3, LowInterest, HighRadius, MedResilience);
                break;

            case 4:
                // Spoon: low intrest, medium radius, high resilience
                Hook.SetBait(4, LowInterest, MedRadius, HighResilience);
                break;

            case 5:
                // Jig: med interest, low radius, high resilience
                Hook.SetBait(5, MedInterest, LowRadius, HighResilience);
                break;
        }
    }

    const string VERTICAL = "Vertical";
    const string FIRE = "Fire1";
    const string HORIZONTAL = "Horizontal";

    float axisY;
    void Update()
    {
        if (Player.Current.State == Player.PlayerState.Shopping && GameState.Current.State == GameState.GlobalState.Playing)
        {
            if (Input.GetAxis(HORIZONTAL) < 0)
            {
                Player.Current.State = Player.PlayerState.Moving;
                Toggle(false);
            }

            if (Input.GetButtonUp(FIRE))
            {
                if (GameState.Current.Moneys < Costs[SelectedItem])
                {
                    // Do nothing
                    // Play an error sound here
                }
                else
                {
                    ApplyBonus();
                    GameState.Current.Moneys -= Costs[SelectedItem];
                    Player.Current.State = Player.PlayerState.Moving;
                    Toggle(false);
                }
            }

            if (Timer > 0)
                Timer -= Time.deltaTime;

            if (Timer <= 0 && Mathf.Abs(Input.GetAxis(VERTICAL)) > 0)
            {
                axisY = Input.GetAxis(VERTICAL);
                if (axisY > 0 && SelectedItem > 0)
                {
                    SelectedItem--;
                    Timer = MaxTimer;
                    SetCursor();
                }
                else if (axisY < 0 && SelectedItem < MenuOptions.Length - 1)
                {
                    SelectedItem++;
                    Timer = MaxTimer;
                    SetCursor();
                }
            }
        }
    }
}
