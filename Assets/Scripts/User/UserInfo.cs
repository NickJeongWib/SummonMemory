using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    // string ĳ���� �̸� / Character ĳ���� ������
    public static Dictionary<string, Character> UserCharDict = new Dictionary<string, Character>();

    [Header("---Currency---")]
    public static int Money;
    public static int Dia;
    public static int EnterHealth;

    public static int R_Book;
    public static int SR_Book;
    public static int SSR_Book;
}
