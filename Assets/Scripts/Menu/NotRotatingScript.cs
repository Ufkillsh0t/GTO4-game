using UnityEngine;
using System.Collections;

public class NotRotatingScript : MonoBehaviour {

    private Quaternion rotation;
    public bool rotateWithObject;

    void Awake()
    {
        rotation = transform.rotation;
    }

	// Update is called once per frame
	void LateUpdate () {
        if (!rotateWithObject)
        {
            transform.rotation = rotation;
        }
	}
}
