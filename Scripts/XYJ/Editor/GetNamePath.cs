using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetNamePath : EditorWindow
{
    public Transform parentTrans;//要显示的物体必须是public
    private SerializedObject serObj;
    private SerializedProperty showObj;

    private string path;
    private bool isRoot = false;
    
    [MenuItem("MenuCommand/GetNamePath", false)]
    public static void ShowWindow()
    {
        //显示现有窗口实例。如果没有，请创建一个。
        EditorWindow.GetWindow(typeof(GetNamePath));
    }

    private void OnEnable()
    {
        ////**********************自建window显示类变量****************************
        parentTrans = Selection.activeTransform;

        serObj = new SerializedObject(this);
        showObj = serObj.FindProperty("parentTrans");
    }

    void OnGUI()
    {

        EditorGUILayout.LabelField("选中最终父物体打开面板后，选中子物体，点击寻找按钮。返回父物体到子物体的路径");

        //**********************自建window显示类变量****************************
        EditorGUILayout.PropertyField(showObj, true);
        serObj.ApplyModifiedProperties();//应用更改

        EditorGUILayout.LabelField("是否需要最根源的父物体路径");
        isRoot = EditorGUILayout.Toggle(isRoot);

        if (GUILayout.Button("寻找"))
        {
            Find();
        }
        EditorGUILayout.TextField(path);
    }

    private void Find()
    {
        path = "";

        Transform childTrans = Selection.activeTransform;
        if (childTrans == null)
        {
            Debug.Log("未选中子物体");
            return;
        }

        Stack<string> nameStack = new Stack<string>();

        do
        {
            nameStack.Push(childTrans.name);

            if (childTrans == parentTrans)
            {
                int count = nameStack.Count;
                for (int i = 0; i < count; i++)
                    path += nameStack.Pop() + "/";

                path = path.Substring(0,path.Length - 1);

                if (isRoot == false)
                    path = path.Substring(path.IndexOf("/") + 1);

                return;
            }

            childTrans = childTrans.parent;
        } while (childTrans.parent != null);

        path = "未找到路径，请检查子物体是否在父物体下";
    }

}
