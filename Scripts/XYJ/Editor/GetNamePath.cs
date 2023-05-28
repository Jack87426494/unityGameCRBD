using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetNamePath : EditorWindow
{
    public Transform parentTrans;//Ҫ��ʾ�����������public
    private SerializedObject serObj;
    private SerializedProperty showObj;

    private string path;
    private bool isRoot = false;
    
    [MenuItem("MenuCommand/GetNamePath", false)]
    public static void ShowWindow()
    {
        //��ʾ���д���ʵ�������û�У��봴��һ����
        EditorWindow.GetWindow(typeof(GetNamePath));
    }

    private void OnEnable()
    {
        ////**********************�Խ�window��ʾ�����****************************
        parentTrans = Selection.activeTransform;

        serObj = new SerializedObject(this);
        showObj = serObj.FindProperty("parentTrans");
    }

    void OnGUI()
    {

        EditorGUILayout.LabelField("ѡ�����ո����������ѡ�������壬���Ѱ�Ұ�ť�����ظ����嵽�������·��");

        //**********************�Խ�window��ʾ�����****************************
        EditorGUILayout.PropertyField(showObj, true);
        serObj.ApplyModifiedProperties();//Ӧ�ø���

        EditorGUILayout.LabelField("�Ƿ���Ҫ���Դ�ĸ�����·��");
        isRoot = EditorGUILayout.Toggle(isRoot);

        if (GUILayout.Button("Ѱ��"))
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
            Debug.Log("δѡ��������");
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

        path = "δ�ҵ�·���������������Ƿ��ڸ�������";
    }

}
