using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviour
{
    private Socket _socket;
    private IPAddress _serverIp;
    private const string ServerDomain = "zecocostudio.com";
    private const int ServerPort = 51341;
    private IPEndPoint _serverIpEndPoint;
    private bool _isFinishConnectServer = false;

    private const int MaxPacketSize = 1500;
    public GameObject ConnectServerBtn;

    void Start()
    {
    }

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
        /*timer += Time.deltaTime;
        if (timer >= 0.01f)
        {
            timer = 0;
            for (int i = 0; i < Users.Length; i++)
            {
                if (!Users[i].isRoom)
                {
                    continue;
                }

                cs_sc_testPacket packet = new cs_sc_testPacket(Users[i].networkID, 0);
                Send(Users[i], packet);
            }
        }*/
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

    /*public void SendAddNewItem(User user, Int32 itemCode)
    {
        cs_sc_AddNewItemPacket packet;
        packet = new cs_sc_AddNewItemPacket(user.networkID, itemCode);
        Send(user, packet);
    }*/

    public void StartMatching()
    {
        Packet.cs_StartMatchingPacket packet;
        packet.size = (UInt16) Marshal.SizeOf<Packet.cs_StartMatchingPacket>();
        packet.type = (char) PacketType.cs_startMatching;
        packet.networkID = PlayerManager.Instance.Players[0].ID;
        packet.character = (char) 0;
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
                var connectServerPacket = ByteArrayToStruct<Packet.sc_ConnectServerPacket>(bytes);
                //Users[i].networkID = connectServerPacket.networkID;4
                PlayerManager.Instance.Players[0].SetID(connectServerPacket.networkID);
                Debug.Log($"플레이어 네트워크ID : {connectServerPacket.networkID} 로 접속");
                break;
            case PacketType.sc_connectRoom:
                var connectRoomPacket = ByteArrayToStruct<Packet.sc_ConnectRoomPacket>(bytes);
                Debug.Log($"room id : {connectRoomPacket.roomNumber.ToString()}에 입장");
                PlayerManager.Instance.CreateEnemy(connectRoomPacket.users);
                WindowManager.Instance.SetWindow(3);
                break;
            case PacketType.cs_sc_addNewItem:
                var addNewItemPacket = ByteArrayToStruct<Packet.cs_sc_AddNewItemPacket>(bytes);
                //AddDebug($"{addNewItemPacket.networkID} 번 유저가 새로운 아이템 {addNewItemPacket.itemCode} 을 추가하였습니다");
                break;
            case PacketType.sc_disconnect:
                DisconnectServer();
                ConnectServerBtn.SetActive(true);
                Debug.Log("연결이 해제되었습니다");
                break;
            case PacketType.cs_sc_changeItemSlot:
                //var packet1 = ByteArrayToStruct<Packet.cs_sc_changeItemSlotPacket>(bytes);
                break;
            case PacketType.cs_sc_changeCharacter:
                var changeCharacterPacket = ByteArrayToStruct<Packet.cs_sc_changeCharacterPacket>(bytes);
                FindObjectOfType<Select>()
                    .ChangeCharacterImage(changeCharacterPacket.networkID, changeCharacterPacket.characterType);
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