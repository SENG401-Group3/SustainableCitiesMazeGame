using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
  // makes referencing corners easier
  public enum Corners {
    LT,
    LB,
    RT,
    RB
  }

  // corners we may want to remove
  [SerializeField]
  GameObject CornerLT;
  [SerializeField]
  GameObject CornerLB;
  [SerializeField]
  GameObject CornerRT;
  [SerializeField]
  GameObject CornerRB;

  Dictionary<Corners, GameObject> corners = new Dictionary<Corners, GameObject>();

  public Vector2 GetRoomSize(){
    SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();

    Vector3 minBounds = Vector3.positiveInfinity;
    Vector3 maxBounds = Vector3.negativeInfinity;

    foreach(SpriteRenderer ren in spriteRenderers){
      minBounds = Vector3.Min(minBounds, ren.bounds.min);
      maxBounds = Vector3.Max(maxBounds, ren.bounds.max);
    }

    return new Vector2(maxBounds.x - minBounds.x, maxBounds.y - minBounds.y);
  }

  public void updateCorners(){

  }

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    corners[Corners.LT] = CornerLT; 
    corners[Corners.LB] = CornerLB; 
    corners[Corners.RT] = CornerRT; 
    corners[Corners.RB] = CornerRB; 
  }
}
