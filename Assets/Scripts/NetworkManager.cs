using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Packet;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class NetworkManager : Singleton<NetworkManager>
{
    private Socket _socket;
    private IPAddress _serverIp;
    private const string ServerDomain = "zecocostudio.com";
    private const int ServerPort = 51341;
    private const int ServerLocalPort = 51342;
    private IPEndPoint _serverIpEndPoint;
    private bool _isFinishConnectServer = false;
    private bool _isSuccessConnectServer = false;

    private const int MaxPacketSize = 1500;
    public TMP_Text DebugText;

    private void Start()
    {
        //Invoke("StartMatching", 1f);
    }

    void OnApplicationQuit()
    {
        DisconnectServer();
    }

    // 서버 컴퓨터 서버
    public void StartConnectServer()
    {
        ConnectServer(ServerPort);
    }

    // 개인 컴퓨터 서버
    public void StartConnectLocalServer()
    {
        ConnectServer(ServerLocalPort);
    }

    private void ConnectServer(int port)
    {
        try
        {
            _serverIp = Dns.GetHostEntry(ServerDomain).AddressList[0];
        }
        catch (Exception)
        {
            Debug.LogError("해당 도메인을 찾을 수 없습니다");
            throw;
        }

        _serverIpEndPoint = new IPEndPoint(_serverIp, port);
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
            _isSuccessConnectServer = true;
        }
        catch (SocketException)
        {
            Debug.Log("연결 실패");
        }

        _isFinishConnectServer = true;
    }

    private void Update()
    {
        Receive();
        if (_isSuccessConnectServer)
        {
            _isSuccessConnectServer = false;
            Invoke("SendStartMatchingPacket", 1);
        }
    }

    void SendPacket(object packet)
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

    private void SendStartMatchingPacket()
    {
        cs_StartMatchingPacket packet;
        packet.size = (UInt16) Marshal.SizeOf<cs_StartMatchingPacket>();
        packet.type = EPacketType.cs_startMatching;
        packet.networkID = PlayerManager.Instance.Players[0].ID;
        byte[] nameBuf = new byte[22];
        var encodingStrBytes = Encoding.Unicode.GetBytes(PlayerManager.Instance.Players[0].NickName.Trim((char) 8203));
        Array.Copy(encodingStrBytes, nameBuf, encodingStrBytes.Length);
        packet.name = nameBuf;
        SendPacket(packet);
    }

    public void SendChangeCharacterPacket(int networkID, int characterType)
    {
        cs_sc_ChangeCharacterPacket packet = new cs_sc_ChangeCharacterPacket(networkID, (ECharacterType) characterType);
        SendPacket(packet);
    }

    public void SendChangeItemSlotPacket(Int32 networkID, Byte slot1, Byte slot2)
    {
        if (slot1 == slot2)
            return;

        var packet = new cs_sc_ChangeItemSlotPacket(networkID, slot1, slot2);
        SendPacket(packet);
    }

    public void SendBattleReadyPacket(Int32 networkID, Int16 firstAttack)
    {
        cs_BattleReadyPacket packet = new cs_BattleReadyPacket(networkID, firstAttack);
        DebugText.text = networkID.ToString();
        SendPacket(packet);
    }

    public void SendAddNewItemPacket(Int32 networkID, Byte itemCode)
    {
        cs_sc_AddNewItemPacket packet = new cs_sc_AddNewItemPacket(networkID, itemCode);
        SendPacket(packet);
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
        catch (Exception)
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
        switch ((EPacketType) packet[2]) // type
        {
            case EPacketType.sc_connectServer:
                var connectServerPacket = ByteArrayToStruct<sc_ConnectServerPacket>(bytes);
                //Users[i].networkID = connectServerPacket.networkID;4
                PlayerManager.Instance.Players[0].SetID(connectServerPacket.networkID);
                Debug.Log($"플레이어 네트워크ID : {connectServerPacket.networkID} 로 접속");
                break;
            case EPacketType.sc_connectRoom:
                var connectRoomPacket = ByteArrayToStruct<sc_ConnectRoomPacket>(bytes);
                Debug.Log("room에 입장");
                PlayerManager.Instance.CreateEnemy(connectRoomPacket.users);
                WindowManager.Instance.SetWindow((int) EWindowType.Select);
                FindObjectOfType<Select>().ChoiceCharacters[0].NetworkID = PlayerManager.Instance.Players[0].ID;
                FindObjectOfType<Select>().SetUserInfo(connectRoomPacket.users);
                break;
            case EPacketType.cs_sc_addNewItem:
                var addNewItemPacket = ByteArrayToStruct<cs_sc_AddNewItemPacket>(bytes);
                PlayerManager.Instance.GetPlayer(addNewItemPacket.networkID).UnUsingInventory
                             .AddItem((EItemCode) addNewItemPacket.itemCode);
                //AddDebug($"{addNewItemPacket.networkID} 번 유저가 새로운 아이템 {addNewItemPacket.itemCode} 을 추가하였습니다");
                break;
            case EPacketType.sc_disconnect:
                DisconnectServer();
                Debug.Log("연결이 해제되었습니다");
                break;
            case EPacketType.cs_sc_changeItemSlot:
                var changeItemSlotPacket = ByteArrayToStruct<cs_sc_ChangeItemSlotPacket>(bytes);
                PlayerManager.Instance.GetPlayer(changeItemSlotPacket.networkID)
                             .SwapItemNetwork(changeItemSlotPacket.slot1, changeItemSlotPacket.slot2);
                break;
            case EPacketType.cs_sc_changeCharacter:
                var changeCharacterPacket = ByteArrayToStruct<cs_sc_ChangeCharacterPacket>(bytes);
                Debug.Log($"{changeCharacterPacket.networkID} -> {(int) changeCharacterPacket.characterType}");
                FindObjectOfType<Select>()
                    .ChangeCharacterImage(changeCharacterPacket.networkID, (int) changeCharacterPacket.characterType);
                break;
            case EPacketType.sc_battleInfo:
                var battleInfoPacket = ByteArrayToStruct<sc_BattleInfoPacket>(bytes);
                for (int i = 0; i < battleInfoPacket.itemQueueInfos.Length; i++)
                {
                    int playerID = battleInfoPacket.itemQueueInfos[i].networkID;
                    if (playerID == -1)
                        break;

                    Player player = PlayerManager.Instance.GetPlayer(playerID);
                    player.ActiveIndex = battleInfoPacket.itemQueueInfos[i].itemQueue;
                }

                BattleManager battleManager = FindObjectOfType<InGame>().transform.Find("Background").transform
                                                                        .Find("Battle").GetComponent<BattleManager>();
                battleManager.BattleOpponents.Clear();
                for (int i = 0; i < battleInfoPacket.battleOpponents.Length; i++)
                {
                    if (battleInfoPacket.battleOpponents[i] == Int32.MaxValue)
                        break;
                    battleManager.BattleOpponents.Add(battleInfoPacket.battleOpponents[i]);
                }

                FindObjectOfType<InGame>().OpenBattle();
                break;
            case EPacketType.sc_ping:
                break;
            default:
                Debug.LogError("새로운 패킷");
                break;
        }
    }

    public void DisconnectServer()
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
