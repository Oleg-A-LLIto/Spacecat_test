using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITextRenderingBehaviour : MonoBehaviour
{
    [SerializeField] Text[] field;
    [SerializeField] string[] fieldName;
    public string[] fieldValue;

    void Start()
    {
        Debug.Assert(field.Length == fieldName.Length && fieldName.Length == fieldValue.Length, "Each UI text field should have its value and name!");
    }

    void LateUpdate()
    {
        for (int i = 0; i < field.Length; i++)
        {
            field[i].text = fieldName[i] + ": " + fieldValue[i];
        }   
    }
}
