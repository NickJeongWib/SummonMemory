using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonSkill_Use_Face : MonoBehaviour
{
    public Image MonIcon;
    public Text Mon_Name;

    public void Set_UI_Init(Sprite _icon, string _name)
    {
        this.gameObject.SetActive(true);
        MonIcon.sprite = _icon;
        Mon_Name.text = _name;
    }
}
