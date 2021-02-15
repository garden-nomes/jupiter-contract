using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct InputScheme
{
    public KeyCode up;
    public KeyCode down;
    public KeyCode left;
    public KeyCode right;

    public KeyCode btn0;
    public KeyCode btn1;
    public KeyCode btn2;
}

public class PlayerInput : MonoBehaviour
{
    public InputScheme inputScheme;

    public float horizontal
    {
        get
        {
            float axis = 0;
            if (Input.GetKey(inputScheme.left)) axis--;
            if (Input.GetKey(inputScheme.right)) axis++;
            return axis;
        }
    }

    public float vertical
    {
        get
        {
            float axis = 0;
            if (Input.GetKey(inputScheme.down)) axis--;
            if (Input.GetKey(inputScheme.up)) axis++;
            return axis;
        }
    }

    public string GetInstructionText()
    {
        var isOverLadder = GetComponent<MovementController>().IsOverLadder;
        var isGrounded = GetComponent<MovementController>().IsGrounded;

        return (isGrounded || isOverLadder ? $"{Icons.HorizontalAxis(inputScheme)} move\n" : "") +
            (isOverLadder ? $"{Icons.VerticalAxis(inputScheme)} climb ladder\n" : "") +
            $"{Icons.IconText(inputScheme.btn0)} toggle mag boots";
    }

    public bool GetBtn(int button) => Input.GetKey(GetButtonKeyCode(button));
    public bool GetBtnDown(int button) => Input.GetKeyDown(GetButtonKeyCode(button));
    public bool GetBtnUp(int button) => Input.GetKeyUp(GetButtonKeyCode(button));

    private KeyCode GetButtonKeyCode(int btn)
    {
        switch (btn)
        {
            case 0:
                return inputScheme.btn0;
            case 1:
                return inputScheme.btn1;
            case 2:
                return inputScheme.btn2;
            default:
                throw new System.ArgumentException("Invalid button number");
        }
    }
}
