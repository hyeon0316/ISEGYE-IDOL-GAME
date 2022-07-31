using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

/// <summary>
/// 일단 안쓸 예정
/// </summary>
public class RandomSelect : MonoBehaviour
{
    private const int INDEX = 1;
    
    private SetItems _setItems;
    public RandomButton[] SelectBtn;
    
    private void Awake()
    {
        SelectBtn = this.GetComponentsInChildren<RandomButton>();
        _setItems = FindObjectOfType<SetItems>();
    }

    private void OnEnable()
    {
        ShowItem();
    }

    
    /// <summary>
    /// 랜덤으로 소지하지 않은 3개의 아이템을 보여줌
    /// </summary>
    private void ShowItem() 
    {
        int itemCode = 0;
        ItemCode.Instance.SelectList.Clear();
        List<int> overlapNum = new List<int>();//3개의 아이템 사이에서도 중복을 제거하기 위한 변수
        
        for (int i = 0; i < SelectBtn.Length; i++)
        {
            itemCode = ItemCode.Instance.RemainValue(1, 15);
            while (true)
            {
                if(overlapNum.Contains(itemCode))
                    itemCode = ItemCode.Instance.RemainValue(1, 15);
                else
                {
                    overlapNum.Add(itemCode);
                    ItemCode.Instance.SelectList.Add(itemCode);
                    break;
                }
            }
            SelectBtn[i].SetData(_setItems.Items[itemCode - INDEX].GetComponent<Item>().Sprite);
        }
    }
}
