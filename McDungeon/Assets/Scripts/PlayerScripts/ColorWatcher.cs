/* Should include citation */

using System;
using UnityEngine;

public class ColorWatcher : MonoBehaviour
{
    private Color value;
    private Color polledValue;
    private Func<Color> getColorValue;
    private Action<Color> callBack;

    public ColorWatcher(Func<Color> getColorValue, Action<Color> callBack)
    {
        this.getColorValue = getColorValue;
        this.callBack = callBack;
        Watch();
    }

    public void Watch()
    {

        polledValue = this.getColorValue();
        if (polledValue != value)
        {
            this.callBack(polledValue);
            value = polledValue;
        }
    }
}
