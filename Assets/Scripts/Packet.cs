using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// 패킷 타입<br/>
/// sc : Server to Client<br/>
/// cs : Client to Server
/// </summary>
enum PacketType
{
    sc_connectServer,
    sc_connectRoom,
    sc_disconnect,
    cs_startMatching,
    cs_sc_addNewItem,
    cs_sc_changeItemSlot,
    cs_sc_upgradeItem,
    cs_sc_changeCharacter,
    sc_battleInfo,
    cs_battleReady,
}

namespace Packet
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct UserInfo
    {
        public UInt32 networkID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public Byte[] name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectServerPacket
    {
        public UInt16 size;
        public Byte type;
        public Int32 networkID;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectRoomPacket
    {
        public UInt16 size;
        public Byte type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UserInfo[] users;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_StartMatchingPacket
    {
        public UInt16 size;
        public Byte type;
        public Int32 networkID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 22)]
        public Byte[] name;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_AddNewItemPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        public readonly Int32 networkID;
        public readonly Byte itemCode;

        public cs_sc_AddNewItemPacket(Int32 networkID, Byte itemCode)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = (Byte) PacketType.cs_sc_addNewItem;
            this.networkID = networkID;
            this.itemCode = itemCode;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_changeItemSlotPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        public readonly Int32 networkID;
        public readonly Byte slot1;
        public readonly Byte slot2;

        public cs_sc_changeItemSlotPacket(Int32 networkID, Byte slot1, Byte slot2)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_changeItemSlotPacket>();
            type = (Byte) PacketType.cs_sc_changeItemSlot;
            this.networkID = networkID;
            this.slot1 = slot1;
            this.slot2 = slot2;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_upgradeItemPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        public readonly Int32 networkID;
        public readonly Byte slot;

        public cs_sc_upgradeItemPacket(Int32 networkID, Byte slot)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_upgradeItemPacket>();
            type = (Byte) PacketType.cs_sc_upgradeItem;
            this.networkID = networkID;
            this.slot = slot;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_changeCharacterPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        public readonly Int32 networkID;
        public readonly char characterType;

        public cs_sc_changeCharacterPacket(Int32 networkID, char characterType)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_changeCharacterPacket>();
            type = (Byte) PacketType.cs_sc_changeCharacter;
            this.networkID = networkID;
            this.characterType = characterType;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_battleReadyPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        public readonly Int32 networkID;

        public cs_battleReadyPacket(Int32 networkID)
        {
            size = (UInt16) Marshal.SizeOf<cs_battleReadyPacket>();
            type = (Byte) PacketType.cs_battleReady;
            this.networkID = networkID;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct ItemQueueInfo
    {
        public Int32 networkID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.ItemQueueLength)]
        public Byte[] itemQueue;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_battleInfoPacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        // todo : 패킷 구조체랑 패킷 타입 이름 변경
        // 플레이어가 없는경우 int 최대값이 들어옴
        // int 최대값이 들어오면 반복문 멈추게 설정
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public Int32[] battleOpponents;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public ItemQueueInfo[] itemQueueInfos;
    }
}
