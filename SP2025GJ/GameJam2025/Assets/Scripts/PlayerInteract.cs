using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using UnityEngine.UI;

// Controls the Crosshair on Gameplay UI

public enum CursorStatus
{
    Hovering,
    Clicked,
    None
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Camera playerCam;
    [SerializeField] private LayerMask interactable;

    [SerializeField] private Image cursor;

    private float clickTimer;

    public CursorStatus CursorStatus {  get; private set; }

    private void Update()
    {
        clickTimer -= Time.deltaTime;

        switch (CursorStatus)
        {
            case CursorStatus.Hovering:
                cursor.sprite = stats.HoverSprite;
                break;

            case CursorStatus.Clicked:
                cursor.sprite = stats.ClickSprite;
                break;

            case CursorStatus.None:
                cursor.sprite = stats.CursorSprite;
                break;
        }
    }

    bool Click()
    {
        if (clickTimer > 0)
            return false;

        clickTimer = stats.clickTimer;
        return true;
    }

    public void SetState(CursorStatus status)
    {
        if (clickTimer > 0)
            return;

        switch (CursorStatus)
        {
            case CursorStatus.Clicked:
                if (Click())    
                    CursorStatus = status;
                break;

            default:
                CursorStatus = status;
                break;
        }
    }
}
