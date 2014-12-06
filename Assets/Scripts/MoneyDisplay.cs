using UnityEngine;
using System.Collections;

[RequireComponent(typeof(TextMesh))]
public class MoneyDisplay : MonoBehaviour {

    TextMesh _tm;
    private int lastMoneys;

	void Start () {
        _tm = GetComponent<TextMesh>();
	}

    const string CASH = "$";
	void Update () {
	    if (GameState.Current.Moneys != lastMoneys)
        {
            _tm.text = CASH + GameState.Current.Moneys.ToString();
            lastMoneys = GameState.Current.Moneys;
        }
	}
}
