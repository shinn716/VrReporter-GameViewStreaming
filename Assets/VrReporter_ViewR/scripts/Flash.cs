using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash : MonoBehaviour {

    [Range(0, 3)]
    public float speed = .5f;
    private Image m_img;
    private bool m_start = false;
    private float m_count;

    private void OnEnable()
    {
        m_count = 1;        
        m_img = GetComponent<Image>();
        m_start = true;
    }

    void FixedUpdate()
    {
        if (m_start)
        {
            if (m_count < .01f)
            {
                m_count = 0;
                m_start = false;
                gameObject.SetActive(false);
                m_img.color = new Color(1, 1, 1, m_count);
            }
            else
            {
                m_count -= speed * Time.fixedDeltaTime;
                m_img.color = new Color(1, 1, 1, m_count);
            }            
        }
    }
}
