using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle : MonoBehaviour {

    public Character Char1;
    public Character Char2;

	// Use this for initialization
	void Start () {
        Char1 = new Character(100, "Bob");
        Char2 = new Character(100, "Leo");
    }

    // Update is called once per frame
    void Update () {
		
	}
}
