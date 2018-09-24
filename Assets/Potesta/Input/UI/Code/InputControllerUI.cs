using System.Collections.Generic;
using UnityEngine;

namespace Potesta.FlexInput
{
    [RequireComponent(typeof(RectTransform))]
    public class InputControllerUI : MonoBehaviour
    {
        public int ControllerNum { get { return controllerNum; } set { controllerNum = value; UpdateFields(); } }
        private int controllerNum;
        public InputAssignmentField inputInterfaceUIPrefab;
        private List<InputAssignmentField> inputAssignmentFields = new List<InputAssignmentField>();
        public InputController Controller { get { return Input.Num[ControllerNum]; } }

        void Start()
        {
            if (inputInterfaceUIPrefab == null) { Debug.LogError("No InputInterfaceUIPrefab found, please assign."); return; }
            for (int i = 0; i < Input.Num[ControllerNum].InputMaps.Length; i++)
            {
                InputAssignmentField field = CreateInputInterface(Controller.InputMaps[i]);
                RectTransform elementRectTransform = field.GetComponent<RectTransform>();
                Vector3 elementPosition = elementRectTransform.rect.height * Vector3.down * i * 1.1f;
                elementRectTransform.position += elementPosition;
                RectTransform rectTransform = GetComponent<RectTransform>();
                rectTransform.sizeDelta = new Vector2(rectTransform.rect.width, -elementPosition.y + elementRectTransform.rect.height);
                rectTransform.position = Vector3.zero;
                inputAssignmentFields.Add(field);
            }
            UpdateFields();
        }

        private void UpdateFields()
        {
            for (int i = 0; i < Input.Num[ControllerNum].InputMaps.Length; i++)
            {
                inputAssignmentFields[i].fieldInfoTarget = Controller.InputMaps[i];
                inputAssignmentFields[i].UpdateUI();
            }
        }
        public void Update()
        {

            if (Input.GetKeyDown(KeyCode.Alpha0)) { ControllerNum = 0; }
            if (Input.GetKeyDown(KeyCode.Alpha1)) { ControllerNum = 1; }
            if (Input.GetKeyDown(KeyCode.Alpha2)) { ControllerNum = 2; }
            if (Input.GetKeyDown(KeyCode.Alpha3)) { ControllerNum = 3; }
        }

        private InputAssignmentField CreateInputInterface(InputMap fieldInfo)
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
}