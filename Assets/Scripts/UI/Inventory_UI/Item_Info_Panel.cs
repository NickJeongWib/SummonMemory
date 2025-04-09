using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class Item_Info_Panel : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Image Item_Image;

    [SerializeField] Image Item_Back;
    [SerializeField] Color[] Item_Grade_Colors;

    [Header("---Spend_Item---")]
    [SerializeField] GameObject Spend_Item_Root;
    public GameObject Get_Spend_Item_Root { get => Spend_Item_Root; }
    [SerializeField] Text Spend_Item_Name;
    [SerializeField] Text Spend_Item_Type;
    [SerializeField] Text Spend_Item_Ex;
    [SerializeField] Text Spend_Item_Des;

    [Header("---Equipment_Item---")]
    [SerializeField] GameObject Equipment_Item_Root;
    public GameObject Get_Equipment_Item_Root { get => Equipment_Item_Root; }
    [SerializeField] Text Equipment_Item_Name;
    [SerializeField] Text Equipment_Item_Grade;
    [SerializeField] Text Equipment_Base_Option;

    [SerializeField] Text Equipment_Random_Option_1;
    [SerializeField] Text Equipment_Random_Option_2;
    [SerializeField] Text Equipment_Random_Option_3;


    public void On_Click_Close_ItemInfo()
    {
        animator.Play("Pop_Down_Item_Info");
    }

    public void ActiveF_Panel()
    {
        this.gameObject.SetActive(false);
    }
    
    public void ItemInfo_Refresh(INVENTORY_TYPE _invenType, int _num)
    {
        if (_invenType == INVENTORY_TYPE.EQUIPMENT)
        {
            Equipment_Item_Root.SetActive(true);
            Spend_Item_Root.SetActive(false);

            Refresh_Equipment(_num);
        }
        else if (_invenType == INVENTORY_TYPE.SPEND)
        {
            Equipment_Item_Root.SetActive(false);
            Spend_Item_Root.SetActive(true);

            Refresh_Spend();
        }
        else
        {

        }
    }

    void Refresh_Spend()
    {
        Item_Back.gameObject.SetActive(false);
    }

    void Refresh_Equipment(int _num)
    {
        Equipment_Item_Name.text = UserInfo.Equip_Inventory[_num].Get_Item_Name;

        Item_Back.gameObject.SetActive(true);
        
        Item_Image.sprite = UserInfo.Equip_Inventory[_num].Get_Item_Image;

        Base_Option(UserInfo.Equip_Inventory[_num].Get_Item_Atk, UserInfo.Equip_Inventory[_num].Get_Item_DEF,
            UserInfo.Equip_Inventory[_num].Get_Item_HP, UserInfo.Equip_Inventory[_num].Get_Item_CRI_RATE,
            UserInfo.Equip_Inventory[_num].Get_Item_CRI_DMG);
    }

    void Base_Option(float _atk, float _def, float _hp, float _criR, float _criD)
    {
        Equipment_Base_Option.text = "";

        if (_atk != 0)
        {
            Equipment_Base_Option.text = $"<color=orange>공격력</color> +{_atk.ToString("N0")}\n";
        }
        
        if (_def != 0)
        {
            Equipment_Base_Option.text += $"<color=orange>방어력</color> +{_def.ToString("N0")}\n";
        }

        if (_hp != 0)
        {
            Equipment_Base_Option.text += $"<color=orange>체력</color> +{_hp.ToString("N0")}\n";
        }

        if (_criR != 0)
        {
            Equipment_Base_Option.text += $"<color=orange>크리티컬 확률</color> +{(_criR * 100.0f).ToString("N1")}%\n";
        }

        if (_criD != 0)
        {
            Equipment_Base_Option.text += $"<color=orange>크리티컬 데미지</color> +{(_criD * 100.0f).ToString("N1")}%\n";
        }
    }
}

