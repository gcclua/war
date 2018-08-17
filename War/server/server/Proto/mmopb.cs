// This file was generated by a tool; you should avoid making direct changes.
// Consider using 'partial classes' to extend these types
// Input: mmopb.proto

#pragma warning disable CS1591, CS0612, CS3021, IDE1006
namespace mmopb
{

    [global::ProtoBuf.ProtoContract()]
    public partial class login_req
    {
        [global::ProtoBuf.ProtoMember(1)]
        public string account { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2)]
        public string password { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class login_ack
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool succ { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string error { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class register_req
    {
        [global::ProtoBuf.ProtoMember(1)]
        public string account { get; set; } = "";

        [global::ProtoBuf.ProtoMember(2)]
        public string password { get; set; } = "";

        [global::ProtoBuf.ProtoMember(3)]
        public string nickname { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class register_ack
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool succ { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string error { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class p_roomInfo
    {
        [global::ProtoBuf.ProtoMember(1)]
        public int roleNum { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public bool isFight { get; set; }

        [global::ProtoBuf.ProtoMember(3)]
        public int roomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class roomList_req
    {
    }

    [global::ProtoBuf.ProtoContract()]
    public partial class roomList_ack
    {
        [global::ProtoBuf.ProtoMember(1, TypeName = "mmopb.p_roomInfo")]
        [global::ProtoBuf.ProtoMap]
        public global::System.Collections.Generic.Dictionary<int, p_roomInfo> roomList { get;  set; } = new global::System.Collections.Generic.Dictionary<int, p_roomInfo>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class p_roleInfoInRoom
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool isOwner { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string nickName { get; set; } = "";

        [global::ProtoBuf.ProtoMember(3)]
        public int camp { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class creteRoom_req
    {
    }

    [global::ProtoBuf.ProtoContract()]
    public partial class createRoom_ack
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool isOwner { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string nickName { get; set; } = "";

        [global::ProtoBuf.ProtoMember(3)]
        public int camp { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class enterRoom_req
    {
        [global::ProtoBuf.ProtoMember(1)]
        public int roomId { get; set; }

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class enterRoom_ack
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool isSuc { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string error { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class leaveRoom_req
    {
    }

    [global::ProtoBuf.ProtoContract()]
    public partial class leaveRoom_ack
    {
        [global::ProtoBuf.ProtoMember(1)]
        public bool isSuc { get; set; }

        [global::ProtoBuf.ProtoMember(2)]
        public string error { get; set; } = "";

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class roleInfoInRoom_bcst
    {
        [global::ProtoBuf.ProtoMember(1, TypeName = "mmopb.p_roleInfoInRoom")]
        [global::ProtoBuf.ProtoMap]
        public global::System.Collections.Generic.Dictionary<int, p_roleInfoInRoom> roleInfoInRoomList { get;  set; } = new global::System.Collections.Generic.Dictionary<int, p_roleInfoInRoom>();

    }

    [global::ProtoBuf.ProtoContract()]
    public partial class startBattle_req
    {
    }

    [global::ProtoBuf.ProtoContract()]
    public partial class startBattle_bcst
    {
        [global::ProtoBuf.ProtoMember(1)]
        public long ticks { get; set; }

    }

}

public class ILRuntime_mmopb
{
    static ILRuntime_mmopb()
    {

        //Initlize();

    }
    public static void Initlize()
    {

        ProtoBuf.PType.RegisterType("mmopb.login_req", typeof(mmopb.login_req));
        ProtoBuf.PType.RegisterType("mmopb.login_ack", typeof(mmopb.login_ack));
        ProtoBuf.PType.RegisterType("mmopb.register_req", typeof(mmopb.register_req));
        ProtoBuf.PType.RegisterType("mmopb.register_ack", typeof(mmopb.register_ack));
        ProtoBuf.PType.RegisterType("mmopb.p_roomInfo", typeof(mmopb.p_roomInfo));
        ProtoBuf.PType.RegisterType("mmopb.roomList_req", typeof(mmopb.roomList_req));
        ProtoBuf.PType.RegisterType("mmopb.roomList_ack", typeof(mmopb.roomList_ack));
        ProtoBuf.PType.RegisterType("mmopb.p_roleInfoInRoom", typeof(mmopb.p_roleInfoInRoom));
        ProtoBuf.PType.RegisterType("mmopb.creteRoom_req", typeof(mmopb.creteRoom_req));
        ProtoBuf.PType.RegisterType("mmopb.createRoom_ack", typeof(mmopb.createRoom_ack));
        ProtoBuf.PType.RegisterType("mmopb.enterRoom_req", typeof(mmopb.enterRoom_req));
        ProtoBuf.PType.RegisterType("mmopb.enterRoom_ack", typeof(mmopb.enterRoom_ack));
        ProtoBuf.PType.RegisterType("mmopb.leaveRoom_req", typeof(mmopb.leaveRoom_req));
        ProtoBuf.PType.RegisterType("mmopb.leaveRoom_ack", typeof(mmopb.leaveRoom_ack));
        ProtoBuf.PType.RegisterType("mmopb.roleInfoInRoom_bcst", typeof(mmopb.roleInfoInRoom_bcst));
        ProtoBuf.PType.RegisterType("mmopb.startBattle_req", typeof(mmopb.startBattle_req));
        ProtoBuf.PType.RegisterType("mmopb.startBattle_bcst", typeof(mmopb.startBattle_bcst));

    }
}

#pragma warning restore CS1591, CS0612, CS3021, IDE1006
