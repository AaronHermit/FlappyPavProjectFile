using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectTracker : MonoBehaviour
{
    public ScrollRect scrollRect;
    
    public RectTransform contentPanel; 

    private RectTransform[] contentItems;

    public int contentValue;
    void Start()
    {
        contentItems = new RectTransform[contentPanel.childCount];
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            contentItems[i] = contentPanel.GetChild(i).GetComponent<RectTransform>();
        }

        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    void OnScroll(Vector2 position)
    {
        foreach (RectTransform item in contentItems)
        {
            if (IsVisible(item))
            {
                contentValue = Int32.Parse(item.name);
                Debug.Log( item.name);
                break;
            }
        }
    }

    bool IsVisible(RectTransform item)
    {
        Vector3[] corners = new Vector3[4];
        item.GetWorldCorners(corners);

        Rect rect = new Rect(scrollRect.viewport.position, scrollRect.viewport.rect.size);
        foreach (Vector3 corner in corners)
        {
            if (rect.Contains(corner))
            {
                return true;
            }
        }
        return false;
    }
}
