
using UnityEngine;

/*
*Create By Keefor On 1/2/2018
*/

public class PipePop : TDButtonItem
{
    private GameObject obj;
    private TextMesh text;
    private Material mater,old;
    private Renderer render;
    protected override void Init()
    {
        base.Init();
        obj = transform.GetChild(0).gameObject;
        text = obj.GetComponentInChildren<TextMesh>();
//        mater = Resources.Load<Material>("flash");
//        render = GetComponent<MeshRenderer>();
//        old = render.material;
        //Hide();
    }

    public void SetContext(string context)
    {
        text.text = context;
    }

    private void Show()
    {
        obj.SetActive(true);
//        render.enabled = true;
//        render.material = mater;
    }

    private void Hide()
    {
        obj.SetActive(false);
//        render.material = old;
//        render.enabled = false;
    }

    public override void MouseHover()
    {
        base.MouseHover();
        Show();
    }

    public override void MouseExit()
    {
        base.MouseExit();
        Hide();
    }
}
