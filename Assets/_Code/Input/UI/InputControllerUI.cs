using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(RectTransform))]
public class InputControllerUI : MonoBehaviour {

    [Range(0,3)]
    public int ControllerNum;
    public InputAssignmentField inputInterfaceUIPrefab;
    private List<InputAssignmentField> inputAssignmentFields = new List<InputAssignmentField>();
    private readonly FieldInfo[] fieldInfos = typeof(InputController).GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Where(x => x.FieldType.IsSubclassOf(typeof(InputMap))).ToArray();

	// Use this for initialization
	void Start ()
    {
        if (inputInterfaceUIPrefab == null) { Debug.LogError("No InputInterfaceUIPrefab found, please assign."); return; }
        for (int i = 0; i < fieldInfos.Length; i++)
        {
            InputAssignmentField field = CreateInputInterface(fieldInfos[i]);
            RectTransform elementRectTransform = field.GetComponent<RectTransform>();
            Vector3 elementPosition = elementRectTransform.rect.height* Vector3.down * i*1.1f;
            elementRectTransform.position += elementPosition;
            RectTransform rectTransform = GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2( rectTransform.rect.width,  -elementPosition.y+ elementRectTransform.rect.height);
            rectTransform.position = Vector3.zero;
            inputAssignmentFields.Add(field);
        }
	}
    private void UpdateFields()
    {
        foreach(InputAssignmentField ia in inputAssignmentFields)
        {
            ia.controllerNum = ControllerNum;
            ia.UpdateUI();
        }
    }
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Alpha0)) { ControllerNum = 0; UpdateFields(); }
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ControllerNum = 1; UpdateFields(); }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { ControllerNum = 2; UpdateFields(); }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { ControllerNum = 3; UpdateFields(); }
    }

    private InputAssignmentField CreateInputInterface(FieldInfo fieldInfo)
    {
        InputAssignmentField inputAssignmentField = Instantiate<InputAssignmentField>(inputInterfaceUIPrefab, transform);
        inputAssignmentField.fieldInfoTarget = fieldInfo;
        inputAssignmentField.controllerNum = ControllerNum;
        inputAssignmentField.Initialize();
        return inputAssignmentField;
    }

    public void SaveSettings()
    {
        Input.SaveSettings();
    }

    public void ResetSettings()
    {
        Input.ResetAllSettings();
        UpdateFields();
    }
}
