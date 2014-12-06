using UnityEngine;
using System.Collections;

public class HookTrigger : MonoBehaviour {

    Hook _h;

	void Start () {
        _h = GetComponentInParent<Hook>();
	}

    const string FISH = "Fish";

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (Player.CurrentState != Player.PlayerState.Fishing)
            return;

        if (collider.tag == FISH)
        {
            Debug.Log("hooked " + collider.gameObject.name);
            _h._fish.Add(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (Player.CurrentState != Player.PlayerState.Fishing)
            return;

        if (collider.tag == FISH)
        {
            Debug.Log("unhooked " + collider.gameObject.name);
            _h._fish.Remove(collider);
        }
    }
}
