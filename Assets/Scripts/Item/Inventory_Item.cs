using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using System;

[Serializable]
public class Inventory_Item
{
    [SerializeField]  INVENTORY_TYPE InventoryType;
    public INVENTORY_TYPE Get_InventoryType { get => InventoryType; }

    [SerializeField] string Item_Name;
    public string Get_Item_Name { get => Item_Name; }

    [SerializeField] int Amount;
    public int Get_Amount { get => Amount; set => Amount = value; }

    [SerializeField] string Item_Desc;
    public string Get_Item_Desc { get => Item_Desc; set => Item_Desc = value; }

    [SerializeField] string ItemIcon_Address;
    public string Get_ItemIcon_Address { get => ItemIcon_Address; }
    // 아이템 이미지 
    Sprite Item_Image;
    public Sprite Get_Item_Image { get => Item_Image; set => Item_Image = value; }

    #region Constructor
    public Inventory_Item(string _name = "", INVENTORY_TYPE _invenType = INVENTORY_TYPE.SPEND, int _amount = 0, string _desc = "")
    {
        Item_Name = _name;
        InventoryType = _invenType;
        Amount = _amount;
        Item_Desc = _desc;
    }
    #endregion

    public void Load_Item_Icon(string _add)
    {
        Item_Image = Resources.Load<Sprite>(_add);
        ItemIcon_Address = _add;
    }

    public void Copy_Data(Inventory_Item _item)
    {
        _item.Item_Name = this.Item_Name;
        _item.InventoryType = this.InventoryType;
        _item.Amount = this.Amount;
        _item.Item_Desc = this.Item_Desc;
        _item.Item_Image = this.Item_Image;
    }
}
