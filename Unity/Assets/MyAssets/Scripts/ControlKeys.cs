using System;
using UnityEngine;

[Serializable]
public class ControlKeys
{
    [SerializeField]
    private KeyCode moveUp,
        moveUp2,
        moveDown,
        moveDown2,
        moveRight,
        moveRight2,
        moveLeft,
        moveLeft2,
        jump,
        run,
        crawl,
        action,
        cancel;

    public KeyCode MoveUp => moveUp;
    public KeyCode MoveUp2 => moveUp2;
    public KeyCode MoveDown => moveDown;
    public KeyCode MoveDown2 => moveDown2;
    public KeyCode MoveRight => moveRight;
    public KeyCode MoveRight2 => moveRight2;
    public KeyCode MoveLeft => moveLeft;
    public KeyCode MoveLeft2 => moveLeft2;
    public KeyCode Jump => jump;
    public KeyCode Run => run;
    public KeyCode Crawl => crawl;
    public KeyCode Action => action;
    public KeyCode Cancel => cancel;
}
