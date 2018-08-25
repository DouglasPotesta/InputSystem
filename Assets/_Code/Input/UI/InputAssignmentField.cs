using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class InputAssignmentField : MonoBehaviour {
    
    public Text InputName;
    public Text InputValue;
    public int controllerNum;
    public FieldInfo fieldInfoTarget;

    public void Initialize()
    {
        UpdateUI();
        Input.OnControllerConnected += ControllerChange;
        Input.OnControllerDisconnected += ControllerChange;
    }
    public void OnEnable()
    {
        if(fieldInfoTarget != null)
        {
            Input.OnControllerConnected += ControllerChange;
            Input.OnControllerDisconnected += ControllerChange;
        }
    }
    public void OnDisable()
    {
        Input.OnControllerConnected -= ControllerChange;
        Input.OnControllerDisconnected -= ControllerChange;
    }
    private void ControllerChange(int x) { UpdateUI(); }
    public void UpdateUI()
    {
        InputName.text = fieldInfoTarget.Name;
        InputValue.text = fieldInfoTarget.GetValue(Input.Num[controllerNum]).ToString();
    }

    public void ReassignButton()
    {
        StartCoroutine(MethodInvokeCast());
    }

    public IEnumerator MethodInvokeCast()
    {
        StartCoroutine(((InputMap) fieldInfoTarget.GetValue(Input.Num[controllerNum])).TestForInput());
        float timeStarted = Time.time;
        yield return new WaitWhile(() => 
        {
            UpdateUI();
            return Input.IsTestingForInput;
        });
        
    }

}
