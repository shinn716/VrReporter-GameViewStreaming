using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShinnViewR : MonoBehaviour
{
    public Transform TransformAttachToObjs;
    public Canvas canvasviewr;
    private bool m_flag = false;

    private void Update ()
    {
        GameObject _ViewrFinder = GameObject.Find("Clients");
        if (_ViewrFinder != null)
        {
            _ViewrFinder.transform.position = TransformAttachToObjs.position;
            _ViewrFinder.transform.localRotation = TransformAttachToObjs.localRotation;

            try
            {
                GameObject getchild = _ViewrFinder.transform.GetChild(0).gameObject;
                if (getchild != null && !m_flag)
                {
                    print("catch obj:" + getchild.name);
                    m_flag = true;
                    
                    StartCoroutine(InitTransform(getchild));
                }
            }

            catch (Exception e) { }

        }
    }

    private IEnumerator InitTransform(GameObject objs)
    {
        yield return new WaitForSeconds(2);
        objs.transform.localPosition = new Vector3(-.03f, 0, .1f);
        objs.transform.localEulerAngles = Vector3.forward * 90;

        var cam = GameObject.FindObjectsOfType<Camera>();
        //print("0 " + cam[0].gameObject.name);
        //print("1 " + cam[1].gameObject.name);
        canvasviewr.worldCamera = cam[1];
    }
}
