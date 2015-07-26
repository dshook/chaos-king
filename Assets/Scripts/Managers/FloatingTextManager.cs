using UnityEngine;
using UnityEngine.UI;

public class FloatingTextManager : MonoBehaviour
{
    public static void Write(string text, Vector3 position)
    {
        var floatingTextPrefab = Resources.Load<GameObject>("UI/FloatingText");
        var go = Instantiate(floatingTextPrefab);

        go.transform.position = position;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        var guiText = go.GetComponent<TextMesh>();
        guiText.text = text;
        guiText.fontSize = 90;
        guiText.color = new Color(1, 1, 0);

    }
}
