using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UIControls
{
    public class SwapLevelMenu : MonoBehaviour
    {

        public GameObject scrollBar;
        float scrollPos = 0.0f;
        float[] pos;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            pos = new float[transform.childCount];
            float dist = 1.0f / (pos.Length - 1.0f);
            for (int i = 0; i < pos.Length; ++i)
            {
                pos[i] = dist * i;
            }
            if (Input.GetMouseButton(0))
            {
                scrollPos = scrollBar.GetComponent<Scrollbar>().value;
            }
            else
            {
                for (int i = 0; i < pos.Length; ++i)
                {
                    if (scrollPos < pos[i] + (dist / 2) && scrollPos > pos[i] - (dist / 2))
                    {
                        scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                    }
                }
            }

            for (int i = 0; i < pos.Length; ++i)
            {
                if (scrollPos < pos[i] + (dist / 2) && scrollPos > pos[i] - (dist / 2))
                {
                    transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.0f, 1.0f), 0.1f);
                    for (int a = 0; a < pos.Length; ++a)
                    {
                        if (a != i)
                        {
                            transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        }
                    }
                }
            }
        }
    }
}