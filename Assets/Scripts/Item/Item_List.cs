using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System.Linq;

public class Item_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public static List<Item> Spend_Item_List = new List<Item>();
    public static List<Item> Equipment_Item_List = new List<Item>();

    private void Awake()
    {
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
            else
            {
                Spend_Item_List.Add(Node);
            }
        }
    }
}
