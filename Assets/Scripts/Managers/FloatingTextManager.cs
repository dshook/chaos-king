using UnityEngine;
using System;

public class FloatingTextManager : MonoBehaviour
{
    public static void Write(string text, Vector3 position, int fontSize, Color color)
    {
        var floatingTextPrefab = Resources.Load<GameObject>("UI/FloatingText");
        var go = Instantiate(floatingTextPrefab);

        go.transform.position = position;
        go.transform.rotation = Quaternion.identity;
        go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        var guiText = go.GetComponent<TextMesh>();
        guiText.text = text;
        guiText.fontSize = fontSize;
        guiText.color = color;
    }

    public static void EnemyDamage(int damage, Vector3 position)
    {
        int size = Math.Min(100, (int)(60 + (damage * 0.5)));
        Write(damage.ToString(), position, size, new Color(0.98f, 0.41f, 0.01f));
    }

    public static void PlayerDamage(int damage, Vector3 position)
    {
        Write(damage.ToString(), position, 90, new Color(0.98f, 0.01f, 0.01f));
    }

    public static void PlayerXp(int amount, Vector3 position)
    {
        Write(string.Format("+{0} xp",amount), position, 50, new Color(0.01f, 0.91f, 0.01f));
    }
}
