using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>
public class GoogleSheetSO : ScriptableObject
{
	public List<Character_DB> Character_DBList;
	public List<Character_Image_Address> Character_Image_AddressList;
}

[Serializable]
public class Character_DB
{
	public int CHAR_ID;
	public string CHAR_NAME;
	public string CHAR_ELEMENT;
	public string CHAR_TYPE;
	public string CHAR_GRADE;
	public int CHAR_STAR;
	public int CHAR_HP;
	public int CHAR_ATK;
	public int CHAR_DEF;
	public float CHAR_CRI_RATE;
	public float CHAR_CRI_DAMAGE;
}

[Serializable]
public class Character_Image_Address
{
	public string CHAR_ILLUST;
	public string CHAR_NORMAL_IMAGE;
	public string CHAR_GRADE_UP_IMAGE;
	public string CHAR_PROFILE_IMAGE;
}

