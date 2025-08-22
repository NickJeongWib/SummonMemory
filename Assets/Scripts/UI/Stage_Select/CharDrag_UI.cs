using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharDrag_UI : MonoBehaviour
{
    [SerializeField] Image CantChange;

    public void Set_UI(bool _isOn)
    {
        CantChange.gameObject.SetActive(_isOn);
    }
}
