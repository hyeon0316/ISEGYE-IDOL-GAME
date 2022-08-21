using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WindowType //todo: 매칭창 추가
{
   Lobby,
   Server,
   Loading,
   Select,
   InGame
}

public class WindowManager : Singleton<WindowManager>
{
   [SerializeField] private GameObject[] _windows;
   public GameObject[] Windows => _windows; 
      
   private void Start()
   {
      SetWindow((int)WindowType.Lobby);
   }

   public void SetWindow(int windowNum)
   {
      for (int i = 0; i < _windows.Length; i++)
      {
         if (i == windowNum)
            _windows[i].SetActive(true);
         else
            _windows[i].SetActive(false);
      }
   }
   
   
   
}
