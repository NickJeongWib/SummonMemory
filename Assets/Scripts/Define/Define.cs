public static class Define
{
    public enum ITEM_TYPE
    {
        NONE,
        SPEND,
        EQUIPMENT,
    }

    public enum EQUIP_TYPE
    {
        NONE,
        WEAPON,
        HELMET,
        UPPER,
        ACCESSORY,
        GLOVE,
    }

    public enum EQUIPMENT_GRADE
    {
        NONE,
        N,
        R,
        U,
        L
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
}
