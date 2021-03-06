﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Soldier;
using DG.Tweening;

public class CardCtrl : MonoBehaviour
{
    private Dictionary<SoldierEnum, Card> cards=new Dictionary<SoldierEnum, Card>();
    private Image card_bg;
    private RectTransform rectCard;
    private GameObject last_selected_card = null;
    public static CardCtrl instance;
    public AudioClip clickCard;
    private AudioSource audioSource;

    public void Awake()
    {
        instance = this;
    }
    // Use this for initialization
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();

        //音量设置
        GameObject canvas = GameObject.Find("Cube");
        AudioSource audio = canvas.GetComponent<AudioSource>();
        audio.volume = LocalUser.Instance.Voice;
        audio.mute = LocalUser.Instance.IsMute;
        //事件
        UIDispacher.Instance.AddEventListener("ClickCard", OnClickOfCard);
        UIDispacher.Instance.AddEventListener("StayOnCard", StayOnCard);
        UIDispacher.Instance.AddEventListener("ExitCard", ExitCard);

        InitCards();
        LoadCards();
        InitPlayer();
    }

    public void Update()
    {
        //音量设置
        GameObject canvas = GameObject.Find("Cube");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("ClickCard", OnClickOfCard);
        UIDispacher.Instance.RemoveEventListener("StayOnCard", StayOnCard);
        UIDispacher.Instance.RemoveEventListener("ExitCard", ExitCard);
    }
    
    /// <summary>
    /// 初始化卡片
    /// </summary>
    private void InitCards()
    {
        //兽族刺客
        Card Orc_Assassin = new Card();
        Orc_Assassin.Name = Constant.Orc_Assassin_Name;
        Orc_Assassin.Img = Resources.Load(Constant.Orc_Assassin_ImgPath, typeof(Sprite)) as Sprite;
        Orc_Assassin.Energy = Constant.Orc_Assassin_Energy;
        Orc_Assassin.SoldierType = SoldierEnum.Orc_Assassin;
        Orc_Assassin.Role_info = Constant.Orc_Assassin_Info;
        cards.Add(SoldierEnum.Orc_Assassin, Orc_Assassin);

        //兽族法师
        Card Orc_Magic = new Card();
        Orc_Magic.Name = Constant.Orc_Magic_Name;
        Orc_Magic.Img = Resources.Load(Constant.Orc_Magic_ImgPath, typeof(Sprite)) as Sprite;
        Orc_Magic.Energy = Constant.Orc_Magic_Energy;
        Orc_Magic.SoldierType = SoldierEnum.Orc_Magic;
        Orc_Magic.Role_info = Constant.Orc_Magic_Info;
        cards.Add(SoldierEnum.Orc_Magic, Orc_Magic);

        //兽族投石手
        Card Orc_Scout = new Card();
        Orc_Scout.Name = Constant.Orc_Scout_Name;
        Orc_Scout.Img = Resources.Load(Constant.Orc_Scout_ImgPath, typeof(Sprite)) as Sprite;
        Orc_Scout.Energy = Constant.Orc_Scout_Energy;
        Orc_Scout.SoldierType = SoldierEnum.Orc_Scout;
        Orc_Scout.Role_info = Constant.Orc_Scout_Info;
        cards.Add(SoldierEnum.Orc_Scout, Orc_Scout);

        //兽族剑士
        Card Orc_Swordsman = new Card();
        Orc_Swordsman.Name = Constant.Orc_Swordsman_Name;
        Orc_Swordsman.Img = Resources.Load(Constant.Orc_Swordsman_ImgPath, typeof(Sprite)) as Sprite;
        Orc_Swordsman.Energy = Constant.Orc_Swordsman_Energy;
        Orc_Swordsman.SoldierType = SoldierEnum.Orc_Swordsman;
        Orc_Swordsman.Role_info = Constant.Orc_Swordsman_Info;
        cards.Add(SoldierEnum.Orc_Swordsman, Orc_Swordsman);

        //兽族步兵
        Card Orc_Infantry = new Card();
        Orc_Infantry.Name = Constant.Orc_Infantry_Name;
        Orc_Infantry.Img = Resources.Load(Constant.Orc_Infantry_ImgPath, typeof(Sprite)) as Sprite;
        Orc_Infantry.Energy = Constant.Orc_Infantry_Energy;
        Orc_Infantry.SoldierType = SoldierEnum.Orc_Infantry;
        Orc_Infantry.Role_info = Constant.Orc_Infantry_Info;
        cards.Add(SoldierEnum.Orc_Infantry, Orc_Infantry);

        //人族工人
        Card Terren_Worker = new Card();
        Terren_Worker.Name = Constant.Terren_Worker_Name;
        Terren_Worker.Img = Resources.Load(Constant.Terren_Worker_ImgPath, typeof(Sprite)) as Sprite;
        Terren_Worker.Energy = Constant.Terren_Worker_Energy;
        Terren_Worker.SoldierType = SoldierEnum.Terren_Worker;
        Terren_Worker.Role_info = Constant.Terren_Worker_Info;
        cards.Add(SoldierEnum.Terren_Worker, Terren_Worker);

        //人族投矛手
        Card Terren_Spear = new Card();
        Terren_Spear.Name = Constant.Terren_Spear_Name;
        Terren_Spear.Img = Resources.Load(Constant.Terren_Spear_ImgPath, typeof(Sprite)) as Sprite;
        Terren_Spear.Energy = Constant.Terren_Spear_Energy;
        Terren_Spear.SoldierType = SoldierEnum.Terren_Spear;
        Terren_Spear.Role_info = Constant.Terren_Spear_Info;
        cards.Add(SoldierEnum.Terren_Spear, Terren_Spear);

        //人类骑兵
        Card Terren_Cavalry = new Card();
        Terren_Cavalry.Name = Constant.Terren_Cavalry_Name;
        Terren_Cavalry.Img = Resources.Load(Constant.Terren_Cavalry_ImgPath, typeof(Sprite)) as Sprite;
        Terren_Cavalry.Energy = Constant.Terren_Cavalry_Energy;
        Terren_Cavalry.SoldierType = SoldierEnum.Terren_Cavalry;
        Terren_Cavalry.Role_info = Constant.Terren_Cavalry_Info;
        cards.Add(SoldierEnum.Terren_Cavalry, Terren_Cavalry);

        //人族弓箭手
        Card Terren_Archer = new Card();
        Terren_Archer.Name = Constant.Terren_Archer_Name;
        Terren_Archer.Img = Resources.Load(Constant.Terren_Archer_ImgPath, typeof(Sprite)) as Sprite;
        Terren_Archer.Energy = Constant.Terren_Archer_Energy;
        Terren_Archer.SoldierType = SoldierEnum.Terren_Archer;
        Terren_Archer.Role_info = Constant.Terren_Archer_Info;
        cards.Add(SoldierEnum.Terren_Archer, Terren_Archer);

        //人族步兵
        Card Terren_Infantry = new Card();
        Terren_Infantry.Name = Constant.Terren_Infantry_Name;
        Terren_Infantry.Img = Resources.Load(Constant.Terren_Infantry_ImgPath, typeof(Sprite)) as Sprite;
        Terren_Infantry.Energy = Constant.Terren_Infantry_Energy;
        Terren_Infantry.SoldierType = SoldierEnum.Terren_Infantry;
        Terren_Infantry.Role_info = Constant.Terren_Infantry_Info;
        cards.Add(SoldierEnum.Terren_Infantry, Terren_Infantry);
    }

    /// <summary>
    /// 初始化玩家
    /// </summary>
    private void InitPlayer()
    {
        PlayerCtrl playerCtrl = new GameObject("PlayerCtrl").AddComponent<PlayerCtrl>();

        playerCtrl.Init(LocalUser.Instance.PlayerId,(Camp)LocalUser.Instance.Camp);
    }
    
    /// <summary>
    /// 加载卡片
    /// </summary>
    private void LoadCards()
    {
        //LocalUser.Instance.Camp = 2;
        if (LocalUser.Instance.Camp == (int)Camp.Dark)
        {
            int i = 1;
            GameObject[] _cards = GameObject.FindGameObjectsWithTag("CARD");
            foreach (GameObject card in _cards)
            {
                CardView cardView = card.GetComponentInChildren<CardView>();
                Card card_temp = null;
                if (cards.TryGetValue((SoldierEnum)i, out card_temp) == true)
                {
                    cardView.role_name.text = card_temp.Name;
                    cardView.role_img.sprite = card_temp.Img;
                    cardView.role_energy.text = card_temp.Energy.ToString();
                    cardView.soldierType = card_temp.SoldierType;
                    cardView.role_info_text.text = card_temp.Role_info.Replace('n', '\n');
                }
                i++;
            }
        }

        if (LocalUser.Instance.Camp == (int)Camp.Bright)
        {
            int i = 6;
            GameObject[] _cards = GameObject.FindGameObjectsWithTag("CARD");
            foreach(GameObject card in _cards)
            {
                CardView cardView = card.GetComponentInChildren<CardView>();
                Card card_temp=null;
                if(cards.TryGetValue((SoldierEnum)i,out card_temp) == true)
                {
                    cardView.role_name.text = card_temp.Name;
                    cardView.role_img.sprite = card_temp.Img;
                    cardView.role_energy.text = card_temp.Energy.ToString();
                    cardView.soldierType = card_temp.SoldierType;
                    cardView.role_info_text.text = card_temp.Role_info.Replace('n', '\n');
                }
                i++;
            }
        }
    }

    /// <summary>
    /// 点击卡片
    /// </summary>
    /// <param name="param"></param>
    private void OnClickOfCard(object param)
    {
        audioSource.clip = clickCard;
        audioSource.Play();
        GameObject card = (GameObject)param;
        
        if (last_selected_card == null)
        {
            last_selected_card = card;
        }
        else
        {
            if (last_selected_card != card)
            {
                Image _card_bg = last_selected_card.GetComponentInChildren<Image>();
                _card_bg.color = new Color32(48, 187, 176, 185);
                RectTransform _rectCard = last_selected_card.GetComponent<RectTransform>();
                _rectCard.DOLocalMoveY(-0.1f, 0.5f);
                last_selected_card = card;
            }
        }
        
        card_bg = card.GetComponentInChildren<Image>();
        CardView cardView = card.gameObject.GetComponent<CardView>();
        LocalUser.Instance.CardStatus = cardView.soldierType;
        card_bg.color = new Color32(0, 0, 0, 162);
        rectCard = card.GetComponent<RectTransform>();
        rectCard.DOLocalMoveY(80f, 0.5f);
    }

    /// <summary>
    /// 鼠标在卡牌上停留显示卡牌信息
    /// </summary>
    /// <param name="param"></param>
    private void StayOnCard(object param)
    {
        GameObject card = (GameObject)param;
        CardView cardView = card.GetComponent<CardView>();
        cardView.role_info.SetActive(true);
    }

    /// <summary>
    /// 鼠标离开了卡牌
    /// </summary>
    /// <param name="param"></param>
    private void ExitCard(object param)
    {
        GameObject card = (GameObject)param;
        CardView cardView = card.GetComponent<CardView>();
        cardView.role_info.SetActive(false);
    }

    /// <summary>
    /// 外部调用，重置卡片
    /// </summary>
    public void ResetCardColor()
    {
        card_bg.color = new Color32(48, 187, 176, 185);
        rectCard.DOLocalMoveY(-0.1f, 0.5f);
        LocalUser.Instance.CardStatus = SoldierEnum.None;
    }
}
