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

    // ���õ� �κ��丮
    public enum INVENTORY_TYPE
    {
        SPEND,
        EQUIPMENT,
        UPGRADE,
        CURRENCY,
    }

    // ������ Ÿ��
    public enum ITEM_TYPE
    {
        NONE,
        SPEND,
        EQUIPMENT,
    }

    // ��� ������ Ÿ��
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

    // ĳ���� Ÿ��
    public enum CHAR_TYPE
    {
        ATTACK,
        MAGE,
        DEFEND,
        SUPPORT,
    }

    // ĳ���� �Ӽ�
    public enum CHAR_ELE
    {
        FIRE,
        WATER,
        WIND,
        GROUND,
    }

    // ĳ���� ���
    public enum CHAR_GRADE
    {
        R,
        SR,
        SSR,
    }

    public enum PACKETTYPE
    {
        CHARLIST,
        CLEAR_CHAR_LIST,
        EQUIP_CHAR_LIST,
        CLEAR_EQUIP_CHAR,
    }
}
