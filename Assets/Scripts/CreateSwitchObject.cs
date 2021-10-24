using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSwitchObject : MonoBehaviour
{
	public GameObject switchOutline;
	public GameObject rj45white;
	public GameObject rj45black;

	private GameObject newEmptyObject;
    // Start is called before the first frame update
    void Start()
    {
		// Instantiate(newEmptyObject);

		Instantiate(switchOutline);
		Instantiate(rj45white);

		// switchOutline.transform.parent = newEmptyObject.transform;
		rj45white.transform.parent = switchOutline.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
