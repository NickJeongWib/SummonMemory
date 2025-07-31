using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipChar_Move : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    Vector3 StartPos = Vector3.zero;

    [Header("PointerDown")]
    int Pos_Num;

    [Header("Drag_Var")]
    public RectTransform canvasRectTransform;

    [Header("PointerUp_Var")] 
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    #region Handler

    public void OnPointerDown(PointerEventData eventData)
    {
        // 이름 가져와서 몇번째 장착된 캐릭인지 알기
        string[] name = this.gameObject.name.Split("_");
        Pos_Num = int.Parse(name[2]);

        if(gameObject.transform.childCount <= 0)
        {
            return;
        }
        StartPos = gameObject.transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {

        if (gameObject.transform.childCount <= 0)
        {
            return;
        }
        // Canvas가 Screen space - camera로 되있기 때문에
        // 좌표계를 바꿔줘야 할 필요가 있음
        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            GetComponent<RectTransform>().localPosition = localPoint;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (gameObject.transform.childCount <= 0)
        {
            return;
        }

        // Raycast 준비
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = eventData.position;

        // 레이캐스트로 받은 리스트 추가
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // Raycast로 받은게 아무거도 없다면
        if(results.Count <= 0)
        {
            // 원래 위치로 복귀
            this.transform.position = StartPos;
        }

        // 현재 오브젝트는 제외하고 그 밑에 있는 오브젝트 찾아보기
        foreach (RaycastResult result in results)
        {
            // 만약 Raycast로 받은 오브젝트가 이 오브젝트가 아니라 다른 오브젝트면
            if (result.gameObject != this.gameObject)
            {
                if (result.gameObject.GetComponent<EquipChar_Move>() != null)
                {
                    // 위치 바꿔주기
                    this.transform.position = result.gameObject.transform.position;
                    // 바꿔준 오브젝트 위치는 기존에 드래그했던 캐릭터 위치로
                    result.gameObject.transform.position = StartPos;

                }
                else
                {
                    // 원래 위치로 복귀
                    this.transform.position = StartPos;
                }

                // 바로 밑에 있는 UI만 필요하기 때문에 바로 break
                break; 
            }
        }
    }
    #endregion

    #region Change_Position
    void Change_Char_Pos()
    {
        // UserInfo.Pos_Index[]
    }
    #endregion
}

