using UnityEngine;
using System.Collections;

public class ResetButton : MonoBehaviour {

	void OnMouseUpAsButton()
    {
        GameState.Current.Reset();
    }
}
