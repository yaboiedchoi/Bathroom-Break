using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject 
{
    public float playerWalkSpeed;
    public float playerRunSpeed;
    public float jumpForce;
    public float moveSmoothTime;
    public Vector2 sensitivity;
    public float punchForce;
    public float punchRange;
    public float interactDistance;
    public float clickTimer;
    public Sprite HoverSprite;
    public Sprite ClickSprite;
    public Sprite CursorSprite;
}
