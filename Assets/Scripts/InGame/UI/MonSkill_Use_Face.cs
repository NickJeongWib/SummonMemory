using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonSkill_Use_Face : MonoBehaviour
{
    public Image MonIcon;
    public Text Mon_Name;
    public Text Desc;

    [SerializeField] Animator animator;

    public void Set_UI_Init(Sprite _icon, string _name)
    {
        this.gameObject.SetActive(true);
        MonIcon.sprite = _icon;
        Mon_Name.text = _name;
    }

    public void Set_UI_Init(Sprite _icon, string _name, string _text, string _animString)
    {
        this.gameObject.SetActive(true);
        // 재생시킬 애니메이션 정해주기
        animator.Play(_animString);

        MonIcon.sprite = _icon;
        Mon_Name.text = _name;
        Desc.text = _text;
    }
}
