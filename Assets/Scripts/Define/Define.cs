public static class Define
{
    public enum QUEST_REWARD_TYPE
    {
        DIA,
        GOLD,
    }

    public enum STORE_TYPE
    {
        CURRENCY,
        TICKET,
        R_BOOK,
        SR_BOOK,
        SSR_BOOK
    }

    public enum CONSUME_TYPE
    {
        DIA,
        MONEY,
        R_BOOK,
        SR_BOOK,
        SSR_BOOK,
    }

    // 선택된 인벤토리
    public enum INVENTORY_TYPE
    {
        SPEND,
        EQUIPMENT,
        UPGRADE,
        CURRENCY,
    }

    // 아이템 타입
    public enum ITEM_TYPE
    {
        NONE,
        SPEND,
        EQUIPMENT,
    }

    // 장비 아이템 타입
    public enum EQUIP_TYPE
    {
        WEAPON,
        HELMET,
        UPPER,
        ACCESSORY,
        GLOVE,
        NONE,
    }

    public enum EQUIPMENT_OPTION_GRADE
    {
        C,
        B,
        A,
        S,
        NONE,
    }

    public enum EQUIPMENT_GRADE
    {
        C,
        B,
        A,
        S,
        NONE,
    }

    public enum EQUIPMENT_OPTION
    {
        ATK_INT,
        ATK_PERCENT,
        DEF_INT,
        DEF_PERCENT,
        HP_INT,
        HP_PERCENT,
        CRIR_PERCENT,
        CRID_PERCENT,
        NONE,
    }

    // 캐릭터 타입
    public enum CHAR_TYPE
    {
        ATTACK,
        MAGE,
        DEFEND,
        SUPPORT,
    }

    // 캐릭터 속성
    public enum CHAR_ELE
    {
        FIRE,
        WATER,
        WIND,
        GROUND,
    }

    // 캐릭터 등급
    public enum CHAR_GRADE
    {
        R,
        SR,
        SSR,
    }

    // 데이터 저장 타입
    public enum PACKETTYPE
    {
        CHARLIST,
        EQUIP_CHAR_LIST,
        CLEAR_EQUIP_CHAR,
        ITEM_INVENTORY,
        EQUIP_ITEM_INVENTORY,
        DIA,
        MONEY,
        PROFILE_IMG,
    }

    #region Skill
    public enum BUFF_TYPE
    {
        NONE,       // 버프 스킬 X
        HILL,       // 힐
        SP_HILL,    // 스킬포인트 증가
        DEF,        // 방어력 증가
        DEF_TARGET, // 도발 + 방어력 증가
        ATK,        // 공격력 증가
        ALL_BUFF    // 힐 + 공격력 증가
    }

    public enum DEBUFF_TYPE
    {
        NONE,
        STUNNED,    // 기절
        DEF,        // 방어력 디버프
    }

    public enum SKILL_TYPE
    {
        ATTACK,
        BUFF,
    }
    #endregion
}
