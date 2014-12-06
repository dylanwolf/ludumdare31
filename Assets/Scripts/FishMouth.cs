using UnityEngine;
using System.Collections;

public class FishMouth : MonoBehaviour {

    Fish _fish;
    Collider2D _c;

	// Use this for initialization
	void Start () {
	    _fish = transform.parent.GetComponent<Fish>();
        _c = collider2D;
	}

    const string BAIT = "Bait";

    public void Disable(Vector3 hook)
    {
        _fish.enabled = false;
        foreach (Collider2D c in _fish.GetComponentsInChildren<Collider2D>())
        {
            c.enabled = false;
        }
        _fish.rigidbody2D.velocity = Vector3.zero;
        this.enabled = false;
        _fish.transform.position = hook;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.tag == BAIT)
        {
            _fish.Bait(collider.bounds.center, _c.bounds.center);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == BAIT)
        {
            _fish.RandomizeMovement();
        }
    }
}
