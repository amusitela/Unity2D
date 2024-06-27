using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickHandler : MonoBehaviour, IPointerClickHandler
{
    // ����������ڻ��屻���ʱ����
    public void OnPointerClick(PointerEventData eventData)
    {
        // ������Ϣ��ȷ�ϵ���¼�������
        Debug.Log("Canvas �����!");

        if (GameManager.instance != null)
        {
            GameManager.instance.EnterPlaying();
        }
        else
        {
            Debug.LogError("GameManager û�ҵ�.");
        }
    }
}
