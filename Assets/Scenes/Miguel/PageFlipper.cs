using UnityEngine;

public class PageFlipper : MonoBehaviour
{
    [SerializeField] private GameObject[] pages;
    private int currentPage = 0;

    private void Start() {
        ShowPage(currentPage);
    }

    public void FlipRight() {
        if (currentPage < pages.Length - 1){
            
            pages[currentPage].SetActive(false);
            currentPage++;
            pages[currentPage].SetActive(true);
        }
    }

    public void FlipLeft(){
        if (currentPage > 0)
        {
            pages[currentPage].SetActive(false);
            currentPage--;
            pages[currentPage].SetActive(true);
        }
    }

    private void ShowPage(int index){
        
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(i == index);
        }
    }
}