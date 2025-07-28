using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipGacha_Slot : MonoBehaviour
{
    [SerializeField] Image Item_Img;
    [SerializeField] Image Item_Rank_Img;
    [SerializeField] Image Rank_BG;

    // 가차 완료 후 목록에 표시 할 이미지 새팅을 위한 함수
    public void Set_GachaEquipItem(Sprite _item, Sprite _rank, Color _color)
    {
        // 아이템 이미지 Sprite 설정
        Item_Img.sprite = _item;
        // 아이템 등급 Sprite 설정
        Item_Rank_Img.sprite = _rank;

        Rank_BG.color = _color;
    }
}
