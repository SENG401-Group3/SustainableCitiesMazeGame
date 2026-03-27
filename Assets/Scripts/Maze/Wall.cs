using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
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

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
  }
}
