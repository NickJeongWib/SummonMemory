using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CharInfo_CharSelect_Btn : MonoBehaviour
{
    public Character character;
    public Image Select_Img;
    public Image Character_Face;

    public void Test()
    {
        Debug.Log(character.Get_CharName);
    }
}
