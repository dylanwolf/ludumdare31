using UnityEngine;
using System.Collections;

public class FishMouth : MonoBehaviour {

    [System.NonSerialized]
    public Fish Fish;
    Collider2D _c;

	// Use this for initialization
	void Start () {
	    Fish = transform.parent.GetComponent<Fish>();
        _c = collider2D;
	}

    const string BAIT = "Bait";
    const string HOOK = "Hook";

    public void Disable(Vector3 hook)
    {
        Fish.enabled = false;
        foreach (Collider2D c in Fish.GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }
        Fish.rigidbody2D.velocity = Vector3.zero;
        this.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == HOOK && Player.Current.State == Player.PlayerState.Fishing)
        {
            Hook.Current.HookedFishes.Add(this);
        }
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == BAIT)
        {
            Fish.Bait(collider.bounds.center, _c.bounds.center);              
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == BAIT)
        {
            Fish.RandomizeMovement();
        }

        if (collider.tag == HOOK && Player.Current.State == Player.PlayerState.Fishing)
        {
            Hook.Current.HookedFishes.Remove(this);
        }
    }
}
