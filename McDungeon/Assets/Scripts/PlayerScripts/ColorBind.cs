/* Should include citation */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ColorBind : ScriptableObject
{
    [SerializeField]
    private Color color = new Color(0.412f, 0.824f, 0.906f); //aoi

    public Color Color()
    {
        return this.color;
    }
}
