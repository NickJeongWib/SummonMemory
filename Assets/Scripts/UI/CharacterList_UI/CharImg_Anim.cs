using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharImg_Anim : MonoBehaviour
{
    [SerializeField] int ImageIndex;
    public int Get_ImageIndex { get => ImageIndex; set => ImageIndex = value; }

    [SerializeField] Image CharacterImage;
    public Image Get_CharacterImage { get => CharacterImage; set => CharacterImage = value; }


    [SerializeField] Animator CharChange_animator;
    public Animator Get_CharChange_animator { get => CharChange_animator; }

    private void Start()
    {
        CharChange_animator.enabled = false;

        // TODO ## �ʱ� �׽�Ʈ ��
        Debug.Log("Count : " + UserInfo.Equip_Characters.Count);
        
        if (UserInfo.Equip_Characters.Count == 0)
        {
            Debug.LogError("����");
        }
        else
        {
            Debug.Log(UserInfo.Equip_Characters[0].Get_CharName);
        }
       
        R_SR_Image_Change(0);
    }

    public void CharImage_ChangeAnimF()
    {
        // ĳ���� �̹��� ���� ��ũ��Ʈ �۵�
        if (UserInfo.Equip_Characters.Count < 2)
        {
            CharacterImage.color = Color.white; // ���İ� x
            CharChange_animator.enabled = false;

            // R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
            R_SR_Image_Change(0);
        }
        else if (2 <= UserInfo.Equip_Characters.Count)
        {
            CharChange_animator.enabled = true;

            // R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
            R_SR_Image_Change(0);
        }
    }

    // �ִϸ��̼� �̺�Ʈ �Լ�
    public void CharImg_Change()
    {
        if (UserInfo.Equip_Characters[Get_NextIndex()].Get_CharGrade != Define.CHAR_GRADE.R)
        {
            UserInfo.Get_Square_Image(CharacterImage, ImageIndex);
            // ĳ���� �̹��� ��ü
            // CharacterImage.sprite = UserInfo.Equip_Characters[ImageIndex].Get_SquareIllust_Img;
        }
        else
        {
            UserInfo.Get_Square_Image(CharacterImage, ImageIndex);
            // ĳ���� �̹��� ��ü
            //CharacterImage.sprite = UserInfo.Equip_Characters[ImageIndex].Get_Illust_Img;
        }
    }

    int Get_NextIndex()
    {
        ImageIndex++;

        // ���� ĳ���� ���� �Ѿ�ٸ� 
        if (ImageIndex >= UserInfo.Equip_Characters.Count)
        {
            // �ٽ� ó������ ����
            ImageIndex = 0;
            return ImageIndex;
        }

        return ImageIndex;
    }
    
    public void R_SR_Image_Change(int _index)
    {
        if (UserInfo.Equip_Characters.Count < 2)
        {
            // R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
            UserInfo.Get_Square_Image(CharacterImage, _index);

            if (CharChange_animator.enabled == true)
            {
                CharChange_animator.enabled = false;
            }
        }
        else if (2 <= UserInfo.Equip_Characters.Count)
        {
            //R��ް� SR, SSR�� ���簢������ �� �̹����� �޶� �̿�
            UserInfo.Get_Square_Image(CharacterImage, _index);
        }
    }
}
