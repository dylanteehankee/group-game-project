/* Should include citation */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorController : MonoBehaviour
{
    [SerializeField] private ColorBind colorBind;
    private ColorWatcher watcher;

    void Awake()
    {
        this.watcher = new ColorWatcher(colorBind.Color, ChangeColor);
    }

    private void ChangeColor(Color color)
    {
        this.GetComponent<SpriteRenderer>().color = color;
    }

    void Update()
    {
        this.watcher.Watch();
    }
}
