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

    // 상점, 도감, 업적 등 여러 창으로 이동
    public void On_Click_OnPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(true);
    }

    // 현재 열려 있는 창을 닫고 로비로 이동
    public void On_Click_OffPanel(GameObject _obj)
    {
        NotTouch_RayCast.SetActive(true); // 이동 중 버튼 클릭 방지
        ShaderTransition.SetActive(true);
        _obj.SetActive(false);
    }

    public void On_Click_Back(GameObject _obj)
    {
        _obj.SetActive(false);
    }
}
