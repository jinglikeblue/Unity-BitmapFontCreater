using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ZeroEditor
{
    /// <summary>
    /// 位图字创建GUI界面
    /// </summary>
    public class BitmapFontCreateEditorWindow : EditorWindow
    {
        /// <summary>
        /// 打开
        /// </summary>
        public static BitmapFontCreateEditorWindow Open()
        {
            return GetWindow<BitmapFontCreateEditorWindow>("Bitmap Font Creater");
        }

        [SerializeField]
        public List<Texture2D> textures = new List<Texture2D>();

        public string charContent;

        public string outputPath = "Assets/";
        public string fontName;

        //序列化对象
        SerializedObject _serializedObject;

        //序列化属性
        SerializedProperty _assetLstProperty;

        Vector2 scroll;

        private void OnEnable()
        {
            //使用当前类初始化
            _serializedObject = new SerializedObject(this);
            //获取当前类中可序列话的属性
            _assetLstProperty = _serializedObject.FindProperty("textures");
        }

        private void OnGUI()
        {
            _serializedObject.Update();

            GUILayout.BeginVertical();

            EditorGUILayout.Space();
            GUILayout.Label("字体图片：");

            scroll = EditorGUILayout.BeginScrollView(scroll);

            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(_assetLstProperty, true);
            if (EditorGUI.EndChangeCheck())
            {//提交修改
                _serializedObject.ApplyModifiedProperties();
            }

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            GUILayout.Label("字体名称：");
            fontName = EditorGUILayout.TextField(fontName);

            EditorGUILayout.Space();
            GUILayout.Label("字符内容：");
            charContent = EditorGUILayout.TextArea(charContent, GUILayout.Height(50));

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button(string.IsNullOrEmpty(outputPath) ? "选择输出路径" : outputPath))
            {
                outputPath = EditorUtility.OpenFolderPanel("字体输出路径", Application.dataPath, "");
                if (false == string.IsNullOrEmpty(outputPath))
                {
                    outputPath = outputPath.Replace(Application.dataPath, "Assets") + "/";
                }
                else
                {
                    outputPath = "Assets/";
                }
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("创建"))
            {
                var chars = charContent.Replace("\n", ""); 

                if (textures.Count == 0)
                {
                    Debug.LogErrorFormat("PNG数量为0!");
                    return;
                }

                if (chars.Length == 0)
                {
                    Debug.LogErrorFormat("字符数量为0!");
                    return;
                }

                if(textures.Count != chars.Length)
                {
                    Debug.LogErrorFormat("PNG数量和字符数量不一致!");
                    return;
                }        
                
                if(string.IsNullOrWhiteSpace(outputPath))
                {
                    Debug.LogErrorFormat("输出路径未选择!");
                    return;
                }

                if (string.IsNullOrWhiteSpace(fontName))
                {
                    Debug.LogErrorFormat("输出字体名称未选择!");
                    return;
                }

                new BitmapFontCreateCommand(textures.ToArray(), chars.ToCharArray(), outputPath, fontName).Execute();
            }
            GUILayout.EndHorizontal();

            EditorGUILayout.Space();
            GUILayout.EndVertical();
        }
    }
}