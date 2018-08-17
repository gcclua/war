﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Soldier;

public class GameResultCtrl : MonoBehaviour
{

    public GameResultView gameResultView;
    // Use this for initialization
    void Start()
    {
        //胜利
        if (GameResult.Instance.IsWinGame)
        {
            gameResultView.resultText.text = "游戏胜利";
            gameResultView.resultText.color = new Color32(22, 114, 149, 255);
        }
        else
        {
            gameResultView.resultText.text = "游戏失败";
            gameResultView.resultText.color = new Color32(190, 65, 52, 255);
        }

        Dictionary<Int32, mmopb.p_gameResultInfo> resultList = GameResult.Instance.ResultList;
        foreach(KeyValuePair<Int32,mmopb.p_gameResultInfo> info in resultList)
        {
            if (info.Key == LocalUser.Instance.PlayerId)
            {
                gameResultView.soldire_me.text = info.Value.createSoldierNum.ToString();
                gameResultView.building_me.text = info.Value.createBuildingNum.ToString();
                gameResultView.energy_me.text = info.Value.energyRewardNum.ToString();
                gameResultView.goldcoin_me.text = info.Value.goldCoin.ToString();
                LocalUser.Instance.User_coin += info.Value.goldCoin;
            }
            else
            {
                gameResultView.soldire_friend.text = info.Value.createSoldierNum.ToString();
                gameResultView.building_friend.text = info.Value.createBuildingNum.ToString();
                gameResultView.energy_friend.text = info.Value.energyRewardNum.ToString();
                gameResultView.goldcoin_friend.text = info.Value.goldCoin.ToString();
            }
        }

        //事件
        UIDispacher.Instance.AddEventListener("ClickConfirm", ClickConfirm);
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("ClickConfirm", ClickConfirm);
    }

    //点击游戏结果界面的确认按钮
    private void ClickConfirm(object param)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
       
    }
}
