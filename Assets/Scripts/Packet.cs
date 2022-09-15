using System;
using System.Runtime.InteropServices;

/// <summary>
/// 패킷 타입<br/>
/// sc : Server to Client<br/>
/// cs : Client to Server
/// </summary>
public enum EPacketType : byte
{
    sc_connectServer,           // 서버가 클라이언트에게 서버 접속했음을 알리는 패킷 타입
    sc_connectRoom,             // 서버가 클라이언트에게 Room 생성과 접속했음을 알리는 패킷 타입
    sc_disconnect,              // 서버가 클라이언트에게 접속이 해제되었다고 알리는 패킷 타입
    cs_startMatching,           // 클라이언트가 서버에게 매칭을 시작했음을 알리는 패킷 타입
    cs_sc_addNewItem,           // 어느쪽으로든 아이템을 추가했음을 알리는 패킷 타입 
    cs_sc_changeItemSlot,       // 어느쪽으로든 아이템을 다른 슬롯으로 이동했음을 알리는 패킷 타입
    cs_sc_upgradeItem,          // 어느쪽으로든 아이템을 업그레이드 했음을 알리는 패킷 타입
    cs_sc_changeCharacter,      // 어느쪽으로든 선택 캐릭터가 교체되었음을 알리는 패킷 타입
    sc_battleInfo,              // 서버가 클라이언트에게 전투 정보를 알리는 패킷 타입
    cs_battleReady,             // 클라이언트가 서버에게 전투 준비를 알리는 패킷 타입
}

namespace Packet
{
    /// <summary> 서버가 클라이언트에게 서버 접속했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectServerPacket
    {
        public UInt16 size;
        public EPacketType type;
        public Int32 networkID;
    }

    /// <summary> 유저의 정보 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct UserInfo
    {
        public UInt32 networkID;
        
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxUserNameSizeByByte)]
        public Byte[] name;
    }
    
    /// <summary> 서버가 클라이언트에게 Room 생성과 접속했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectRoomPacket
    {
        public UInt16 size;
        public EPacketType type;

        /// <summary>
        /// Room에 속한 유저들 정보<br/>
        /// - 유저의 네트워크 아이디와 닉네임이 들어있다
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public UserInfo[] users;
    }

    /// <summary> 클라이언트가 서버에게 매칭을 시작했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_StartMatchingPacket
    {
        public UInt16 size;
        public EPacketType type;
        public Int32 networkID;

        /// <summary>
        /// 유저의 이름을 서버에 알려주는 용도로 사용<br/>
        /// 글자수가 10자를 넘기면 안 된다
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxUserNameSizeByByte)]
        public Byte[] name;
    }

    /// <summary> 어느쪽으로든 아이템을 추가했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_AddNewItemPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        public readonly Int32 networkID;
        /// <summary> 추가되는 아이템 코드 </summary>
        public readonly Byte itemCode;

        public cs_sc_AddNewItemPacket(Int32 networkID, Byte itemCode)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = EPacketType.cs_sc_addNewItem;
            this.networkID = networkID;
            this.itemCode = itemCode;
        }
    }

    /// <summary> 어느쪽으로든 아이템을 다른 슬롯으로 이동했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_ChangeItemSlotPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        public readonly Int32 networkID;
        /// <summary> 아이템이 원래 있던 슬롯 번호 </summary>
        public readonly Byte slot1;
        /// <summary> 아이템이 이동되는 슬롯 번호 </summary>
        public readonly Byte slot2;

        public cs_sc_ChangeItemSlotPacket(Int32 networkID, Byte slot1, Byte slot2)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_ChangeItemSlotPacket>();
            type = EPacketType.cs_sc_changeItemSlot;
            this.networkID = networkID;
            this.slot1 = slot1;
            this.slot2 = slot2;
        }
    }

    /// <summary> 어느쪽으로든 아이템을 업그레이드 했음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_UpgradeItemPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        public readonly Int32 networkID;
        /// <summary> 업그레이드 할 아이템이 있는 슬롯 번호 </summary>
        public readonly Byte slot;

        public cs_sc_UpgradeItemPacket(Int32 networkID, Byte slot)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_UpgradeItemPacket>();
            type = EPacketType.cs_sc_upgradeItem;
            this.networkID = networkID;
            this.slot = slot;
        }
    }

    /// <summary> 어느쪽으로든 선택 캐릭터가 교체되었음을 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_ChangeCharacterPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        public readonly Int32 networkID;
        /// <summary> 변경할 캐릭터 타입 </summary>
        public readonly ECharacterType characterType;

        public cs_sc_ChangeCharacterPacket(Int32 networkID, ECharacterType characterType)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_ChangeCharacterPacket>();
            type = EPacketType.cs_sc_changeCharacter;
            this.networkID = networkID;
            this.characterType = characterType;
        }
    }

    /// <summary> 클라이언트가 서버에게 전투 준비를 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_BattleReadyPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        public readonly Int32 networkID;
        /// <summary>
        /// 유저의 선공 스텟을 보내준다<br/>
        /// 이 선공 스텟으로 전투에서 누가 선공할지를 결정한다
        /// </summary>
        public readonly Int16 firstAttack;

        public cs_BattleReadyPacket(Int32 networkID, Int16 firstAttack)
        {
            size = (UInt16) Marshal.SizeOf<cs_BattleReadyPacket>();
            type = EPacketType.cs_battleReady;
            this.networkID = networkID;
            this.firstAttack = firstAttack;
        }
    }

    /// <summary> 아이템 발동 순서가 담긴 정보 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct ItemQueueInfo
    {
        /// <summary> 아이템 발동 순서의 유저 네트워크 아이디 </summary>
        public readonly Int32 networkID;

        /// <summary>
        /// UsingInventory 아이템 슬롯의 발동 순서와 발동 여부가 반복횟수 만큼 들어있다.<br/>
        /// 발동 여부는 서버에 저장되어 있는 아이템 발동 확률에 의해 결정되어서 클라이언트로 전달된다.<br/>
        /// 발동 슬롯 번호에 255 값이 들어있으면 더 이상 발동될 수 있는 아이템이 없는 것이다.<br/>
        /// 발동 슬롯 번호에 254 값이 들어있으면 해당 슬롯이 Lock 된것이다.<br/>
        /// 발동 여부의 값이 1인 경우 발동되는 아이템이고 0인 경우 발동되지 않는다.<br/>
        /// 
        /// 형태) [발동 슬롯 번호, 발동 여부, 발동 슬롯 번호, 발동 여부, 발동 슬롯 번호, 발동 여부, ... ]<br/>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.ItemQueueLength)]
        public readonly Byte[] itemQueue;
    }

    /// <summary> 서버가 클라이언트에게 전투 정보를 알리는 패킷 </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_BattleInfoPacket
    {
        public readonly UInt16 size;
        public readonly EPacketType type;
        
        /// <summary>
        /// 전투 순서가 담겨져 있는 배열<br/>
        /// 배열 요소들은 유저의 네트워크 아이디로 이루어져 있다.<br/>
        /// 값이 Int32 최대값이 들어올 경우 뒷 요소들은 플레이어가 더이상 없는 경우이다.<br/>
        /// 
        /// 형태) [전투A유저1, 전투A유저2, 전투B유저1, 전투B유저2, 전투C유저1, 전투C유저2, 전투D유저1, 전투D유저2]<br/>
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public readonly Int32[] battleOpponents;

        
        /// <summary> 모든 Room 유저의 아이템 발동 순서가 담겨져 있는 배열 </summary>
        // 전투는 해당 배열만 활용해서 진행해야 한다. 절대 수정하면 안 된다.
        // 만약 배열을 수정하면 다른 클라이언트랑 결과가 달라지게 된다
        // 자세한 내용은 구조체 설명을 참고
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public readonly ItemQueueInfo[] itemQueueInfos;
    }
}
