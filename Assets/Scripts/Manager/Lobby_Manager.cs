using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lobby_Manager : MonoBehaviour
{
    [SerializeField] GameObject ShaderTransition;
    [SerializeField] GameObject NotTouch_RayCast;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // ����, ����, ���� �� ���� â���� �̵�
    public void On_Click_OnPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // �̵� �� ��ư Ŭ�� ����
        ShaderTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // ���� ���� �ִ� â�� �ݰ� �κ�� �̵�
    public void On_Click_OffPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // �̵� �� ��ư Ŭ�� ����
        ShaderTransition.SetActive(true);
        _obj.SetActive(false);
    }

    public void On_Click_Back(GameObject _obj)
    {
        _obj.SetActive(false);
    }
}
