using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Packet;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : Singleton<NetworkManager>
{
    private Socket _socket;
    private IPAddress _serverIp;
    private const string ServerDomain = "zecocostudio.com";
    private const int ServerPort = 51341;
    private IPEndPoint _serverIpEndPoint;
    private bool _isFinishConnectServer = false;

    private const int MaxPacketSize = 1500;
    public GameObject ConnectServerBtn;

    void OnApplicationQuit()
    {
        DisconnectServer();
    }

    public void ConnectServer()
    {
        try
        {
            _serverIp = Dns.GetHostEntry(ServerDomain).AddressList[0];
        }
        catch (Exception e)
        {
            Debug.LogError("해당 도메인을 찾을 수 없습니다");
            throw;
        }

        _serverIpEndPoint = new IPEndPoint(_serverIp, ServerPort);
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        //_socket.Connect(_serverIpEndPoint);
        _socket.SendTimeout = 3000;
        _socket.ReceiveTimeout = 3000;
        StartCoroutine(LoadingConnectServerCoroutine());
        _socket.BeginConnect(_serverIpEndPoint, new AsyncCallback(Connected), _socket);
    }

    private IEnumerator LoadingConnectServerCoroutine()
    {
        string text = "서버에 연결 시도 중";
        while (!_socket.Connected && !_isFinishConnectServer)
        {
            text += ".";
            Debug.Log(text);
            yield return new WaitForSeconds(1);
        }
    }

    private void Connected(IAsyncResult iar)
    {
        _socket = (Socket) iar.AsyncState;
        try
        {
            _socket.EndConnect(iar);
            Debug.Log($"{_socket.RemoteEndPoint.ToString()}에 접속 완료");
            //client.BeginReceive(data, 0, size, SocketFlags.None, new AsyncCallback(ReceiveData), client);
        }
        catch (SocketException)
        {
            Debug.Log("연결 실패");
        }

        _isFinishConnectServer = true;
    }

    private float timer = 0;

    private void Update()
    {
        Receive();
    }

    void Send(object packet)
    {
        if (packet == null)
        {
            Debug.LogError("패킷이 없습니다");
        }

        try
        {
            byte[] sendPacket = StructToByteArray(packet);
            _socket.Send(sendPacket, 0, sendPacket.Length, SocketFlags.None);
            Debug.Log($"{sendPacket.Length.ToString()}byte 사이즈 데이터 전송");
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }

    public void StartMatching()
    {
        cs_StartMatchingPacket packet;
        packet.size = (UInt16) Marshal.SizeOf<cs_StartMatchingPacket>();
        packet.type = (char) PacketType.cs_startMatching;
        packet.networkID = PlayerManager.Instance.Players[0].ID;
        byte[] nameBuf = new byte[22];
        var encodingStrBytes = Encoding.Unicode.GetBytes(PlayerManager.Instance.Players[0].NickName.Trim((char)8203));
        Array.Copy(encodingStrBytes,nameBuf, encodingStrBytes.Length);
        packet.name = nameBuf;
        Send(packet);
    }

    public void SendChangeCharacterPacket(int networkID, int characterType)
    {
        cs_sc_changeCharacterPacket packet = new cs_sc_changeCharacterPacket(networkID, (char)characterType);
        Send(packet);
    }

    public void SendChangeItemSlotPacket(Int32 networkID, Int16 slot1, Int16 slot2)
    {
        var packet = new cs_sc_changeItemSlotPacket(networkID, slot1, slot2);
        Send(packet);
    }

    public void SendBattleItemQueuePacket(Int32 networkID, Byte[] itemQueue)
    {
        var packet = new cs_sc_battleItemQueuePacket(networkID, itemQueue);
        Send(packet);
    }

    void Receive()
    {
        int receive = 0;
        if (_socket == null || !_socket.Connected)
        {
            return;
        }

        if (_socket.Available == 0)
        {
            return;
        }

        byte[] packet = new byte[MaxPacketSize];
        try
        {
            receive = _socket.Receive(packet);
        }
        catch (Exception ex)
        {
            //Debug.Log(ex.ToString());
            return;
        }

        if (receive <= 0)
        {
            return;
        }

        int size = BitConverter.ToUInt16(packet);
        byte[] bytes = new byte[size];
        Debug.Log($"{size}의 데이터를 받았습니다");
        Array.Copy(packet, 0, bytes, 0, size);
        switch ((PacketType) packet[2]) // type
        {
            case PacketType.sc_connectServer:
                var connectServerPacket = ByteArrayToStruct<sc_ConnectServerPacket>(bytes);
                //Users[i].networkID = connectServerPacket.networkID;4
                PlayerManager.Instance.Players[0].SetID(connectServerPacket.networkID);
                Debug.Log($"플레이어 네트워크ID : {connectServerPacket.networkID} 로 접속");
                break;
            case PacketType.sc_connectRoom:
                var connectRoomPacket = ByteArrayToStruct<sc_ConnectRoomPacket>(bytes);
                Debug.Log($"room id : {connectRoomPacket.roomNumber.ToString()}에 입장");
                PlayerManager.Instance.CreateEnemy(connectRoomPacket.users);
                WindowManager.Instance.SetWindow(3);
                FindObjectOfType<Select>().ChoiceCharacters[0].NetworkID = PlayerManager.Instance.Players[0].ID;
                FindObjectOfType<Select>().SetUserInfo(connectRoomPacket.users);
                break;
            case PacketType.cs_sc_addNewItem:
                var addNewItemPacket = ByteArrayToStruct<cs_sc_AddNewItemPacket>(bytes);
                //AddDebug($"{addNewItemPacket.networkID} 번 유저가 새로운 아이템 {addNewItemPacket.itemCode} 을 추가하였습니다");
                break;
            case PacketType.sc_disconnect:
                DisconnectServer();
                ConnectServerBtn.SetActive(true);
                Debug.Log("연결이 해제되었습니다");
                break;
            case PacketType.cs_sc_changeItemSlot:
                var changeItemSlotPacket = ByteArrayToStruct<cs_sc_changeItemSlotPacket>(bytes);
                PlayerManager.Instance.GetPlayer(changeItemSlotPacket.networkID).SwapItemNetwork(changeItemSlotPacket.slot1,changeItemSlotPacket.slot2);
                break;
            case PacketType.cs_sc_changeCharacter:
                var changeCharacterPacket = ByteArrayToStruct<cs_sc_changeCharacterPacket>(bytes);
                Debug.Log($"{changeCharacterPacket.networkID} -> {(int)changeCharacterPacket.characterType}");
                FindObjectOfType<Select>()
                    .ChangeCharacterImage(changeCharacterPacket.networkID, (int)changeCharacterPacket.characterType);
                break;
            default:
                Debug.LogError("새로운 패킷");
                break;
        }
    }

    void DisconnectServer()
    {
        if (_socket == null)
            return;

        if (_socket.Connected)
        {
            _socket.Close();
            _socket = null;
        }
    }

    byte[] StructToByteArray(object obj)
    {
        int size = Marshal.SizeOf(obj);
        byte[] arr = new byte[size];
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.StructureToPtr(obj, ptr, true);
        Marshal.Copy(ptr, arr, 0, size);
        Marshal.FreeHGlobal(ptr);
        return arr;
    }

    T ByteArrayToStruct<T>(byte[] buffer) where T : struct
    {
        int size = Marshal.SizeOf(typeof(T));
        if (size > buffer.Length)
        {
            throw new Exception();
        }

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.Copy(buffer, 0, ptr, size);
        T obj = (T) Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return obj;
    }
}
