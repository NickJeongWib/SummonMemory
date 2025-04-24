using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfo_Pop : MonoBehaviour
{
    public Store_Item StoreItem_Info;

    [SerializeField] Text ItemName;
    [SerializeField] Text UserItem_Count;
    [SerializeField] Text Item_Desc;
    [SerializeField] Image ItemIcon;

    // UI 초기화
    public void Init_ItemInfo_UI(int _num)
    {
        if (StoreItem_Info != null)
        {
            ItemName.text = StoreItem_Info.Get_Item_Name;
            ItemIcon.sprite = StoreItem_Info.Get_Item_Icon;

            // 줄바꿈 및 \기호 삭제
            StoreItem_Info.Get_Item_Desc = StoreItem_Info.Get_Item_Desc.Replace("\\n", "\n");
            StoreItem_Info.Get_Item_Desc = StoreItem_Info.Get_Item_Desc.Replace("\\", "");
            Item_Desc.text = StoreItem_Info.Get_Item_Desc;

            UserItem_Count.text = $"유저 보유 {_num.ToString("N0")}개";
        }
    }



    // StoreItem_Info 받아오기
    public void Set_StoreItem(Store_Item _storeInfo)
    {
        StoreItem_Info = _storeInfo;
    }
}
