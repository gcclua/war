﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Soldier;
public class PlayerView : MonoBehaviour {

    Vector2 mouseScreenPos;
	// Use this for initialization
	void Start () {
		if(PlayerCtrl.Camp == Camp.Dark)
        {
            transform.position = new Vector3(-15, 15, -9);
        }
        if(PlayerCtrl.Camp == Camp.Bright)
        {
            transform.position = new Vector3(17, 15, -9);
        }
	}
	
	// Update is called once per frame
	void Update () {
        mouseScreenPos = Input.mousePosition;
        if(mouseScreenPos.x <= 1 && transform.position.x >= -18)
        {
            transform.Translate(new Vector3(-1, 0, 0) * Time.deltaTime * 7);
        }
        if(mouseScreenPos.x >= Screen.width - 1 && transform.position.x <= 18)
        {
            transform.Translate(new Vector3(1, 0, 0) * Time.deltaTime * 7);
        }
	}
}