﻿using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLobbyCtrl : MonoBehaviour {
    
    public Transform root;
    public GameLobbyView lobbyView;
    public FriendView friendView;
    public GameObject musicButton;
    public GameObject voice;

    //请求添加好友玩家的id
    private int roleId;
    //发送消息的好友id
    private int roleId_chat;
    //邀请自己玩游戏的玩家id
    private int roleId_invite;

    //消息内容列表
    Dictionary<int, LinkedList<Message>> messages = new Dictionary<int, LinkedList<Message>>();

    //按钮声音特效
    public AudioSource button;
    //一般按钮声音
    public AudioClip common;
    //发送消息按钮声音
    public AudioClip sendMsg;
    //消息声音
    public AudioSource audio_message;
    //通知声音
    public AudioSource audio_notice;

    void Start()
    {
        //事件
        UIDispacher.Instance.AddEventListener("Click1V1", Click1V1);
        UIDispacher.Instance.AddEventListener("ClickFriends", ClickFriends);
        UIDispacher.Instance.AddEventListener("ClickYesNewFriend", ClickYesNewFriend);
        UIDispacher.Instance.AddEventListener("ClickNoNewFriend", ClickNoNewFriend);
        UIDispacher.Instance.AddEventListener("ClickYesNewInvite", ClickYesNewInvite);
        UIDispacher.Instance.AddEventListener("ClickNoNewInvite", ClickNoNewInvite);
        UIDispacher.Instance.AddEventListener("ClickAddFriend", ClickAddFriend);
        UIDispacher.Instance.AddEventListener("ClickCloseFriends", ClickCloseFriends);
        UIDispacher.Instance.AddEventListener("ClickDeleteFriend", ClickDeleteFriend);
        UIDispacher.Instance.AddEventListener("ClickSendToFriend", ClickSendToFriend);
        UIDispacher.Instance.AddEventListener("ClickSendToTargetFriend", ClickSendToTargetFriend);
        UIDispacher.Instance.AddEventListener("ClickCloseChat", ClickCloseChat);
        UIDispacher.Instance.AddEventListener("ClickLogoutAccount", ClickLogoutAccount);
        UIDispacher.Instance.AddEventListener("ClickMusicButton", ClickMusicButton);
        UIDispacher.Instance.AddEventListener("ClickSetting", ClickSetting);
        

        NetDispacher.Instance.AddEventListener("friendsList_ack", LoadFriendsHandle);
        NetDispacher.Instance.AddEventListener("addFriend_ack", AddFriendHandle);
        NetDispacher.Instance.AddEventListener("deleteFriend_ack", DeleteFriendHandle);
        NetDispacher.Instance.AddEventListener("playerAddFriend_ecn", AddFriendMsgFromServerHandle);
        NetDispacher.Instance.AddEventListener("sendMessageToOnlineFriend_ecn", RevNewMessageHandle);
        NetDispacher.Instance.AddEventListener("noticeInviteFriend_ecn", RevInviteMsgHandle);
        NetDispacher.Instance.AddEventListener("inviteResult_ack", RevInviteResultHandle);
        NetDispacher.Instance.AddEventListener("roleInfoInRoom_bcst", RoleInfoInRoomHandle);
        NetDispacher.Instance.AddEventListener("exitGame_ack", LogoutHandle);


        //显示账号 昵称
        GameObject account = GameObject.Find("Account");
        Text account_text = account.GetComponent<Text>();
        account_text.text = "账户：" + LocalUser.Instance.User_account;
        GameObject name = GameObject.Find("Name");
        Text name_text = name.GetComponent<Text>();
        name_text.text = "昵称：" + LocalUser.Instance.User_nickname;

        //加载金币钻石数据
        GameObject goldcoin = GameObject.Find("GoldCoin");
        Text goldcoin_text = goldcoin.GetComponentInChildren<Text>();
        goldcoin_text.text = LocalUser.Instance.User_coin.ToString();
        GameObject diamond = GameObject.Find("Diamond");
        Text diamond_text = diamond.GetComponentInChildren<Text>();
        diamond_text.text = LocalUser.Instance.User_diamond.ToString();

        //设置初始化
        Text button_text = musicButton.GetComponentInChildren<Text>();
        if (LocalUser.Instance.IsMute)
        {
            button_text.text = "打开";
        }
        else
        {
            button_text.text = "关闭";
        }
        Scrollbar scrollbar = voice.GetComponent<Scrollbar>();
        scrollbar.value = LocalUser.Instance.Voice;

        //音量设置
        GameObject canvas = GameObject.Find("Canvas");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;
    }

    public void Update()
    {
        //音量设置
        GameObject canvas = GameObject.Find("Canvas");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;
    }

    public void OnDestroy()
    {
        UIDispacher.Instance.RemoveEventListener("Click1V1", Click1V1);
        UIDispacher.Instance.RemoveEventListener("ClickYesNewFriend", ClickYesNewFriend);
        UIDispacher.Instance.RemoveEventListener("ClickNoNewFriend", ClickNoNewFriend);
        UIDispacher.Instance.RemoveEventListener("ClickFriends", ClickFriends);
        UIDispacher.Instance.RemoveEventListener("ClickAddFriend", ClickAddFriend);
        UIDispacher.Instance.RemoveEventListener("ClickCloseFriends", ClickCloseFriends);
        UIDispacher.Instance.RemoveEventListener("ClickDeleteFriend", ClickDeleteFriend);
        UIDispacher.Instance.RemoveEventListener("ClickSendToFriend", ClickSendToFriend);
        UIDispacher.Instance.RemoveEventListener("ClickSendToTargetFriend", ClickSendToTargetFriend); 
        UIDispacher.Instance.RemoveEventListener("ClickCloseChat", ClickCloseChat);
        UIDispacher.Instance.RemoveEventListener("ClickYesNewInvite", ClickYesNewInvite);
        UIDispacher.Instance.RemoveEventListener("ClickNoNewInvite", ClickNoNewInvite);
        UIDispacher.Instance.RemoveEventListener("ClickLogoutAccount", ClickLogoutAccount);
        UIDispacher.Instance.RemoveEventListener("ClickMusicButton", ClickMusicButton);
        UIDispacher.Instance.RemoveEventListener("ClickSetting", ClickSetting);

        NetDispacher.Instance.RemoveEventListener("friendsList_ack", LoadFriendsHandle);
        NetDispacher.Instance.RemoveEventListener("addFriend_ack", AddFriendHandle);
        NetDispacher.Instance.RemoveEventListener("deleteFriend_ack", DeleteFriendHandle);
        NetDispacher.Instance.RemoveEventListener("playerAddFriend_ecn", AddFriendMsgFromServerHandle);
        NetDispacher.Instance.RemoveEventListener("sendMessageToOnlineFriend_ecn", RevNewMessageHandle);
        NetDispacher.Instance.RemoveEventListener("noticeInviteFriend_ecn", RevInviteMsgHandle);
        NetDispacher.Instance.RemoveEventListener("inviteResult_ack", RevInviteResultHandle);
        NetDispacher.Instance.RemoveEventListener("roleInfoInRoom_bcst", RoleInfoInRoomHandle);
        NetDispacher.Instance.RemoveEventListener("exitGame_ack", LogoutHandle);
    }
    
    /// <summary>
    /// 改变设置的音量时调用
    /// </summary>
    public void OnChangeValue()
    {
        //改变音量
        Scrollbar scrollbar = voice.GetComponent<Scrollbar>();
        LocalUser.Instance.Voice = scrollbar.value;
    }


    /// <summary>
    /// 点击游戏大厅的1v1按钮
    /// </summary>
    /// <param name="param"></param>
    private void Click1V1(object param)
    {
        button.clip = common;
        button.Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Rooms");
    }

    /// <summary>
    /// 点击游戏大厅的朋友按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickFriends(object param)
    {
        button.clip = common;
        button.Play();
        var friendReq = new mmopb.friendsList_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(friendReq));

        friendView.friends.SetActive(true);
        lobbyView.partHome.SetActive(false);
        lobbyView.newMessage.SetActive(false);
    }

    /// <summary>
    /// 点击游戏大厅的设置按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickSetting(object param)
    {
        if (lobbyView.settingDetail.activeInHierarchy)
        {
            lobbyView.settingDetail.SetActive(false);
        }
        else
        {
            lobbyView.settingDetail.SetActive(true);
        }
        
    }


    /// <summary>
    /// 点击好友界面的添加好友按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickAddFriend(object param)
    {
        button.clip = common;
        button.Play();
        var addFriendMsg = new mmopb.addFriend_req();
        string friendName = friendView.friendNameAdd.text;
        addFriendMsg.nickName = friendName;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(addFriendMsg));
    }

    /// <summary>
    /// 在好友列表界面点击右上角的叉叉，关闭显示大厅
    /// </summary>
    /// <param name="param"></param>
    private void ClickCloseFriends(object param)
    {
        button.clip = common;
        button.Play();
        //删除原来有的列表
        GameObject[] friends = GameObject.FindGameObjectsWithTag("FRIEND");
        foreach (GameObject friend in friends)
        {
            Destroy(friend);
        }
        friendView.friends.SetActive(false);
        lobbyView.partHome.SetActive(true);

    }

    /// <summary>
    /// 在好友列表界面点击删除按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickDeleteFriend(object param)
    {
        button.clip = common;
        button.Play();
        var deleteMsg = new mmopb.deleteFriend_req();
        GameObject friend = (GameObject)param;
        FriendDetail friendDetail = friend.GetComponentInChildren<FriendDetail>();
        deleteMsg.roleId = friendDetail.friendId;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(deleteMsg));
    }

    /// <summary>
    /// 点击好友列表页面的发送消息按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickSendToFriend(object param)
    {
        button.clip = sendMsg;
        button.Play();
        var chatMsg = new mmopb.sendMessageToOnlineFriend_req();
        friendView.chatPanle.SetActive(true);
        GameObject friend = (GameObject)param;
        FriendDetail friendDetail = friend.GetComponentInChildren<FriendDetail>();
        roleId_chat = friendDetail.friendId;
        friendDetail.messageNotice.SetActive(false);

        //加载预制体
        GameObject _message = Resources.Load("MessageFriend") as GameObject;
       
        LinkedList<Message> friendMsg;
        if(messages.TryGetValue(roleId_chat,out friendMsg))
        {
            foreach(Message friend_message in friendMsg)
            {
                // 对象初始化
                GameObject message = Instantiate(_message, friendView.chatParentTransForm);
                MessageDetail messageDetail = message.GetComponent<MessageDetail>();
                
                DateTime dt = DateTime.Parse("1970-01-01 00:00:00").AddSeconds(friend_message.Time);
                dt = dt.AddSeconds(8 * 60 * 60);     // +8h的时差  
                string time = dt.ToString("HH:mm:ss");
                messageDetail.time.text = "【" + time + "】";
                messageDetail.name.text = friend_message.Name;
                messageDetail.content.text = "：" + friend_message.Content;
            }
        }
    }

    /// <summary>
    /// 在聊天框点击发送按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickSendToTargetFriend(object param)
    {
        button.clip = sendMsg;
        button.Play();
        var sendMessageToOnlineFriend_req = new mmopb.sendMessageToOnlineFriend_req();
        sendMessageToOnlineFriend_req.content = friendView.chatMsg.text;
        sendMessageToOnlineFriend_req.roleId = roleId_chat;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(sendMessageToOnlineFriend_req));

        //加载预制体
        GameObject _message = Resources.Load("MessageMe") as GameObject;
        // 对象初始化
        GameObject message = Instantiate(_message, friendView.chatParentTransForm);
        Text myContent = message.GetComponentInChildren<Text>();
        myContent.text = friendView.chatMsg.text;

        //清空聊天框
        friendView.chatMsg.text = "";
    }

    /// <summary>
    /// 点击关闭好友聊天
    /// </summary>
    /// <param name="param"></param>
    private void ClickCloseChat(object param)
    {
        button.clip = common;
        button.Play();
        friendView.chatPanle.SetActive(false);
        GameObject[] messages = GameObject.FindGameObjectsWithTag("FRIENDMSG");
        foreach(GameObject msg in messages)
        {
            Destroy(msg);
        }
    }


    /// <summary>
    /// 点击添加好友通知的拒绝
    /// </summary>
    /// <param name="param"></param>
    private void ClickNoNewFriend(object param)
    {
        button.clip = common;
        button.Play();
        var noticePlayerAddFriend_ack = new mmopb.noticePlayerAddFriend_ack();
        noticePlayerAddFriend_ack.isAgree = false;
        noticePlayerAddFriend_ack.roleId = roleId;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(noticePlayerAddFriend_ack));

        lobbyView.newFriend.SetActive(false);
        
    }

    /// <summary>
    /// 点击添加好友通知的同意添加
    /// </summary>
    /// <param name="param"></param>
    private void ClickYesNewFriend(object param)
    {
        button.clip = common;
        button.Play();
        var noticePlayerAddFriend_ack = new mmopb.noticePlayerAddFriend_ack();
        noticePlayerAddFriend_ack.isAgree = true;
        noticePlayerAddFriend_ack.roleId = roleId;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(noticePlayerAddFriend_ack));

        lobbyView.newFriend.SetActive(false);
        if (friendView.friends.activeInHierarchy == true)
        {
            var friendReq = new mmopb.friendsList_req();
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(friendReq));
        }
    }

    /// <summary>
    /// 点击邀请通知的同意按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickYesNewInvite(object param)
    {
        button.clip = common;
        button.Play();
        var inviteMsg = new mmopb.noticeInviteFriend_ack();
        inviteMsg.isSuc = true;
        inviteMsg.roleId = roleId_invite;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(inviteMsg));
        lobbyView.newInvite.SetActive(false);
    }

    /// <summary>
    /// 点击邀请通知的拒绝按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickNoNewInvite(object param)
    {
        button.clip = common;
        button.Play();
        var inviteMsg = new mmopb.noticeInviteFriend_ack();
        inviteMsg.isSuc = false;
        inviteMsg.roleId = roleId_invite;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(inviteMsg));
        lobbyView.newInvite.SetActive(false);
    }

    /// <summary>
    /// 点击设置下的音乐开关按钮
    /// </summary>
    /// <param name="param"></param>
    private void ClickMusicButton(object param)
    {
        GameObject musicButton = GameObject.Find("MusicButton");
        Text button_text = musicButton.GetComponentInChildren<Text>();
        if (button_text.text.Equals("打开"))
        {
            LocalUser.Instance.IsMute = false;
            button_text.text = "关闭";
        }
        else
        {
            LocalUser.Instance.IsMute = true;
            button_text.text = "打开";
        }
        
    }

    /// <summary>
    /// 点击设置下的退出账号退出当前账户
    /// </summary>
    /// <param name="param"></param>
    private void ClickLogoutAccount(object param)
    {
        var logoutMsg = new mmopb.exitGame_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(logoutMsg));
    }



    /// <summary>
    /// 加载好友列表
    /// </summary>
    /// <param name="msg"></param>
    private void LoadFriendsHandle(object msg)
    {
        //删除原来有的列表
        GameObject[] oldfriends = GameObject.FindGameObjectsWithTag("FRIEND");
        foreach (GameObject oldfriend in oldfriends)
        {
            Destroy(oldfriend);
        }

        //加载预制体
        GameObject _friend = Resources.Load("Friend") as GameObject;
        var friends = msg as mmopb.friendsList_ack;
        Dictionary<Int32, mmopb.p_FriendsInfo> friendList = friends.friendList;
        foreach (KeyValuePair<Int32, mmopb.p_FriendsInfo> friendInfo in friendList)
        {
            // 对象初始化
            GameObject friend = Instantiate(_friend, friendView.parentTransForm);
            FriendDetail friendDetail = friend.GetComponentInChildren<FriendDetail>();

            friendDetail.friendName.text = friendInfo.Value.nickName;
            Debug.Log(friendInfo.Value.nickName);
            if (friendInfo.Value.isOnline==1)
            {
                friendDetail.avatar.color = new Color32(91, 62, 62, 255);

                friendDetail.status_img_online.SetActive(false);
                friendDetail.status_img_buzy.SetActive(false);
                friendDetail.status_img_offline.SetActive(false);
                friendDetail.status_text.text = "离线";
               
            }
            else if (friendInfo.Value.isOnline == 2)
            {
                friendDetail.avatar.color = new Color32(255, 255, 255, 255);

                friendDetail.status_img_online.SetActive(true);
                friendDetail.status_img_buzy.SetActive(false);
                friendDetail.status_img_offline.SetActive(false);
                friendDetail.status_text.text = "在大厅";
            }
            else if(friendInfo.Value.isOnline == 3)
            {
                friendDetail.avatar.color = new Color32(255, 255, 255, 255);

                friendDetail.status_img_online.SetActive(false);
                friendDetail.status_img_buzy.SetActive(true);
                friendDetail.status_img_offline.SetActive(false);
                friendDetail.status_text.text = "在房间";
            }
            else
            {
                friendDetail.avatar.color = new Color32(255, 255, 255, 255);

                friendDetail.status_img_online.SetActive(false);
                friendDetail.status_img_buzy.SetActive(false);
                friendDetail.status_img_offline.SetActive(false);
                friendDetail.status_text.text = "游戏中";
            }
            friendDetail.messageNotice.SetActive(false);
            friendDetail.friendId = friendInfo.Key;
            
        }

        GameObject[] myFriends = GameObject.FindGameObjectsWithTag("FRIEND");
        foreach (KeyValuePair<int, LinkedList<Message>> message in messages)
        {
            foreach(GameObject myFriend in myFriends)
            {
                FriendDetail myfriendDetail = myFriend.GetComponent<FriendDetail>();
                if (myfriendDetail.friendId == message.Key)
                {
                    myfriendDetail.messageNotice.SetActive(true);
                }
            }
        }
    }

    /// <summary>
    /// 在好友列表点击删除按钮之后处理服务器返回的删除确认消息
    /// </summary>
    /// <param name="msg"></param>
    private void DeleteFriendHandle(object msg)
    {
        var deleteMsg = msg as mmopb.deleteFriend_ack;
        if (deleteMsg.isSuc)
        {
           
            var friendReq = new mmopb.friendsList_req();
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(friendReq));
        }
        else
        {
            TipUtils.Instance.ShowToastUI(deleteMsg.error, root, 1.0f);
        }

    }

    /// <summary>
    /// 接受并处理服务器返回的关于添加好友的ack消息
    /// </summary>
    /// <param name="msg">返回消息</param>
    private void AddFriendHandle(object msg)
    {
        var addFriendAck = msg as mmopb.addFriend_ack;
        if (addFriendAck.isSuc)
        {
            TipUtils.Instance.ShowToastUI("添加成功", root, 1.0f);
            var friendReq = new mmopb.friendsList_req();
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(friendReq));
        }
        else
        {
            TipUtils.Instance.ShowToastUI(addFriendAck.error, root, 1.0f);
        }
    }
    
    /// <summary>
    /// 处理服务器发送的其他玩家请求添加自己的消息
    /// </summary>
    /// <param name="msg"></param>
    private void AddFriendMsgFromServerHandle(object msg)
    {
        var noticeMsg = msg as mmopb.playerAddFriend_ecn;
        lobbyView.newFriend.SetActive(true);
        audio_notice.Play();
        lobbyView.newFriendMsg.text = "玩家：" + noticeMsg.reqPlayerNickName + ",请求添加您为好友，是否同意？";
        roleId = noticeMsg.roleId;
    }

    /// <summary>
    /// 处理服务器返回的别的玩家发给自己的消息
    /// </summary>
    /// <param name="msg"></param>
    private void RevNewMessageHandle(object msg)
    {
        lobbyView.newMessage.SetActive(true);
        audio_message.Play();
        var chatMsg = msg as mmopb.sendMessageToOnlineFriend_ecn;
        //消息内容
        string content = chatMsg.content;
        //发送方昵称
        string playName = chatMsg.nickName;
        //发送方ID
        int chatRoleId = chatMsg.roleId;
        // 秒转化为字符串  
        long seconds = chatMsg.ticks;
        
        
        if (friendView.chatPanle.activeInHierarchy == true)
        {
            //加载预制体
            GameObject messagePrefab = Resources.Load("MessageFriend") as GameObject;
           // 对象初始化
            GameObject friend_message = Instantiate(messagePrefab, friendView.chatParentTransForm);
            MessageDetail messageDetail = friend_message.GetComponent<MessageDetail>();

            DateTime dt = DateTime.Parse("1970-01-01 00:00:00").AddSeconds(seconds);
            dt = dt.AddSeconds(8 * 60 * 60);     // +8h的时差  
            string time = dt.ToString("HH:mm:ss");
            messageDetail.time.text = "【" + time + "】";
            messageDetail.name.text = playName;
            messageDetail.content.text = "：" + content;

            GameObject[] myFriends = GameObject.FindGameObjectsWithTag("FRIEND");
            foreach (KeyValuePair<int, LinkedList<Message>> message in messages)
            {
                foreach (GameObject myFriend in myFriends)
                {
                    FriendDetail myfriendDetail = myFriend.GetComponent<FriendDetail>();
                    if (myfriendDetail.friendId == message.Key)
                    {
                        myfriendDetail.messageNotice.SetActive(true);
                    }
                }
            }
        }
        else
        {
            Message _message = new Message();
            _message.Time = seconds;
            _message.Name = playName;
            _message.Content = content;
            LinkedList<Message> message = new LinkedList<Message>();
            message.AddLast(_message);
            messages.Add(chatRoleId, message);
        }
    }

    /// <summary>
    /// 在大厅时接收并处理服务器发出的别的玩家邀请自己的信息
    /// </summary>
    /// <param name="msg"></param>
    private void RevInviteMsgHandle(object msg)
    {
        lobbyView.newInvite.SetActive(true);
        audio_notice.Play();
        var inviteMsg = msg as mmopb.noticeInviteFriend_ecn;
        lobbyView.newInviteMsg.text = "玩家：" + inviteMsg.nickName + ",邀请你加入游戏";
        roleId_invite = inviteMsg.roleId;
    }


    /// <summary>
    /// 处理点击同意游戏邀请后，服务器返回的消息
    /// </summary>
    /// <param name="msg"></param>
    private void RevInviteResultHandle(object msg)
    {
        var inviteResult = msg as mmopb.inviteResult_ack;
        if (inviteResult.isSuc)
        {
            InviteInfo.Instance.IsAgreeInvite = true;

        }
        else
        {
            TipUtils.Instance.ShowToastUI(inviteResult.error, root, 1.0f);
        }
    }

    /// <summary>
    /// 接收服务器返回的房间玩家信息
    /// </summary>
    /// <param name="msg">服务器返回的房间玩家信息</param>
    private void RoleInfoInRoomHandle(object msg)
    {
        var players = msg as mmopb.roleInfoInRoom_bcst;
        Dictionary<Int32, mmopb.p_roleInfoInRoom> playerList = players.roleInfoInRoomList;
        InviteInfo.Instance.PlayerList = playerList;

        UnityEngine.SceneManagement.SceneManager.LoadScene("Rooms");
    }

    /// <summary>
    /// 处理服务器返回的退出当前账户的返回消息
    /// </summary>
    /// <param name="msg"></param>
    private void LogoutHandle(object msg)
    {
        var logoutMsg = msg as mmopb.exitGame_ack;
        if (logoutMsg.isSuc)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("LogAndReg");
        }
        else
        {
            TipUtils.Instance.ShowToastUI(logoutMsg.error, root, 1.0f);
        }
    }

}
