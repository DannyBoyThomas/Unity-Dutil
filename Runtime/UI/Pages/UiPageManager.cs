using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
using System.Linq;
namespace Dutil
{
    public class UiPageManager : MonoBehaviour
    {
        public UiPage startingPage;
        public List<UiPage> pageQueue = new List<UiPage>();
        public List<UiPage> pages = new List<UiPage>();
        void Start()
        {
            Focus(startingPage);
        }
#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void AddPage()
        {
            int index = Pages.Count + 1;
            GameObject obj = new GameObject("Page " + index);
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.SetSiblingIndex(0);
            RectTransform rt = obj.AddComponent<RectTransform>();
            //fill parent
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            Image img = obj.AddComponent<Image>();
            img.color = Color.white.WithAlpha(0);
            img.sprite = null;

            UiPage page = obj.AddComponent<UiPage>();
            Focus(page);
        }
        List<UiPage> Pages
        {
            get
            {
                pages = GetComponentsInChildren<UiPage>(true).ToList();
                //   pages.AddUniqueAll(transform.GetComponentsInChildren<UiPage>());
                return pages;
            }
        }
        public void Focus(int index)
        {
            if (index < Pages.Count)
            {
                for (int i = 0; i < Pages.Count; i++)
                {
                    Pages[i].gameObject.SetActive(i == index);
                }
            }
        }
        public void Focus(UiPage page)
        {
            Focus(Pages.IndexOf(page));
        }
        public void Navigate(UiPage page)
        {
            if (page == null) { return; }
            UiPage current = Pages.FirstOrDefault(x => x.gameObject.activeSelf);
            pageQueue.Add(current);
            pageQueue.Clean();
            Focus(page);
        }

#if ODIN_INSPECTOR
        [Sirenix.OdinInspector.Button]
#endif
        public void Back()
        {
            if (pageQueue.Count > 0)
            {
                Focus(pageQueue.PopLast());
            }
        }

    }
}