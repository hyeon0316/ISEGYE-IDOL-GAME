using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 각 아이템 발동 효과 관련 사용될 데이터 모음
/// </summary>
public class ItemTriggerData : MonoBehaviour
{
    public int UseAttackCount { get; set; }//아이템 타입 중 Attack의 사용 횟수를 저장하기 위한 변수
    public int NestValue { get; set; } //상태이상 데미지에 대한 중첩값이 담긴 변수
}
