using UnityEngine;
using System.Collections.Generic;
using Patterns;

public class PlayerItemController : MonoBehaviour, Subject
{
  List<Observer> observers = new List<Observer>();

  // unless we want item persistence, these lists do not matter
  // private List<int> collectedItems;  
  // private List<int> usedItems;  

  // this one does
  private List<HelperItem.itemName> heldItems;  

  public bool hasItem(HelperItem.itemName id){
    return heldItems.Contains(id);
  }

  public void collectItem(HelperItem.itemName id){
    heldItems.Add(id);
    notifyObservers();
  }

  public void useItem(HelperItem.itemName id){
    heldItems.Remove(id);
  }

  public void addObserver(Observer o){
    if(!observers.Contains(o))
      observers.Add(o);
  }

  public void removeObserver(Observer o){
    observers.Remove(o);
  }

  public void notifyObservers(){
    foreach(Observer o in observers)
    {
      o.notify();
    }
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    heldItems = new List<HelperItem.itemName>();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
