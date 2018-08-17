﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 在房间列表里，单个的房间的消息信息
/// </summary>
public class RoomDetails : UIViewBase
{

    public Text roomID;
    public Text roomOwner;
    public Text perpleNumber;
    public Text status;
    public int _roomId;
    public GameObject room;

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "Join":
                UIDispacher.Instance.DispachEvent("Join", room);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}
