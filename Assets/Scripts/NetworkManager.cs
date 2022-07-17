using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviour
{
    private Socket m_Client;
    public string m_Ip = "221.151.106.33";
    public int m_Port = 51341;
    private IPEndPoint m_ServerIpEndPoint;
    private EndPoint m_RemoteEndPoint;
    private TestPacket _mSendTestPacket;
    private TestPacket _mReceiveTestPacket;

    private List<Socket> _sockets;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    [Serializable]
    struct TestPacket
    {
        public UInt32 type;
        public UInt32 networkId;
        public int c;
    }

    void Start()
    {
        _sockets = new List<Socket>();
        //InitClient();
    }

    void OnApplicationQuit()
    {
        CloseClient();
    }

    public void InitClient()
    {
        m_ServerIpEndPoint = new IPEndPoint(IPAddress.Parse(m_Ip), m_Port);
        m_Client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        m_Client.Connect(m_ServerIpEndPoint);
        m_Client.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
        _sockets.Add(m_Client);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            InitClient();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CloseClient();
        }

        if (m_Client != null)
        {
            Receive();
            //SetIncSendPacket();
            //Send();
        }
    }

    public void SetRandomSendPacket()
    {
        _mSendTestPacket.type = (uint) Random.Range(0,20);
        _mSendTestPacket.networkId = 100;
        _mSendTestPacket.c = Random.Range(0,9999);
        
        Send();
    }

    private int num = 1;

    void Send()
    {
        try
        {
            byte[] sendPacket = StructToByteArray(_mSendTestPacket);
            m_Client.Send(sendPacket, 0, sendPacket.Length, SocketFlags.None);
            Debug.Log($"{_mSendTestPacket.type} {_mSendTestPacket.networkId} {_mSendTestPacket.c}");
        }

        catch (Exception ex)
        {
            Debug.Log(ex.ToString());
            return;
        }
    }

    void Receive()
    {
        int receive = 0;
        foreach (Socket socket in _sockets)
        {
            if (socket.Available != 0)
            {
                byte[] packet = new byte[1024];

                try
                {
                    receive = socket.Receive(packet);
                }

                catch (Exception ex)
                {
                    //Debug.Log(ex.ToString());
                    return;
                }

                _mReceiveTestPacket = ByteArrayToStruct<TestPacket>(packet);

                if (receive > 0)
                {
                    DoReceivePacket(); // 받은 값 처리
                }
            }
        }
    }

    void DoReceivePacket()
    {
        Debug.LogFormat($"{_mReceiveTestPacket.type} {_mReceiveTestPacket.networkId} {_mReceiveTestPacket.c}");
        //출력: m_IntArray[0] = 7 m_IntArray[1] = 47 FloatlVariable = 2020 StringlVariable = Coder ZeroBoolVariable = True IntlVariable = 13 
    }

    void CloseClient()
    {
        foreach (var socket in _sockets)
        {
            if (socket != null)
            {
                socket.Close();
            }
        }

        m_Client = null;
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
        T obj = (T)Marshal.PtrToStructure(ptr, typeof(T));
        Marshal.FreeHGlobal(ptr);
        return obj;
    }
}
