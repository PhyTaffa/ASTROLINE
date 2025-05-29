using UnityEngine;
using UnityEngine.UI;

public class PageFlipper : MonoBehaviour {
    

    [SerializeField] GameObject[] pages;

    [SerializeField] Button[] arrowLeftButtons;
    [SerializeField] Button[] arrowRightButtons;

    int currentPage = 0;

    void Start()
    {
        ShowPage(0);
        
        foreach (var btn in arrowLeftButtons) {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(FlipLeft);
        }
        
        foreach (var btn in arrowRightButtons){
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(FlipRight);
        }
        
        UpdateNavButtons();
    }

    public void FlipRight()
    {
        if (currentPage < pages.Length - 1)
        {
            ShowPage(++currentPage);
            UpdateNavButtons();
        }
    }

    public void FlipLeft()
    {
        if (currentPage > 0)
        {
            ShowPage(--currentPage);
            UpdateNavButtons();
        }
    }

    void ShowPage(int index)
    {
        for (int i = 0; i < pages.Length; i++)
            pages[i].SetActive(i == index);
    }

    void UpdateNavButtons(){
        bool canGoLeft  = currentPage > 0;
        bool canGoRight = currentPage < pages.Length - 1;

        foreach (var btn in arrowLeftButtons){
            btn.interactable = canGoLeft;
        }

        foreach (var btn in arrowRightButtons){
            btn.interactable = canGoRight;
        }
    }
}