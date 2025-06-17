using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System.Linq;

public class Item_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public static List<Inventory_Item> Spend_Item_List = new List<Inventory_Item>();
    public static List<Inventory_Item> Upgrade_Item_List = new List<Inventory_Item>();

    public static List<Item> Equipment_Item_List = new List<Item>();

    private void Awake()
    {
        #region Equip_Item
        for (int i = 0; i < GoogleSheetSORef.Item_DBList.Count; i++)
        {
            ITEM_TYPE.TryParse(GoogleSheetSORef.Item_DBList[i].ITEM_TYPE, out ITEM_TYPE itemType);
            EQUIP_TYPE.TryParse(GoogleSheetSORef.Item_DBList[i].EQUIP_TYPE, out EQUIP_TYPE equipType);

            //TODO ## Equipment_Item_List 아이템 데이터 저장
            Item Node = new Item(GoogleSheetSORef.Item_DBList[i].ITEM_ID, GoogleSheetSORef.Item_DBList[i].ITEM_NAME, GoogleSheetSORef.Item_DBList[i].ITEM_ATK, GoogleSheetSORef.Item_DBList[i].ITEM_DEF,
                GoogleSheetSORef.Item_DBList[i].ITEM_CRI_RATE, GoogleSheetSORef.Item_DBList[i].ITEM_CRI_DAMAGE, GoogleSheetSORef.Item_DBList[i].ITEM_HP, GoogleSheetSORef.Item_DBList[i].ITEM_VALUE_MIN_RANGE,
                GoogleSheetSORef.Item_DBList[i].ITEM_VALUE_MAX_RANGE, itemType, equipType);

            // TODO ## Item_List 아이템 이미지 리소스 저장
            Node.Load_Item_Icon(GoogleSheetSORef.Item_DBList[i].ITEM_IMAGE_ADDRESS);

            // 아이템 랜덤 옵션을 계산하기 위해 변수들 저장
            for (int index = 0; index < 8; index++)
            {
                EQUIPMENT_OPTION.TryParse(GoogleSheetSORef.EquipOption_DBList[index].OPTION_NAME, out EQUIPMENT_OPTION OptionType);

                EquipmentOption equipmentOption = new EquipmentOption();
                //Debug.Log($"{OptionType} : {GoogleSheetSORef.EquipOption_DBList[index].OPTION_MIN} / {GoogleSheetSORef.EquipOption_DBList[index].OPTION_MAX}");
                equipmentOption.Set_MinMax(GoogleSheetSORef.EquipOption_DBList[index].OPTION_MIN, GoogleSheetSORef.EquipOption_DBList[index].OPTION_MAX, OptionType);

                UserInfo.OptionList.Add(equipmentOption);
                // Node.UserInfo.OptionList.Add(equipmentOption);
            }

            // 이미지 누락 경고
            #region Image_Null_Warning
            if (Node.Get_Item_Image == null)
            {
                Debug.LogWarning($"{Node.Get_Item_Name}의 업그레이드 이미지가 없습니다");
            }
            #endregion

            if (Node.Get_ItemType == ITEM_TYPE.EQUIPMENT)
            {
                Equipment_Item_List.Add(Node);
            }
        }
        #endregion

        #region Inventory_Item
        for (int i = 0; i < GoogleSheetSORef.Inventory_Item_DBList.Count; i++)
        {
            // 아이템 타입
            INVENTORY_TYPE.TryParse(GoogleSheetSORef.Inventory_Item_DBList[i].INVENTORY_TYPE, out INVENTORY_TYPE invenType);

            // 아이템 데이터 생성
            Inventory_Item item = new Inventory_Item(GoogleSheetSORef.Inventory_Item_DBList[i].ITEM_NAME, invenType,
                GoogleSheetSORef.Inventory_Item_DBList[i].ITEM_AMOUNT, GoogleSheetSORef.Inventory_Item_DBList[i].ITEM_DESC);
            item.Load_Item_Icon(GoogleSheetSORef.Inventory_Item_DBList[i].ITEM_IMAGE);

            // 아이템 카피본으로 저장 필요
            if (item.Get_InventoryType == INVENTORY_TYPE.SPEND)
            {
                Spend_Item_List.Add(item);
            }
            else if (item.Get_InventoryType == INVENTORY_TYPE.UPGRADE)
            {
                Upgrade_Item_List.Add(item);
            }
        }
        #endregion

        #region Test
        //for (int i = 0; i < Spend_Item_List.Count; i++)
        //{
        //    Debug.Log(Spend_Item_List[i].Get_Item_Name);
        //}

        //for (int i = 0; i < Upgrade_Item_List.Count; i++)
        //{
        //    Debug.Log(Upgrade_Item_List[i].Get_Item_Name);
        //}
        #endregion
    }
}
