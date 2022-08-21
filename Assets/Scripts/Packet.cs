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
    sc_battleItemQueue,
    sc_battleOpponentQueue,
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
        public readonly Int32 itemCode;

        public cs_sc_AddNewItemPacket(Int32 networkID, Int32 itemCode)
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
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
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
            size = (UInt16) Marshal.SizeOf<sc_battleItemQueuePacket>();
            type = (Byte) PacketType.cs_sc_changeCharacter;
            this.networkID = networkID;
            this.characterType = characterType;
        }
    }

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

    public struct ItemQueueInfo
    {
        public Int32 networkID;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.ItemQueueLength)]
        public Byte[] itemQueue;
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_battleItemQueuePacket
    {
        public readonly UInt16 size;
        public readonly Byte type;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Global.MaxRoomPlayer)]
        public ItemQueueInfo[] itemQueueInfos;
    }
}
