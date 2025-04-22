using UnityEngine;
using UnityEngine.UI;

public class Progresscell : RawImage {
    
    private Slider _Progresscell;

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();

        //获取血条
        if (_Progresscell == null)
            _Progresscell = transform.parent.parent.GetComponent<Slider>();

        //获取血条的值
        if (_Progresscell != null)
        {
            //刷新血条的显示
            float value = _Progresscell.value;
            uvRect = new Rect(0,0,value,1);
        }
    }
}