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

        // TODO ## 초기 테스트 값
        Debug.Log("Count : " + UserInfo.Equip_Characters.Count);
        
        if (UserInfo.Equip_Characters.Count == 0)
        {
            Debug.LogError("없음");
        }
        else
        {
            Debug.Log(UserInfo.Equip_Characters[0].Get_CharName);
        }
       
        R_SR_Image_Change(0);
    }

    public void CharImage_ChangeAnimF()
    {
        // 캐릭터 이미지 변경 스크립트 작동
        if (UserInfo.Equip_Characters.Count < 2)
        {
            CharacterImage.color = Color.white; // 알파값 x
            CharChange_animator.enabled = false;

            // R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
            R_SR_Image_Change(0);
        }
        else if (2 <= UserInfo.Equip_Characters.Count)
        {
            CharChange_animator.enabled = true;

            // R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
            R_SR_Image_Change(0);
        }
    }

    // 애니메이션 이벤트 함수
    public void CharImg_Change()
    {
        if (UserInfo.Equip_Characters[Get_NextIndex()].Get_CharGrade != Define.CHAR_GRADE.R)
        {
            UserInfo.Get_Square_Image(CharacterImage, ImageIndex);
            // 캐릭터 이미지 교체
            // CharacterImage.sprite = UserInfo.Equip_Characters[ImageIndex].Get_SquareIllust_Img;
        }
        else
        {
            UserInfo.Get_Square_Image(CharacterImage, ImageIndex);
            // 캐릭터 이미지 교체
            //CharacterImage.sprite = UserInfo.Equip_Characters[ImageIndex].Get_Illust_Img;
        }
    }

    int Get_NextIndex()
    {
        ImageIndex++;

        // 장착 캐릭터 수를 넘어간다면 
        if (ImageIndex >= UserInfo.Equip_Characters.Count)
        {
            // 다시 처음부터 시작
            ImageIndex = 0;
            return ImageIndex;
        }

        return ImageIndex;
    }
    
    public void R_SR_Image_Change(int _index)
    {
        if (UserInfo.Equip_Characters.Count < 2)
        {
            // R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
            UserInfo.Get_Square_Image(CharacterImage, _index);

            if (CharChange_animator.enabled == true)
            {
                CharChange_animator.enabled = false;
            }
        }
        else if (2 <= UserInfo.Equip_Characters.Count)
        {
            //R등급과 SR, SSR은 정사각형으로 된 이미지가 달라서 이용
            UserInfo.Get_Square_Image(CharacterImage, _index);
        }
    }
}
