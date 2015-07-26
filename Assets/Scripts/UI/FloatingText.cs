using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float timeToFade = 1f;

    TextMesh floatingText;

    void Start()
    {
        floatingText = GetComponent<TextMesh>();
        Destroy(gameObject, timeToFade + 0.1f);
    }

    void Update()
    {
        transform.Translate(new Vector3(0, 1f * Time.deltaTime, 0));
        Color tempcolor = floatingText.color;
        //TODO: reinvestigate color.Lerp, or just use animator
        tempcolor.a = Mathf.MoveTowards(tempcolor.a, 0, Time.deltaTime * (1 / timeToFade));
        floatingText.color = tempcolor;
    }
}
