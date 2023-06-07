using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    public GameObject speechBubblePrefab;

    void Start()
    {
        
    }

    public void GenerateTextBubble(Transform parent, string text, Vector3 dimensions, Vector3 offset, float fontSize, float duration)
    {
        GameObject newTextBubble = Instantiate(speechBubblePrefab, parent);
        
        newTextBubble.GetComponent<SpeechBubbleController>().Init(
            text: text,
            dimensions: dimensions, 
            offset: offset, 
            fontSize: fontSize, 
            duration: duration
        );
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
