using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class GameTimerDisplay : MonoBehaviour {

    TextMesh _tm;
    private int lastSeconds;

    void Start()
    {
        _tm = GetComponent<TextMesh>();
    }

    const string SEP = ":";
    const string ZERO = "0";
    void Update()
    {
        if (Mathf.RoundToInt(GameState.Current.Times) != lastSeconds)
        {
            lastSeconds = Mathf.RoundToInt(GameState.Current.Times);
            _tm.text = (lastSeconds / 60) + SEP + (lastSeconds % 60 < 10 ? ZERO : string.Empty) + (lastSeconds % 60);
        }
    }
}
