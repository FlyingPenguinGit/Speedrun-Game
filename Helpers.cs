using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers
{
    private static PointerEventData eventDataCurrentPos;
    private static List<RaycastResult> results;

    public static bool IsOverUI(Vector3 pos)
    {
        eventDataCurrentPos = new PointerEventData(EventSystem.current) {position = pos};
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPos, results);
        return results.Count > 0;
    }
}
