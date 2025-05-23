using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using static Define;

public class Store_Item
{
    #region Variable
    //public Store_Manager StoreManagerRef;

    string Item_Name;
    public string Get_Item_Name { get => Item_Name; set => Item_Name = value; }

    STORE_TYPE StoreType;
    public STORE_TYPE Get_StoreType { get => StoreType; set => StoreType = value; }

    int Item_Ex;
    public int Get_Item_Ex { get => Item_Ex; set => Item_Ex = value; }

    CONSUME_TYPE ConsumeType;
    public CONSUME_TYPE Get_ConsumeType { get => ConsumeType; set => ConsumeType = value; }

    INVENTORY_TYPE InvenType;
    public INVENTORY_TYPE Get_InvenType { get => InvenType; set => InvenType = value; }

    int ConsumeCount;
    public int Get_ConsumeCount { get => ConsumeCount; set => ConsumeCount = value; }

    Sprite Item_Icon;
    public Sprite Get_Item_Icon { get => Item_Icon; set => Item_Icon = value; }

    string Item_Desc;
    public string Get_Item_Desc { get => Item_Desc; set => Item_Desc = value; }
    #endregion

    public Store_Item(string _name, INVENTORY_TYPE _invenType, STORE_TYPE _storeType, int _itemEx, CONSUME_TYPE _consumeType, int _consumeCount, string _iconAdd, string _itemDesc)
    {
        Item_Name = _name;
        InvenType = _invenType;
        StoreType = _storeType;
        Item_Ex = _itemEx;
        ConsumeType = _consumeType;
        ConsumeCount = _consumeCount;
        Item_Desc = _itemDesc;
        Item_Icon = Resources.Load<Sprite>(_iconAdd);
    }
}
