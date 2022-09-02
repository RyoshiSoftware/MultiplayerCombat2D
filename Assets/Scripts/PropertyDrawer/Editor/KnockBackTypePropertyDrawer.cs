using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;


[CustomPropertyDrawer(typeof(KnockBackTypeAttribute))]
public class KnockBackTypePropertyDrawer: PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label) * 2;
    }
    
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

        EditorGUI.BeginProperty(position, label, property);

        if (property.propertyType == SerializedPropertyType.Float)
        {
            EditorGUI.BeginChangeCheck();

            var newValue = EditorGUI.FloatField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.floatValue);

            EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Knock Back Type", HelperMethods.GetKnockBackType(newValue).ToString(), EditorStyles.boldLabel);

            if (EditorGUI.EndChangeCheck())
            {
                property.floatValue = newValue;
            }
        }
        
        EditorGUI.EndProperty();
    }

}

