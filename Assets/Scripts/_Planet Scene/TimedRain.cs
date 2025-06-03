using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedRain : MonoBehaviour{
  
    [Header("Duration")]
    [SerializeField] private float onDuration  = 40f;
    [Header("How long to stay inactive")]
    [SerializeField] private float offDuration = 60f;
    
    private List<GameObject> allChildren = new List<GameObject>();

    private void Awake(){
        
        FetchAllChildren(transform);
    }

    private void OnEnable() {
        StartCoroutine(LoopToggle());
    }

    private void OnDisable(){ 
        StopAllCoroutines();
    }

   
    private void FetchAllChildren(Transform parent){
        
        for (int i = 0; i < parent.childCount; i++){
            Transform child = parent.GetChild(i);
            allChildren.Add(child.gameObject);
            FetchAllChildren(child);
        }
    }

    private IEnumerator LoopToggle() {
      
        SetChildrenActive(false);
        yield return new WaitForSeconds(offDuration);

        while (true){
            
            SetChildrenActive(true);
            yield return new WaitForSeconds(onDuration);
            
            SetChildrenActive(false);
            yield return new WaitForSeconds(offDuration);
        }
    }
    
    private void SetChildrenActive(bool active){
        for (int i = 0; i < allChildren.Count; i++)
        {
            // early‐out if something was destroyed or missing
            if (allChildren[i] != null)
                allChildren[i].SetActive(active);
        }
    }
}
