using UnityEngine;
using System.Collections.Generic;
using Patterns;
using UnityEngine.UI;

public class PlayerItemController : MonoBehaviour, Subject
{
  private List<Observer> observers = new List<Observer>();

  [SerializeField]
  private Button telescopeButton;
  [SerializeField]
  private Button teleporterButton;

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

    // handle telescope and teleporter
    if(id == HelperItem.itemName.Teleport){
      teleporterButton.interactable = true;
    }else if(id == HelperItem.itemName.Telescope){
      telescopeButton.interactable = true;
    }

    notifyObservers();
  }

  public void useItem(HelperItem.itemName id){
    heldItems.Remove(id);

    if(id == HelperItem.itemName.Teleport &&
        !heldItems.Contains(HelperItem.itemName.Teleport)){
      teleporterButton.interactable = false;
    }else if(id == HelperItem.itemName.Telescope &&
        !heldItems.Contains(HelperItem.itemName.Telescope)){
      telescopeButton.interactable = false;
    }
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

  public void onTelescopeClick(){
    useItem(HelperItem.itemName.Telescope);
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    heldItems = new List<HelperItem.itemName>();
    teleporterButton.interactable = false;
    telescopeButton.interactable = false;
  }

  // Update is called once per frame
  void Update()
  {

  }
}
