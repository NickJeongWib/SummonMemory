using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Info_Panel : MonoBehaviour
{
    [SerializeField] Text Info_Text;

    // �ؽ�Ʈ �Է�
   public void Set_Text(string _text)
   {
       Info_Text.text = _text;
   }
}
