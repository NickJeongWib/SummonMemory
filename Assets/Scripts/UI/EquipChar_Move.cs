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
        // �̸� �����ͼ� ���° ������ ĳ������ �˱�
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
        // Canvas�� Screen space - camera�� ���ֱ� ������
        // ��ǥ�踦 �ٲ���� �� �ʿ䰡 ����
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

        // Raycast �غ�
        PointerEventData pointerData = new PointerEventData(eventSystem);
        pointerData.position = eventData.position;

        // ����ĳ��Ʈ�� ���� ����Ʈ �߰�
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // Raycast�� ������ �ƹ��ŵ� ���ٸ�
        if(results.Count <= 0)
        {
            // ���� ��ġ�� ����
            this.transform.position = StartPos;
        }

        // ���� ������Ʈ�� �����ϰ� �� �ؿ� �ִ� ������Ʈ ã�ƺ���
        foreach (RaycastResult result in results)
        {
            // ���� Raycast�� ���� ������Ʈ�� �� ������Ʈ�� �ƴ϶� �ٸ� ������Ʈ��
            if (result.gameObject != this.gameObject)
            {
                if (result.gameObject.GetComponent<EquipChar_Move>() != null)
                {
                    // ��ġ �ٲ��ֱ�
                    this.transform.position = result.gameObject.transform.position;
                    // �ٲ��� ������Ʈ ��ġ�� ������ �巡���ߴ� ĳ���� ��ġ��
                    result.gameObject.transform.position = StartPos;

                }
                else
                {
                    // ���� ��ġ�� ����
                    this.transform.position = StartPos;
                }

                // �ٷ� �ؿ� �ִ� UI�� �ʿ��ϱ� ������ �ٷ� break
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

