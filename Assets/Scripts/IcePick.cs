using UnityEngine;
using System.Collections;

public class IcePick : MonoBehaviour {

    Player _p;

	// Use this for initialization
	void Start () {
        _p = transform.parent.GetComponent<Player>();
	}
	
    const string ICEFLOE = "IceFloe";

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == ICEFLOE)
        {
            _p.SetIcePick(collider);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == ICEFLOE)
        {
            _p.ClearIcePick(collider);
        }
    }
}
