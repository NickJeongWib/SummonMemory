using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Store_Slot_Init : MonoBehaviour
{
    public Store_Item StoreItemInfo;
    public Store_Manager StoreManager_Ref;

    #region UI
    [SerializeField] Image ItemIcon;
    [SerializeField] Image Consume_Icon;
    [SerializeField] Text Item_Ex_Text;
    [SerializeField] Text Item_Name_Text;
    [SerializeField] Text Item_Cost_Text;
    #endregion

    public void Init_UI()
    {
        // StoreItemInfo ������ ������ null�� �ƴϸ�
        if (StoreItemInfo != null)
        {
            ItemIcon.sprite = StoreItemInfo.Get_Item_Icon;

            Item_Name_Text.text = $"{StoreItemInfo.Get_Item_Name}";
            Item_Cost_Text.text = $"{StoreItemInfo.Get_ConsumeCount.ToString("N0")}";

            // ������ ������ 0�� �ƴ϶��
            if (StoreItemInfo.Get_Item_Ex != 0)
                Item_Ex_Text.text = $"{StoreItemInfo.Get_Item_Ex.ToString("N0")}Ex";
            else
                Item_Ex_Text.text = "";

            // �����Ŵ����� null�� �ƴϸ�
            if (StoreManager_Ref != null)
                Consume_Icon.sprite = StoreManager_Ref.Consume_Icons[(int)StoreItemInfo.Get_ConsumeType];
        }
    }
    // ��ư Ŭ�� ��
    public void On_Click_Btn()
    {
        if (StoreItemInfo.Get_StoreType == STORE_TYPE.CURRENCY &&
            StoreManager_Ref != null)
        {
            StoreManager_Ref.On_Click_Buy_Dia(StoreItemInfo.Get_Item_Ex);
        }
    }

    // ������ ���� ��ư Ŭ�� ��
    public void On_Click_ItemInfo()
    {
        if (StoreManager_Ref != null)
        {
            StoreManager_Ref.On_ItemInfo_Pop(StoreItemInfo);
        }
    }
}
