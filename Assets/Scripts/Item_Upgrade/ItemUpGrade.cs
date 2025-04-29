using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class ItemUpGrade : MonoBehaviour
{
    Item SelectItem;
    public Item Get_SelectItem { get => SelectItem; set => SelectItem = value; }

    [Header("Ref")]
    [SerializeField] Item_Info_Panel Item_Info_Panel_Ref;
    [SerializeField] Inventory_UI Inventory_UI_Ref;

    [Header("Upgrade_Root")]
    [SerializeField] GameObject UpgradeRoot;
    [SerializeField] int[] UpGrade_Rate;
    [SerializeField] float ItemState_Rate;

    [Header("---Item_UI---")]
    [SerializeField] Image ItemImage;
    [SerializeField] Image ItemGradeBack;
    [SerializeField] Image ItemGrade;
    [SerializeField] Text ItemName;
    [SerializeField] Text ItemOption;
    [SerializeField] Image[] UpgradeImage;

    private void OnEnable()
    {
        if (SelectItem == null)
            return;

        if (SelectItem.Get_Item_Lv >= 9)
        {
            UpgradeRoot.SetActive(false);
        }
        else
        {
            for (int i = 0; i < UpgradeImage.Length; i++)
            {
                UpgradeImage[i].gameObject.SetActive(false);
            }
            UpgradeRoot.SetActive(true);
            Refresh_Upgrade_Option();
        }

        ItemImage.sprite = SelectItem.Get_Item_Image;
        ItemGrade.sprite = Item_Info_Panel_Ref.Get_Grade_Sprites[(int)SelectItem.Get_Equipment_Grade];
        ItemGradeBack.color = Inventory_UI_Ref.Get_Colors[(int)SelectItem.Get_Equipment_Grade];
        ItemName.text = SelectItem.Get_Item_Name;
    }

    #region Item_Upgrade
    void Refresh_Upgrade_Option()
    {
        // ItemOption.text = 
        // �ؽ�Ʈ �ʱ�ȭ
        ItemOption.text = "";

        if (SelectItem.Get_Item_Atk != 0)
        {
            ItemOption.text += $"���ݷ�            {SelectItem.Get_Item_Atk.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_Atk + SelectItem.Get_Item_Atk * ItemState_Rate).ToString("N0")}</color>\n\n";
            NextImage_Refresh();
        }

        if (SelectItem.Get_Item_DEF != 0)
        {
            ItemOption.text += $"����            {SelectItem.Get_Item_DEF.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_DEF + SelectItem.Get_Item_DEF * ItemState_Rate).ToString("N0")}</color>\n\n";
            NextImage_Refresh();
        }

        if (SelectItem.Get_Item_HP != 0)
        {
            ItemOption.text += $"ü��                {SelectItem.Get_Item_HP.ToString("N0")}             <color=orange>{(SelectItem.Get_Item_HP + SelectItem.Get_Item_HP * ItemState_Rate).ToString("N0")}</color>\n\n";
            NextImage_Refresh();
        }

        if (SelectItem.Get_Item_CRI_RATE != 0)
        {
            ItemOption.text += $"ġ��Ȯ��        {(SelectItem.Get_Item_CRI_RATE * 100).ToString("N0")}%            <color=orange>{((SelectItem.Get_Item_CRI_RATE * 100) + ((SelectItem.Get_Item_CRI_RATE * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            NextImage_Refresh();
        }

        if (SelectItem.Get_Item_CRI_DMG != 0)
        {
            ItemOption.text += $"ġ������        {(SelectItem.Get_Item_CRI_DMG * 100).ToString("N0")}%             <color=orange>{((SelectItem.Get_Item_CRI_DMG * 100) + ((SelectItem.Get_Item_CRI_DMG * 100) * ItemState_Rate)).ToString("N1")}%</color>\n\n";
            NextImage_Refresh();
        }

    }

    void NextImage_Refresh()
    {
        for (int i = 0; i < UpgradeImage.Length; i++)
        {
           if(UpgradeImage[i].gameObject.activeSelf == false)
           {
                UpgradeImage[i].gameObject.SetActive(true);
                break;
           }
           else
           {
                continue;
           }
        }
    }

    // ������ ��ȭ ���� �� ĳ���Ϳ� ����� �ɷ�ġ�� ���� �� 10�� ������ ���� ���Ѵ�
    public void Refresh_Character_Ability()
    {
        if (SelectItem.Get_OwnCharacter == null)
            return;

        // ���� �������� ����� ���� �ɷ�ġ�� ���ش� ������ �� EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true)������ ȣ���Ͽ� �ٽ� ����� �ʿ䰡����
        SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, false);
    }

    // TODO ## ItemUpGrade ������ ���׷��̵�
    public void On_Click_UpgradeBtn()
    {
        // �ִ� ���� ���� �� ����x
        if (SelectItem.Get_Item_Lv >= 9)
        {
            return;
        }

        Debug.Log(SelectItem.Get_Item_Lv);
        int Rate = Random.Range(1, 101);
        Debug.Log(Rate);

        // ���� ����
        if (UpGrade_Rate[SelectItem.Get_Item_Lv] >= Rate)
        {
            Refresh_Character_Ability();
            SelectItem.UpGrade_Success(ItemState_Rate);

            // �������� ĳ���Ͱ� �ִٸ�
            if (SelectItem.Get_OwnCharacter != null)
            {
                SelectItem.Get_OwnCharacter.EquipmentUpgrade_State_Refresh(SelectItem.Get_EquipType, true);
            }
        }
        else // ����
        {

        }

        Refresh_Upgrade_Option();
    }
    #endregion
}
