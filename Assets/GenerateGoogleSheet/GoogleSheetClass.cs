using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>
public class GoogleSheetSO : ScriptableObject
{
	public List<Character_DB> Character_DBList;
	public List<Character_Image_Address> Character_Image_AddressList;
	public List<Character_Growing_State> Character_Growing_StateList;
	public List<Item_DB> Item_DBList;
}

[Serializable]
public class Character_DB
{
	public int CHAR_ID;
	public string CHAR_NAME;
	public string CHAR_ENG_NAME;
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
	public string CHAR_WHITE_IMAGE;
	public string CHAR_ILLUST_SQUARE;
}

[Serializable]
public class Character_Growing_State
{
	public float LINEAR_FACTOR;
	public float EXP_FACTOR;
	public float EXP_MULTIPLIER;
	public int TRANSITION_LEVEL;
}

[Serializable]
public class Item_DB
{
	public int ITEM_ID;
	public string ITEM_NAME;
	public string ITEM_TYPE;
	public string EQUIP_TYPE;
	public int ITEM_ATK;
	public int ITEM_DEF;
	public float ITEM_CRI_RATE;
	public float ITEM_CRI_DAMAGE;
	public int ITEM_HP;
	public int ITEM_VALUE_MIN_RANGE;
	public float ITEM_VALUE_MAX_RANGE;
	public string ITEM_IMAGE_ADDRESS;
}

