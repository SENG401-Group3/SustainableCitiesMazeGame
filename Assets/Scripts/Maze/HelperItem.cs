using UnityEngine;
using System.Collections.Generic;
using System;

public class HelperItem : Interactable
{
  public enum itemName {
    SpeedBoost = 0,
    Telescope,
    Teleport
  };

  [SerializeField]
  private List<Sprite> sprites;

  // c# does weird things with accessors so these attributes have to be public
  private itemName id;

  public PlayerItemController itemController {get; set;} = null;

  public void setId(int id){
    // set the id and sprite based on the item
    if(Enum.IsDefined(typeof(itemName), id))
      this.id = (itemName)id;
    else
      this.id = (itemName)0; 
      
    // TODO: unique sprite for each item
    int safeIndex = (int)this.id;
    if(sprites != null && safeIndex < sprites.Count && sprites[safeIndex] != null){
        GetComponent<SpriteRenderer>().sprite = sprites[safeIndex];
        if(id == (int)itemName.Teleport)
          GetComponent<Transform>().localScale = new Vector3(0.2f, 0.3f, 1f);
        else
          GetComponent<Transform>().localScale = new Vector3(0.4f, 0.4f, 1f);
    }else{
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 0, 255);
    }
  }

  public override void handleCollision(Collider2D collider){
      // register the collected item in the item controller
      itemController.collectItem(id);
    
      // create a text popup that says "item collected!"
      messageHandler.addMessage(id.ToString() + " collected!");

      Destroy(gameObject, 0f);
      return;
  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }
}
