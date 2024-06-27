using UnityEngine;
using UnityEngine.EventSystems;

public class CanvasClickHandler : MonoBehaviour, IPointerClickHandler
{
    // 这个方法将在画板被点击时调用
    public void OnPointerClick(PointerEventData eventData)
    {
        // 调试信息，确认点击事件被触发
        Debug.Log("Canvas 被点击!");

        if (GameManager.instance != null)
        {
            GameManager.instance.EnterPlaying();
        }
        else
        {
            Debug.LogError("GameManager 没找到.");
        }
    }
}
