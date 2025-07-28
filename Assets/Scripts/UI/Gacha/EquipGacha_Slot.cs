using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipGacha_Slot : MonoBehaviour
{
    [SerializeField] Image Item_Img;
    [SerializeField] Image Item_Rank_Img;
    [SerializeField] Image Rank_BG;

    // ���� �Ϸ� �� ��Ͽ� ǥ�� �� �̹��� ������ ���� �Լ�
    public void Set_GachaEquipItem(Sprite _item, Sprite _rank, Color _color)
    {
        // ������ �̹��� Sprite ����
        Item_Img.sprite = _item;
        // ������ ��� Sprite ����
        Item_Rank_Img.sprite = _rank;

        Rank_BG.color = _color;
    }
}
