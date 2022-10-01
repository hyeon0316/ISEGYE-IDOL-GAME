using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattlePlayer : MonoBehaviour
{
    public int MaxAvaterHp { get; set; }
    public int AvatarHp { get; set; }//전투용 Hp
    public int AvaterDefense { get; set; }
    public ItemSlot[] ItemSlots;
    public Image AvatarImage;
    public TextMeshProUGUI PlayerNickName;
    public TextMeshProUGUI AvatarHpText;
    public TextMeshProUGUI DefenseText;
    
    public Player Player { get; private set; }
    private byte[] _itemOrder;
    private int _slotIndex = 0;
    private bool _isMyturn = false;

    public BattlePlayer Opponent { get; set; }
    private int _remainValue; //특정 데미지를 받아 방어도가 전부 깎이고 체력에 데미지를 줄때의 값을 저장하기 위한 변수

    private bool _isCC; //출혈 등 상태이상에 걸렸을때에 대한 bool값
    public ItemTriggerData ItemTriggerData;

    public void SetBattlePlayer(Player player, byte[] itemOrder, BattlePlayer oppoent)
    {
        Player = player;
        _itemOrder = itemOrder;
        Opponent = oppoent;
        
        Player.SetItem(ItemSlots);
        _isMyturn = false;
        MaxAvaterHp = 100;
        AvatarHp = MaxAvaterHp;
        AvaterDefense = Player.Defense;
        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
        DefenseText.text = $"방어력: {AvaterDefense}";
        AvatarImage.sprite = Player.Sprite;
        PlayerNickName.text = $"{Player.NickName}";
        _slotIndex = 0;
        ItemTriggerData.NestValue = 0;
        ItemTriggerData.UseAttackCount = 0;
    }

    public void UpdateAvatarHp(int amount)
    {
        if (amount < 0) //데미지
        {
            UpdateDefense(amount);
            AvatarHp += _remainValue;
            
            if (AvatarHp < 0) 
                AvatarHp = 0;
            else if(AvatarHp > MaxAvaterHp)
                AvatarHp = MaxAvaterHp;
        }
        else //회복
        {
            AvatarHp += amount;
            if (AvatarHp > MaxAvaterHp)
                AvatarHp = MaxAvaterHp;
        }

        AvatarHpText.text = $"아바타 체력: {AvatarHp}";
    }

    public void UpdateDefense(int amount)
    {
        _remainValue = 0;
        AvaterDefense += amount;
        if (AvaterDefense < 0) 
        {
            _remainValue = AvaterDefense;
            AvaterDefense = 0;
        }

        DefenseText.text = $"방어력: {AvaterDefense}";
    }

    public void SetFirstTurn()
    {
        _isMyturn = true;
    }

    public void ActiveCC(int amount)
    {
        _isCC = true;
        ItemTriggerData.NestValue += amount;
    }

    public void ClearCC()
    {
        _isCC = false;
        ItemTriggerData.NestValue = 0;
    }

    private void TakeCC()
    {
        if (_isCC)//아이템 발동 전 자신에게 상태이상이 걸렸을 경우
        {
            Debug.Log("상태이상");
            ActiveCCEfect();
            UpdateAvatarHp(ItemTriggerData.NestValue);
        }
    }

    public void TakeDiaSwordDamage(int amount)
    {
        UpdateAvatarHp(amount * (ItemTriggerData.UseAttackCount + 1));
        ItemTriggerData.UseAttackCount = 0;
    }

    public void NextActiveItem()
    {
        StartCoroutine(NextActiveItemCo());
    }

    private IEnumerator NextActiveItemCo()
    {
        yield return new WaitForSeconds(1f);
        this.GetComponentInParent<Battle>().ProgressBattle();
    }

    public void ActiveItem()
    {
        if (_isMyturn)
        {
            TakeCC();
            
            int itemSlot;
            bool active = false;

            itemSlot = _itemOrder[_slotIndex++];
            active = Convert.ToBoolean(_itemOrder[_slotIndex++]); //나중에 확률아이템에 대한 bool값
            if (_slotIndex == _itemOrder.Length)
                _slotIndex = 0;

            if (active)
            {
                if (ItemSlots[itemSlot].transform.childCount == 1)
                    if (ItemSlots[itemSlot].GetComponentInChildren<Item>().CurItemType == Item.ItemType.Attack) //비챤 검
                        ItemTriggerData.UseAttackCount++;

                ItemSlots[itemSlot].ActiveItem(this, Opponent);
            }
            else
            {
                Debug.Log("슬롯 잠김 OR 아이템 확률 실패");
                //todo: 아이템 잠겼을때 OR 아이템 확률 실패 했을 때 구분하여 이펙트 발생
            }
        }

        _isMyturn = !_isMyturn;
    }

    /// <summary>
    /// 프로토타입 용 출혈데미지 효과
    /// </summary>
    private void ActiveCCEfect()
    {
        StartCoroutine(ActiveCCEfectCo());
    }

    private IEnumerator ActiveCCEfectCo()
    {
        AvatarImage.color = Color.red;
        yield return new WaitForSeconds(0.5f);
        AvatarImage.color = Color.white;
    }
    
}
