using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 일단 안쓸 예정
/// </summary>
public class ItemCode : Singleton<ItemCode>
{
    public readonly List<int> RandList = new List<int>(15);

    public readonly List<int> SelectList = new List<int>(3);

    /// <summary>
    /// min~max값 사이에서 중복값 제외 리턴
    /// </summary>
    public int DistinctValue(int min, int max)
    {
        int curNum = Random.Range(min, max + 1);

        while (true)
        {
            if (RandList.Contains(curNum))
            {
                curNum = Random.Range(min, max + 1);
            }
            else
            {
                RandList.Add(curNum);
                break;
            }
        }
        
        return curNum;
    }

    /// <summary>
    /// 이미 사용된 ItemCode를 제외한 나머지 값들 리턴
    /// </summary>
    public int RemainValue(int min, int max)
    {
        int curNum = Random.Range(min, max + 1);

        while (true)
        {
            if (RandList.Contains(curNum))
                curNum = Random.Range(min, max + 1);
            else
                break;
        }
        return curNum;
    }
}
