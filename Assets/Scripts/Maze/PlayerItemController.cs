using UnityEngine;
using System.Collections.Generic;

public class PlayerItemController : MonoBehaviour
{
    // unless we want item persistence, these lists do not matter
    private List<int> collectedItems;  
    private List<int> usedItems;  

    // this one does
    private List<int> heldItems;  

    public bool hasItem(int id){
      return heldItems.Contains(id);
    }

    public void collectItem(int id){
      heldItems.Add(id);
    }

    public void useItem(int id){
      heldItems.Remove(id);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      heldItems = new List<int>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
