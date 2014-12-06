using UnityEngine;
using System.Collections;

public class InstructionsDisplay : MonoBehaviour {

    Transform _t;

    Player.PlayerState? lastPlayerState;
    GameState.GlobalState? lastGlobalState;

    Renderer[] IcePickRenderers;
    Renderer[] FishRenderers;
    Renderer[] MoveRenderers;
    Renderer[] ShopRenderers;
    Renderer[] ReelRenderers;
    Renderer[] StartRenderers;

    const string ICEPICK = "IcePick";
    const string FISH = "Fish";
    const string MOVE = "Move";
    const string SHOP = "Shop";
    const string REEL = "Reel";
    const string START = "Start";

	void Start () {
        _t = transform;
        IcePickRenderers = CollectRenderers(ICEPICK);
        FishRenderers = CollectRenderers(FISH);
        MoveRenderers = CollectRenderers(MOVE);
        ShopRenderers = CollectRenderers(SHOP);
        ReelRenderers = CollectRenderers(REEL);
        StartRenderers = CollectRenderers(START);
	}

    Renderer[] CollectRenderers(string parentName)
    {
        return _t.FindChild(parentName).GetComponentsInChildren<Renderer>();
    }

    void Toggle(Renderer[] renderers, bool active)
    {
        foreach (Renderer r in renderers)
        {
            r.enabled = active;
        }
    }

    bool lastCanFish;

    void Update()
    {

        if (!lastPlayerState.HasValue || lastPlayerState.Value != Player.Current.State || lastCanFish != Player.Current.CanFish || !lastGlobalState.HasValue || lastGlobalState != GameState.Current.State)
        {
            if (GameState.Current.State == GameState.GlobalState.Paused)
            {
                Toggle(FishRenderers, false);
                Toggle(IcePickRenderers, false);
                Toggle(MoveRenderers, false);
                Toggle(ShopRenderers, false);
                Toggle(ReelRenderers, false);
                Toggle(StartRenderers, true);
            }
            else
            {
                switch (Player.Current.State)
                {
                    case Player.PlayerState.Fishing:
                        Toggle(FishRenderers, true);
                        Toggle(IcePickRenderers, false);
                        Toggle(MoveRenderers, false);
                        Toggle(ShopRenderers, false);
                        Toggle(ReelRenderers, true);
                        Toggle(StartRenderers, false);
                        break;

                    case Player.PlayerState.Moving:
                        Toggle(FishRenderers, Player.Current.CanFish);
                        Toggle(IcePickRenderers, !Player.Current.CanFish);
                        Toggle(MoveRenderers, true);
                        Toggle(ShopRenderers, false);
                        Toggle(ReelRenderers, false);
                        Toggle(StartRenderers, false);
                        break;

                    case Player.PlayerState.Reeling:
                        Toggle(FishRenderers, false);
                        Toggle(IcePickRenderers, false);
                        Toggle(MoveRenderers, false);
                        Toggle(ShopRenderers, false);
                        Toggle(ReelRenderers, false);
                        Toggle(StartRenderers, false);
                        break;

                    case Player.PlayerState.Shopping:
                        Toggle(FishRenderers, false);
                        Toggle(IcePickRenderers, false);
                        Toggle(MoveRenderers, false);
                        Toggle(ShopRenderers, true);
                        Toggle(ReelRenderers, false);
                        Toggle(StartRenderers, false);
                        break;
                }
            }

            lastGlobalState = GameState.Current.State;
            lastPlayerState = Player.Current.State;
            lastCanFish = Player.Current.CanFish;
        }
    }
}
