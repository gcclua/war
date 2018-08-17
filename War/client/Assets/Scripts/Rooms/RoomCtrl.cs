using Assets.Scripts.Soldier;
using ProtoBuf;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 房间系统控制类
/// </summary>
public class RoomCtrl : MonoBehaviour
{

    //房间列表视图
    public RoomsView roomsView;
    //创建的房间视图
    public RoomView roomView;
    public Transform root;

    //邀请自己的玩家
    private int roleId_invite;

    //按钮声音特效
    public AudioSource button;
    //一般按钮声音
    public AudioClip common;
    //发送消息按钮声音
    public AudioClip sendMsg;
    //开始游戏
    public GameObject startGame;


    // Use this for initialization
    public void Start()
    {
        //邀请显示
        if (InviteInfo.Instance.IsAgreeInvite)
        {
            roomsView.rooms.SetActive(false);
            roomView.room_boot.SetActive(true);
            //清空信息
            PlayersInfo playersInfo = roomView.room.GetComponent<PlayersInfo>();
            playersInfo.name_Yang.text = "";
            playersInfo.camp_Yang.text = "";
            playersInfo.name_Yin.text = "";
            playersInfo.camp_Yin.text = "";
            Dictionary<Int32, mmopb.p_roleInfoInRoom> playerList = InviteInfo.Instance.PlayerList;
            foreach (KeyValuePair<Int32, mmopb.p_roleInfoInRoom> player in playerList)
            {

                if (player.Value.camp == 1)
                {
                    playersInfo.name_Yang.text = player.Value.nickName;
                    playersInfo.camp_Yang.text = "阳";
                }
                else
                {
                    playersInfo.name_Yin.text = player.Value.nickName;
                    playersInfo.camp_Yin.text = "阴";
                }
            }
            LocalUser.Instance.Camp = playerList[LocalUser.Instance.PlayerId].camp;
            InviteInfo.Instance.IsAgreeInvite = false;
        }
        
        //进入时请求房间列表数据
        var rooms_req = new mmopb.roomList_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(rooms_req));

        //音量设置
        GameObject canvas = GameObject.Find("Canvas");
        AudioSource audioSource = canvas.GetComponent<AudioSource>();
        audioSource.volume = LocalUser.Instance.Voice;
        audioSource.mute = LocalUser.Instance.IsMute;

        //事件
        UIDispacher.Instance.AddEventListener("Return", OnClickBtnReturn);
        UIDispacher.Instance.AddEventListener("Refresh", OnClickBtnRefresh);
        UIDispacher.Instance.AddEventListener("CreateRoom", OnClickBtnCreateRoom);
        UIDispacher.Instance.AddEventListener("Join", OnClickBtnJoin);
        UIDispacher.Instance.AddEventListener("StartGame", OnClickBtnStartGame);
        UIDispacher.Instance.AddEventListener("QuitRoom", OnClickBtnQuitRoom);
        UIDispacher.Instance.AddEventListener("InviteFriend", OnClickBtnInviteFriend);
        UIDispacher.Instance.AddEventListener("SendMessage", OnClickBtnSendMessage); 
        UIDispacher.Instance.AddEventListener("CloseInviteFriends", OnClickBtnCloseInviteFriends); 
        UIDispacher.Instance.AddEventListener("ClickInviteTargetFriend", ClickInviteTargetFriend);
        UIDispacher.Instance.AddEventListener("ClickYesNewInvite", ClickYesNewInvite);
        UIDispacher.Instance.AddEventListener("ClickNoNewInvite", ClickNoNewInvite);

        NetDispacher.Instance.AddEventListener("roomList_ack", LoadRoomsHandle);
        NetDispacher.Instance.AddEventListener("createRoom_ack", CreateRoomHandle);
        NetDispacher.Instance.AddEventListener("enterRoom_ack", EnterRoomHandle);
        NetDispacher.Instance.AddEventListener("roleInfoInRoom_bcst", RoleInfoInRoomHandle);
        NetDispacher.Instance.AddEventListener("startBattle_bcst", StartBattle_bcstHandle);
        NetDispacher.Instance.AddEventListener("leaveRoom_ack", LeaveRoomHandle);
        NetDispacher.Instance.AddEventListener("sendMessage_bcst", RevChatMsgHandle);
        NetDispacher.Instance.AddEventListener("friendsList_ack", FriendsListHandle);
        NetDispacher.Instance.AddEventListener("noticeInviteFriend_ecn", RevInviteMsgHandle);
        NetDispacher.Instance.AddEventListener("inviteResult_ack", RevInviteResultHandle);
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
        UIDispacher.Instance.RemoveEventListener("Return", OnClickBtnReturn);
        UIDispacher.Instance.RemoveEventListener("Refresh", OnClickBtnRefresh);
        UIDispacher.Instance.RemoveEventListener("CreateRoom", OnClickBtnCreateRoom);
        UIDispacher.Instance.RemoveEventListener("Join", OnClickBtnJoin);
        UIDispacher.Instance.RemoveEventListener("QuitRoom", OnClickBtnQuitRoom);
        UIDispacher.Instance.RemoveEventListener("StartGame", OnClickBtnStartGame);
        UIDispacher.Instance.RemoveEventListener("InviteFriend", OnClickBtnInviteFriend);
        UIDispacher.Instance.RemoveEventListener("SendMessage", OnClickBtnSendMessage);
        UIDispacher.Instance.RemoveEventListener("CloseInviteFriends", OnClickBtnCloseInviteFriends);
        UIDispacher.Instance.RemoveEventListener("ClickInviteTargetFriend", ClickInviteTargetFriend);
        UIDispacher.Instance.RemoveEventListener("ClickYesNewInvite", ClickYesNewInvite);
        UIDispacher.Instance.RemoveEventListener("ClickNoNewInvite", ClickNoNewInvite);

        NetDispacher.Instance.RemoveEventListener("roomList_ack", LoadRoomsHandle);
        NetDispacher.Instance.RemoveEventListener("createRoom_ack", CreateRoomHandle);
        NetDispacher.Instance.RemoveEventListener("enterRoom_ack", EnterRoomHandle);
        NetDispacher.Instance.RemoveEventListener("roleInfoInRoom_bcst", RoleInfoInRoomHandle);
        NetDispacher.Instance.RemoveEventListener("startBattle_bcst", StartBattle_bcstHandle);
        NetDispacher.Instance.RemoveEventListener("leaveRoom_ack", LeaveRoomHandle);
        NetDispacher.Instance.RemoveEventListener("sendMessage_bcst", RevChatMsgHandle);
        NetDispacher.Instance.RemoveEventListener("friendsList_ack", FriendsListHandle);
        NetDispacher.Instance.RemoveEventListener("noticeInviteFriend_ecn", RevInviteMsgHandle);
        NetDispacher.Instance.RemoveEventListener("inviteResult_ack", RevInviteResultHandle);
    }

    /// <summary>
    /// 在房间列表界面点击返回按钮回到游戏的主界面
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnReturn(object param)
    {
        button.clip = common;
        button.Play();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameLobby");
    }

    /// <summary>
    /// 在房间列表界面点击刷新列表按钮刷新房间列表数据
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnRefresh(object param)
    {
        button.clip = common;
        button.Play();
        //删除原来有的列表
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("ROOM");
        foreach (GameObject room in rooms)
        {
            Destroy(room);
        }
        var rooms_req = new mmopb.roomList_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(rooms_req));
    }

    /// <summary>
    /// 在房间列表界面点击创建房间按钮进入创建房间界面
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnCreateRoom(object param)
    {
        button.clip = common;
        button.Play();
        var createRoomMsg = new mmopb.creteRoom_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(createRoomMsg));
    }

    /// <summary>
    /// 在房间列表界面点击房间的加入按钮加入到一个房间
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnJoin(object param)
    {
        button.clip = common;
        button.Play();
        GameObject room = (GameObject)param;
        RoomDetails roomDetails = room.GetComponent<RoomDetails>();
        if (roomDetails.status.text.Equals("战斗中"))
        {
            return;
        }
        else
        {
            var joinRoomMsg = new mmopb.enterRoom_req();
            joinRoomMsg.roomId = roomDetails._roomId;
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(joinRoomMsg));
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
        roomsView.newInvite.SetActive(false);
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
        roomsView.newInvite.SetActive(false);
    }

    /// <summary>
    /// 在房间里面点击开始游戏，向服务器发出开始游戏请求
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnStartGame(object param)
    {
        button.clip = common;
        button.Play();
        var startGameMsg = new mmopb.startBattle_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(startGameMsg));
    }

    /// <summary>
    /// 在房间里面点击退出房间退出房间
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnQuitRoom(object param)
    {
        button.clip = common;
        button.Play();
        var leaveRoomMsg = new mmopb.leaveRoom_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(leaveRoomMsg));
    }

    /// <summary>
    /// 在房间里面点击邀请按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnInviteFriend(object param)
    {
        button.clip = common;
        button.Play();
        roomView.friendList.SetActive(true);
        var friendsReq = new mmopb.friendsList_req();
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(friendsReq));
    }


    /// <summary>
    /// 在房间里面点击发送按钮发送消息
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnSendMessage(object param)
    {
        button.clip = sendMsg;
        button.Play();
        var sendMessage_req = new mmopb.sendMessage_req();
        if (roomView.message.text != "")
        {
            sendMessage_req.content = roomView.message.text;
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(sendMessage_req));
            roomView.message.text = "";
        }
    }

    /// <summary>
    /// 点击邀请好友列表界面的关闭按钮
    /// </summary>
    /// <param name="param"></param>
    private void OnClickBtnCloseInviteFriends(object param)
    {
        button.clip = common;
        button.Play();
        //清除数据
        GameObject[] friends = GameObject.FindGameObjectsWithTag("FRIENDINVITE");
        foreach(GameObject friend in friends)
        {
            Destroy(friend);
        }
        roomView.friendList.SetActive(false);
    }

    /// <summary>
    /// 点击邀请好友列表界面的邀请按钮（对勾）
    /// </summary>
    /// <param name="param"></param>
    private void ClickInviteTargetFriend(object param)
    {
        button.clip = sendMsg;
        button.Play();
        var inviteFriendMsg = new mmopb.inviteFriend_req();
        GameObject friend = (GameObject)param;
        FriendInviteDetail friendInviteDetail = friend.GetComponent<FriendInviteDetail>();
        inviteFriendMsg.roleId = friendInviteDetail.friendId;
        ClientNet.Instance.Send(ProtoHelper.EncodeWithName(inviteFriendMsg));
    }
    
    /// <summary>
    /// 接收服务器的房间请求返回消息，并更新房间列表数据
    /// </summary>
    /// <param name="msg">服务器返回的房间列表信息</param>
    private void LoadRoomsHandle(object msg)
    {
        //加载预制体
        GameObject _room = Resources.Load("Room") as GameObject;
        var rooms = msg as mmopb.roomList_ack;
        Dictionary<Int32, mmopb.p_roomInfo> roomList = rooms.roomList;
        foreach (KeyValuePair<Int32, mmopb.p_roomInfo> roomInfo in roomList)
        {
            // 对象初始化
            GameObject room = Instantiate(_room, roomsView.parentTransForm);
            RoomDetails roomDetails = room.GetComponent<RoomDetails>();
            roomDetails.roomID.text = roomInfo.Value.roomId.ToString();
            roomDetails.roomOwner.text = roomInfo.Value.nickName;
            roomDetails._roomId = roomInfo.Value.roomId;
            roomDetails.perpleNumber.text = roomInfo.Value.roleNum.ToString();
            if (roomInfo.Value.isFight)
            {
                roomDetails.status.text = "战斗中";
            }
            else
            {
                roomDetails.status.text = "等待中";
            }
        }
    }

    /// <summary>
    /// 接收服务器的创建房间请求返回消息，并更新创建的房间里面的数据
    /// </summary>
    /// <param name="msg">服务器返回的创建的房间消息</param>
    private void CreateRoomHandle(object msg)
    {
        //关闭房间列表
        roomsView.rooms.SetActive(false);
        //显示创建的房间
        roomView.room_boot.SetActive(true);
        //清空信息
        PlayersInfo playersInfo = roomView.room.GetComponent<PlayersInfo>();
        playersInfo.name_Yang.text = "";
        playersInfo.camp_Yang.text = "";
        playersInfo.name_Yin.text = "";
        playersInfo.camp_Yin.text = "";
        var room = msg as mmopb.createRoom_ack;
        PlayersInfo players = roomView.room.GetComponent<PlayersInfo>();
        players.name_Yang.text = room.nickName;
        if (room.camp == 1)
        {
            players.camp_Yang.text = "阳";
        }
    }

    /// <summary>
    /// 接收服务器返回的进入房间的确认消息
    /// </summary>
    /// <param name="msg">服务器返回的房间确认消息</param>
    private void EnterRoomHandle(object msg)
    {
        var enterRoomMsg = msg as mmopb.enterRoom_ack;
        if (enterRoomMsg.isSuc)
        {
            //关闭房间列表
            roomsView.rooms.SetActive(false);
            //显示创建的房间
            roomView.room_boot.SetActive(true);
            
        }
        else
        {
            TipUtils.Instance.ShowToastUI("房间不存在", root, 1f);
        }
    }

    /// <summary>
    /// 进入房间时接收服务器返回的房间玩家信息
    /// </summary>
    /// <param name="msg">服务器返回的房间玩家信息</param>
    private void RoleInfoInRoomHandle(object msg)
    {
        //清空信息
        PlayersInfo playersInfo = roomView.room.GetComponent<PlayersInfo>();
        playersInfo.name_Yang.text = "";
        playersInfo.camp_Yang.text = "";
        playersInfo.name_Yin.text = "";
        playersInfo.camp_Yin.text = "";
        var players = msg as mmopb.roleInfoInRoom_bcst;
        Dictionary<Int32, mmopb.p_roleInfoInRoom> playerList = players.roleInfoInRoomList;
        foreach (KeyValuePair<Int32, mmopb.p_roleInfoInRoom> player in playerList)
        {

            if (player.Value.camp == 1)
            {
                playersInfo.name_Yang.text = player.Value.nickName;
                playersInfo.camp_Yang.text = "阳";
            }
            else
            {
                playersInfo.name_Yin.text = player.Value.nickName;
                playersInfo.camp_Yin.text = "阴";
            }

            if (player.Value.isOwner && player.Value.nickName == LocalUser.Instance.User_nickname) 
            {
                
                startGame.SetActive(true);
            }
            else if (!player.Value.isOwner && player.Value.nickName == LocalUser.Instance.User_nickname)
            {
                startGame.SetActive(false);
            }
        }
        LocalUser.Instance.Camp = playerList[LocalUser.Instance.PlayerId].camp;
    }

    /// <summary>
    /// 点击开始游戏后处理服务器返回的消息
    /// </summary>
    /// <param name="msg">服务器返回信息</param>
    private void StartBattle_bcstHandle(object msg)
    {
        //初始化数据
        DataMgr.Instance.Reset();

        var handle = msg as mmopb.startBattle_bcst;
        LocalUser.Instance.MyHp = handle.startHp;
        LocalUser.Instance.EnemyHp = handle.startHp;
        LocalUser.Instance.Energy = handle.startEnergy;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }


    /// <summary>
    /// 点击离开房间后处理服务器返回的消息
    /// </summary>
    /// <param name="msg">服务器返回的离开房间消息</param>
    private void LeaveRoomHandle(object msg)
    {
        var leaveRoomMsg = msg as mmopb.leaveRoom_ack;
        if (leaveRoomMsg.isSuc)
        {
            //打开房间列表
            roomsView.rooms.SetActive(true);
            //关闭进入的房间
            roomView.room_boot.SetActive(false);

            //删除原来有的列表
            GameObject[] rooms = GameObject.FindGameObjectsWithTag("ROOM");
            foreach (GameObject room in rooms)
            {
                Destroy(room);
            }
            //删除聊天消息
            GameObject[] messages = GameObject.FindGameObjectsWithTag("ROOMMSG");
            foreach (GameObject message in messages)
            {
                Destroy(message);
            }
            //请求房间列表数据
            var rooms_req = new mmopb.roomList_req();
            ClientNet.Instance.Send(ProtoHelper.EncodeWithName(rooms_req));
        }
        else
        {
            Debug.Log(leaveRoomMsg.error);
        }
    }

    /// <summary>
    /// 接收服务器返回的聊天信息
    /// </summary>
    /// <param name="msg">聊天消息</param>
    private void RevChatMsgHandle(object msg)
    {
        //加载预制体
        GameObject _msg = Resources.Load("Messages") as GameObject;
        var chatMsg = msg as mmopb.sendMessage_bcst;
        
        // 对象初始化
        GameObject chat = Instantiate(_msg, roomView.chatTransForm);
        MessageInfo messageInfo = chat.GetComponent<MessageInfo>();
        // 秒转化为字符串  
        long seconds = chatMsg.ticks;
        DateTime dt = DateTime.Parse("1970-01-01 00:00:00").AddSeconds(seconds);
        dt = dt.AddSeconds(8 * 60 * 60);     // +8h的时差  
        string time = dt.ToString("HH:mm:ss");
        messageInfo.time.text = "【" + time + "】";
        messageInfo.name.text = chatMsg.nickName;
        messageInfo.Msg.text = "：" + chatMsg.content;
    }

    /// <summary>
    /// 处理服务器返回的好友列表的消息
    /// </summary>
    /// <param name="msg"></param>
    private void FriendsListHandle(object msg)
    {
        //清除原来的数据
        GameObject[] oldFriends = GameObject.FindGameObjectsWithTag("FRIENDINVITE");
        foreach(GameObject oldFriend in oldFriends)
        {
            Destroy(oldFriend);
        }
        //加载预制体
        GameObject _friend = Resources.Load("FriendInvite") as GameObject;
        var friends = msg as mmopb.friendsList_ack;
        Dictionary<Int32, mmopb.p_FriendsInfo> friendList = friends.friendList;
        foreach (KeyValuePair<Int32, mmopb.p_FriendsInfo> friendInfo in friendList)
        {
            if (friendInfo.Value.isOnline == 2)
            {
                // 对象初始化
                GameObject friend = Instantiate(_friend, roomView.friendsTransForm);
                FriendInviteDetail friendInviteDetail = roomView.GetComponentInChildren<FriendInviteDetail>();
                friendInviteDetail.friendName.text = friendInfo.Value.nickName;
                friendInviteDetail.friendId = friendInfo.Key;
            }
        }
    }

    /// <summary>
    /// 在房间列表时接收并处理服务器发出的别的玩家邀请自己的信息
    /// </summary>
    /// <param name="msg"></param>
    private void RevInviteMsgHandle(object msg)
    {
        var inviteMsg = msg as mmopb.noticeInviteFriend_ecn;
        roomsView.newInvite.SetActive(true);
        roomsView.newInviteMsg.text = "玩家：" + inviteMsg.nickName + ",邀请你加入游戏";
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
            //关闭房间列表
            roomsView.rooms.SetActive(false);
            //显示创建的房间
            roomView.room_boot.SetActive(true);
        }
        else
        {
            TipUtils.Instance.ShowToastUI(inviteResult.error, root, 1.0f);
        }
    }
}