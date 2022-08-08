using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

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
    cs_sc_test,
    cs_sc_changeItemSlot,
    cs_sc_upgradeItem,
}

public class Packet : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct UserInfo
    {
        public UInt32 networkID;
        public char character;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectServerPacket
    {
        public UInt16 size;
        public char type;
        public Int32 networkID;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct sc_ConnectRoomPacket
    {
        public UInt16 size;
        public char type;
        public Int32 roomNumber;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UserInfo[] users;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_StartMatchingPacket
    {
        public UInt16 size;
        public char type;
        public Int32 networkID;
        public char character;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_AddNewItemPacket
    {
        public readonly UInt16 size;
        public readonly char type;
        public readonly Int32 networkID;
        public readonly Int32 itemCode;

        public cs_sc_AddNewItemPacket(Int32 networkID, Int32 itemCode)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = (char) PacketType.cs_sc_addNewItem;
            this.networkID = networkID;
            this.itemCode = itemCode;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_testPacket
    {
        public readonly UInt16 size;
        public readonly char type;
        public readonly Int32 networkID;
        public readonly Int32 number;

        public cs_sc_testPacket(Int32 networkID, Int32 num)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = (char) PacketType.cs_sc_test;
            this.networkID = networkID;
            number = num;
        }
    }
    
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    public struct cs_sc_changeItemSlotPacket
    {
        public readonly UInt16 size;
        public readonly char type;
        public readonly Int32 networkID;
        public readonly Int16 slot1;
        public readonly Int16 slot2;

        public cs_sc_changeItemSlotPacket(Int32 networkID, Int16 slot1, Int16 slot2)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = (char) PacketType.cs_sc_changeItemSlot;
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
        public readonly char type;
        public readonly Int32 networkID;
        public readonly Int16 slot;

        public cs_sc_upgradeItemPacket(Int32 networkID, Int16 slot)
        {
            size = (UInt16) Marshal.SizeOf<cs_sc_AddNewItemPacket>();
            type = (char) PacketType.cs_sc_upgradeItem;
            this.networkID = networkID;
            this.slot = slot;
        }
    }
}