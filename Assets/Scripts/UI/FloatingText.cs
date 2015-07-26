using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FloatingText : MonoBehaviour
{
    public float timeToFade = 2f;
    public Color startColor;

    TextMesh floatingText;
    Color transparentColor = new Color(0,0,0,0);

    // Use this for initialization
    void Start()
    {
        floatingText = GetComponent<TextMesh>();
        startColor = floatingText.color;
        Destroy(gameObject, timeToFade + 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, 1f * Time.deltaTime, 0));
        //floatingText.color = Color.Lerp(startColor, transparentColor, timeToFade);
        Color tempcolor = floatingText.color;
        tempcolor.a = Mathf.MoveTowards(tempcolor.a, 0, Time.deltaTime);
        floatingText.color = tempcolor;

    }
}
