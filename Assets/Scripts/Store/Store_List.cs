using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System.Linq;

public class Store_List : MonoBehaviour
{
    [SerializeField] GoogleSheetSO GoogleSheetSORef;

    public static List<Store_Item> CurrencyList = new List<Store_Item>();
    public static List<Store_Item> TicketList = new List<Store_Item>();
    public static List<Store_Item> R_BookList = new List<Store_Item>();
    public static List<Store_Item> SR_BookList = new List<Store_Item>();
    public static List<Store_Item> SSR_BookList = new List<Store_Item>();

    private void Awake()
    {
        for (int i = 0; i < GoogleSheetSORef.Store_Item_DBList.Count; i++)
        {
            STORE_TYPE.TryParse(GoogleSheetSORef.Store_Item_DBList[i].STORE_TYPE, out STORE_TYPE storeType);
            CONSUME_TYPE.TryParse(GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_CONSUME_TYPE, out CONSUME_TYPE consumeType);
            INVENTORY_TYPE.TryParse(GoogleSheetSORef.Store_Item_DBList[i].INVENTORY_TYPE, out INVENTORY_TYPE invenType);

            Store_Item node = new Store_Item(GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_NAME, invenType, storeType, GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_EX,
                consumeType, GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_CONSUME_COUNT, GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_ICON,
                GoogleSheetSORef.Store_Item_DBList[i].STORE_ITEM_DESC);

            // 상점 리스트에 추가
            if (node.Get_StoreType == STORE_TYPE.CURRENCY)
                CurrencyList.Add(node);
            else if (node.Get_StoreType == STORE_TYPE.TICKET)
                TicketList.Add(node);
            else if (node.Get_StoreType == STORE_TYPE.R_BOOK)
                R_BookList.Add(node);
            else if (node.Get_StoreType == STORE_TYPE.SR_BOOK)
                SR_BookList.Add(node);
            else
                SSR_BookList.Add(node);
        }
    }
}
