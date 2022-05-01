using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class JCSVImporterWindow : EditorWindow
{

    GUIStyle AlphaStyle;
    GUIStyle boxStyle;
    GUIStyle richTextStyle_Mid;
    GUIStyle richTextStyle_Left;
    GUIStyle headerStyle;
    GUIStyle smallHeaderStyle;
    GUIStyle lineStyle;
    GUIStyle foldoutWithRichText;


    private TextAsset csvFile;


    public enum VertexInputParamType
    {
        Position = 0,
        Normal = 1,
        Tangent = 2,
        Color = 3,
        UV0 = 10,
        UV1 = 11,
        UV2 = 12,
        UV3 = 13,
        UV4 = 14,
        UV5 = 15,
        UV6 = 16,
        UV7 = 17,
        UV8 = 18
    }
    public enum VertexInputParamSize
    {
        float2,
        float3,
        float4
    }

    public class VertexInputParam
    {
        public Vector2 valueFloat2 = Vector2.zero;
        public Vector3 valueFloat3 = Vector3.zero;
        public Vector4 valueFloat4 = Vector4.zero;
        public Vector2[] listFloat2;
        public Vector3[] listFloat3;
        public Vector4[] listFloat4;
        public Color[] listColor;

        private VertexInputParamType m_paramType = VertexInputParamType.Position;
        public VertexInputParamType ParamType
        {
            get
            {
                return m_paramType;
            }
            set
            {
                m_paramType = value;
            }
        }
        private VertexInputParamSize m_paramSize = VertexInputParamSize.float4;
        public VertexInputParamSize ParamSize
        {
            get
            {
                return m_paramSize;
            }
            set
            {
                if(m_paramType == VertexInputParamType.Position || m_paramType == VertexInputParamType.Normal)
                {
                    if(value == VertexInputParamSize.float2)
                    {
                        m_paramSize = VertexInputParamSize.float3;
                        Debug.LogWarning("Wanted to set [<color=blue>" + m_paramType.ToString() + "</color>] as [<color=yellow>" + value.ToString() + "</color>], but be set to [<color=blue>float3</color>].");
                    }
                    else
                    {
                        m_paramSize = value;
                    }
                }
                else if(m_paramType == VertexInputParamType.Tangent)
                {
                    if (value != VertexInputParamSize.float4)
                    {
                        Debug.LogWarning("Wanted to set [<color=blue>" + m_paramType.ToString() + "</color>] as [<color=yellow>" + value.ToString() + "</color>], but be set to [<color=blue>float4</color>].");
                    }
                    m_paramSize = VertexInputParamSize.float4;
                }
                else
                {
                    m_paramSize = value;
                }
            }
        }

        public VertexInputParam(VertexInputParamType _paramType, VertexInputParamSize _paramSize)
        {
            ParamType = _paramType;
            ParamSize = _paramSize;
        }

        public VertexInputParam Clone()
        {
            VertexInputParam newItem = new VertexInputParam(m_paramType, m_paramSize);
            return newItem;
        }
    }


    public class VertexData_JCSV
    {
        public List<VertexInputParam> vertexInputs = new List<VertexInputParam>();

        public VertexData_JCSV Clone()
        {
            VertexData_JCSV newItem = new VertexData_JCSV();
            for (int i = 0; i < vertexInputs.Count; i++)
            {
                newItem.vertexInputs.Add(vertexInputs[i].Clone());
            }
            return newItem;
        }

        public bool ExistParamType(VertexInputParamType _paramType)
        {
            for(int i=0; i< vertexInputs.Count; i++)
            {
                if (vertexInputs[i].ParamType == _paramType)
                {
                    return true;
                }
            }
            return false;
        }

        public int GetParamTypeIndex(VertexInputParamType _paramType)
        {
            for (int i = 0; i < vertexInputs.Count; i++)
            {
                if (vertexInputs[i].ParamType == _paramType)
                {
                    return i;
                }
            }
            return -1;
        }

        public void AddNewInput()
        {
            if(ExistParamType( VertexInputParamType.Position) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.Position, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.Normal) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.Normal, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.Color) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.Color, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.Tangent) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.Tangent, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV0) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV0, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV1) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV1, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV2) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV2, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV3) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV3, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV4) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV4, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV5) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV5, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV6) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV6, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV7) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV7, VertexInputParamSize.float4));
                return;
            }
            if (ExistParamType(VertexInputParamType.UV8) == false)
            {
                vertexInputs.Add(new VertexInputParam(VertexInputParamType.UV8, VertexInputParamSize.float4));
                return;
            }
            Debug.LogWarning("Can Not Add any type of Inputs !");
        }
        public void ChangeInputParamType(int _index, VertexInputParamType _newParamType)
        {
            int existedIndex = GetParamTypeIndex(_newParamType);
            if (existedIndex == -1 || existedIndex == _index)
            {
                vertexInputs[_index].ParamType = _newParamType;
                vertexInputs[_index].ParamSize = vertexInputs[_index].ParamSize;//reset.
            }
            else
            {
                Debug.LogWarning("Can Not set param type to [<color=red>" + _newParamType.ToString() + "</color>], because it has exist.");
            }
        }
    }


    private VertexData_JCSV setDatas = new VertexData_JCSV();
    private float scale = 1f;

    [MenuItem("JTools/J CSV Importer")]
    static void Open()
    {
        var win = EditorWindow.GetWindow<JCSVImporterWindow>(false, "J CSV Importer", true);
        win.GenerateStyles();
        var icon = Resources.Load("Textures/jicon2") as Texture;
        win.titleContent = new GUIContent("J CSV Importer", icon);
    }

    Vector2 scrollPos_Inputs = Vector2.zero;
    private void OnGUI()
    {
        GUILayout.Box(" J CSV Importer", boxStyle, GUILayout.Height(60), GUILayout.ExpandWidth(true));
        GUILayout.Box("You can use this tool to import some CSV text file.", richTextStyle_Mid, GUILayout.ExpandWidth(true));

        GUILayout.Space(10);
        DrawALine(3);
        GUILayout.Space(10);
        GUILayout.Label(" BaseSets", headerStyle);
        csvFile = EditorGUILayout.ObjectField("CSVFile", csvFile, typeof(TextAsset), true) as TextAsset;
        scale = EditorGUILayout.Slider("Scale", scale, 0.001f, 100f);
        EditorGUILayout.LabelField("Primitive Topology: TrangleList", richTextStyle_Left);

        GUILayout.Space(10);
        DrawALine(3);
        GUILayout.Space(10);
        GUILayout.Label(" Inputs", headerStyle);
        scrollPos_Inputs = EditorGUILayout.BeginScrollView(scrollPos_Inputs);

        if(setDatas.vertexInputs.Count == 0)
        {
            setDatas.AddNewInput();
        }
        for(int i=0; i< setDatas.vertexInputs.Count; i++)
        {
            var inputParam1 = setDatas.vertexInputs[i];
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Input[" + i + "]    <color=blue>" + inputParam1.ParamType.ToString() + "</color>", richTextStyle_Left, GUILayout.Width(120));
            GUILayout.FlexibleSpace();
            if (i == 0)
                GUI.enabled = false;
            if(GUILayout.Button("-", GUILayout.Width(20)))
            {
                setDatas.vertexInputs.RemoveAt(i);
                Repaint();
            }
            GUI.enabled = true;
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel++;
            //if (i == 0) GUI.enabled = false;
            var newParamType = (VertexInputParamType)EditorGUILayout.EnumPopup("Type: ", inputParam1.ParamType, GUILayout.Width(250));
            //GUI.enabled = true;
            if(newParamType != inputParam1.ParamType)
            {
                setDatas.ChangeInputParamType(i, newParamType);//try change param type.
            }
            var newParamSize = (VertexInputParamSize)EditorGUILayout.EnumPopup("Size: ", inputParam1.ParamSize, GUILayout.Width(250));
            if (newParamSize != inputParam1.ParamSize)
            {
                inputParam1.ParamSize = newParamSize;
            }
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if(GUILayout.Button("Add After This", GUILayout.Width(200)))
            {
                setDatas.AddNewInput();
            }
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        DrawALine(3);
        GUILayout.Space(10);

        if(GUILayout.Button("Import ! ", GUILayout.Height(60)))
        {
            StartImport();
        }

        GUILayout.Space(10);
    }

    public void StartImport()
    {
        if (csvFile != null)
        {
            var path = AssetDatabase.GetAssetPath(csvFile);
            if (System.IO.Path.GetExtension(path) == ".csv")
            {
                Dictionary<int, VertexData_JCSV> vertexDic = new Dictionary<int, VertexData_JCSV>();
                List<int> indexData = new List<int>();
                int firstIDX = -1;//
                using (System.IO.StreamReader sr = new System.IO.StreamReader(path))
                {
                    string line = sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine();
                        string[] token = line.Split(',');
                        //Debug.Log(line);
                        int vidx = int.Parse(token[1]);
                        if (firstIDX == -1)
                        {
                            firstIDX = vidx;
                            vidx = 0;
                        }
                        else
                        {
                            vidx = vidx - firstIDX;
                        }

                        indexData.Add(vidx);
                        if (!vertexDic.ContainsKey(vidx))
                        {
                            int idx = 2;
                            VertexData_JCSV vert = setDatas.Clone();
                            for(int i=0; i< vert.vertexInputs.Count; i++)
                            {
                                var inputParam1 = vert.vertexInputs[i];
                                if(inputParam1.ParamSize == VertexInputParamSize.float2)
                                {
                                    inputParam1.valueFloat2 = ReadVector2(token, ref idx);
                                }
                                else if(inputParam1.ParamSize == VertexInputParamSize.float3)
                                {
                                    inputParam1.valueFloat3 = ReadVector3(token, ref idx);
                                }
                                else if (inputParam1.ParamSize == VertexInputParamSize.float4)
                                {
                                    if(inputParam1.ParamType == VertexInputParamType.Position || inputParam1.ParamType == VertexInputParamType.Normal)
                                    {
                                        inputParam1.valueFloat3 = ReadVector4(token, ref idx);
                                    }
                                    else
                                    {
                                        inputParam1.valueFloat4 = ReadVector4(token, ref idx);
                                    }
                                }
                            }
                            vertexDic[vidx] = vert;
                        }

                    }

                }

                int vertCnt = vertexDic.Count;
                Mesh mesh = new Mesh();
                VertexData_JCSV listDataTemp = setDatas.Clone();
                for(int i=0; i< listDataTemp.vertexInputs.Count; i++)
                {
                    var inputParam1 = listDataTemp.vertexInputs[i];
                    if (inputParam1.ParamSize == VertexInputParamSize.float2)
                    {
                        if(inputParam1.ParamType == VertexInputParamType.Color)//color must use color list.
                        {
                            inputParam1.listColor = new Color[vertCnt];
                        }
                        else
                        {
                            inputParam1.listFloat2 = new Vector2[vertCnt];
                        }
                    }
                    else if (inputParam1.ParamSize == VertexInputParamSize.float3)
                    {
                        if (inputParam1.ParamType == VertexInputParamType.Color)//color must use color list.
                        {
                            inputParam1.listColor = new Color[vertCnt];
                        }
                        else
                        {
                            inputParam1.listFloat3 = new Vector3[vertCnt];
                        }
                    }
                    else if (inputParam1.ParamSize == VertexInputParamSize.float4)
                    {
                        if (inputParam1.ParamType == VertexInputParamType.Color)//color must use color list.
                        {
                            inputParam1.listColor = new Color[vertCnt];
                        }
                        else if (inputParam1.ParamType == VertexInputParamType.Position || inputParam1.ParamType == VertexInputParamType.Normal)//these use float3 list.
                        {
                            inputParam1.listFloat3 = new Vector3[vertCnt];
                        }
                        else
                        {
                            inputParam1.listFloat4 = new Vector4[vertCnt];
                        }


                    }
                }

                for (int i = 0; i < vertCnt; i++)
                {
                    if (!vertexDic.ContainsKey(i))
                        continue;
                    VertexData_JCSV vert = vertexDic[i];
                    for(int j=0; j< listDataTemp.vertexInputs.Count; j++)
                    {
                        var paramType = listDataTemp.vertexInputs[j].ParamType;
                        var sizeType = listDataTemp.vertexInputs[j].ParamSize;
                        if (sizeType == VertexInputParamSize.float2)//float2 be uv or color.
                        {
                            if (paramType == VertexInputParamType.Color)//color must use color list.
                            {
                                var listTemp1 = listDataTemp.vertexInputs[j].listColor;
                                Vector2 ve2 = vert.vertexInputs[j].valueFloat2;
                                listTemp1[i] = new Color(ve2.x, ve2.y, 0, 1);
                            }
                            else
                            {
                                var listTemp1 = listDataTemp.vertexInputs[j].listFloat2;
                                listTemp1[i] = vert.vertexInputs[j].valueFloat2;
                            }
                          
                        }
                        else if (sizeType == VertexInputParamSize.float3)
                        {
                            if (paramType == VertexInputParamType.Color)//color must use color list.
                            {
                                var listTemp1 = listDataTemp.vertexInputs[j].listColor;
                                Vector3 ve3 = vert.vertexInputs[j].valueFloat3;
                                listTemp1[i] = new Color(ve3.x, ve3.y, ve3.z, 1);
                            }
                            else
                            {
                                var listTemp1 = listDataTemp.vertexInputs[j].listFloat3;
                                listTemp1[i] = vert.vertexInputs[j].valueFloat3;
                                if (paramType == VertexInputParamType.Position)//position scale
                                {
                                    listTemp1[i] /= scale;
                                }
                            }

                        }
                        else if (sizeType == VertexInputParamSize.float4)
                        {
                            if(paramType == VertexInputParamType.Position || paramType == VertexInputParamType.Normal)
                            {
                                var listTemp1 = listDataTemp.vertexInputs[j].listFloat3;
                                listTemp1[i] = vert.vertexInputs[j].valueFloat3;
                                if (paramType == VertexInputParamType.Position)//position scale
                                {
                                    listTemp1[i] /= scale;
                                }
                            }
                            else
                            {
                                if (paramType == VertexInputParamType.Color)//color must use color list.
                                {
                                    var listTemp1 = listDataTemp.vertexInputs[j].listColor;
                                    Vector4 ve4 = vert.vertexInputs[j].valueFloat4;
                                    listTemp1[i] = ve4;
                                }
                                else
                                {
                                    var listTemp1 = listDataTemp.vertexInputs[j].listFloat4;
                                    listTemp1[i] = vert.vertexInputs[j].valueFloat4;
                                }
                             
                            }

                        }
                    }
                }
                for(int i=0; i< listDataTemp.vertexInputs.Count; i++)
                {
                    var paramType = listDataTemp.vertexInputs[i].ParamType;
                    var paramSize = listDataTemp.vertexInputs[i].ParamSize;
                    if (paramType == VertexInputParamType.Position)
                    {
                        mesh.vertices = listDataTemp.vertexInputs[i].listFloat3;
                    }
                    else if(paramType == VertexInputParamType.Normal)
                    {
                        mesh.normals = listDataTemp.vertexInputs[i].listFloat3;
                    }
                    else if (paramType == VertexInputParamType.Tangent)
                    {
                        mesh.tangents = listDataTemp.vertexInputs[i].listFloat4;
                    }
                    else if (paramType == VertexInputParamType.Color)
                    {
                        mesh.colors = listDataTemp.vertexInputs[i].listColor;
                    }
                    else
                    {
                        if(paramType >= VertexInputParamType.UV0 && paramType<= VertexInputParamType.UV8)
                        {
                            int uvChannel = (int)paramType - 10;
                            if(paramSize == VertexInputParamSize.float2)
                            {
                                mesh.SetUVs(uvChannel, listDataTemp.vertexInputs[i].listFloat2);
                            }
                            else if(paramSize == VertexInputParamSize.float3)
                            {
                                mesh.SetUVs(uvChannel, listDataTemp.vertexInputs[i].listFloat3);
                            }
                            else if (paramSize == VertexInputParamSize.float4)
                            {
                                mesh.SetUVs(uvChannel, listDataTemp.vertexInputs[i].listFloat4);
                            }
                        }
                        else
                        {
                            Debug.LogError("Unknow param type : [<color=red>" + (int)paramType + "</color>] !!!");
                        }
                    }
                }
                mesh.RecalculateBounds();
                mesh.triangles = indexData.ToArray();

                if (mesh)
                {
                    SaveMeshToAsset(mesh);
                }

            }
        }
        else
        {
            Debug.Log("Not have a valid csvFile.");
        }
    }

    private void SaveMeshToAsset(Mesh _mesh)
    {
        if (_mesh)
        {
            var path = AssetDatabase.GetAssetPath(csvFile);

            string folder = System.IO.Path.GetDirectoryName(path);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            AssetDatabase.CreateAsset(_mesh, $"{folder}/{name}.asset");
            AssetDatabase.SaveAssets();
            Debug.Log("saved [" + csvFile.name + "]");
        }

    }

    static float ReadFloat(string[] token, ref int idx)
    {
        var res = float.Parse(token[idx]);
        idx += 1;
        return res;
    }

    static Vector2 ReadVector2(string[] token, ref int idx)
    {
        var res = new Vector2(float.Parse(token[idx]), float.Parse(token[idx + 1]));
        idx += 2;
        return res;
    }

    static Vector3 ReadVector3(string[] token, ref int idx)
    {
        var res = new Vector3(float.Parse(token[idx]), float.Parse(token[idx + 1]), float.Parse(token[idx + 2]));
        idx += 3;
        return res;
    }

    static Vector4 ReadVector4(string[] token, ref int idx)
    {
        var res = new Vector4(float.Parse(token[idx]), float.Parse(token[idx + 1]), float.Parse(token[idx + 2]), float.Parse(token[idx + 3]));
        idx += 4;
        return res;
    }

    #region HelpDrawFunctions
    void DrawALine(int _height)
    {
        GUILayout.Box("", lineStyle, GUILayout.ExpandWidth(true), GUILayout.Height(_height));
    }
    void ShowProcessBar(float _jindu)
    {
        if (Event.current.type == EventType.Repaint)
        {
            var lastRect = GUILayoutUtility.GetLastRect();
            EditorGUI.ProgressBar(new Rect(lastRect.x, lastRect.y + lastRect.height, lastRect.width, 20), Mathf.Clamp01(_jindu), "");
        }
        GUILayout.Space(20);
    }
    #endregion



    void GenerateStyles()
    {
        AlphaStyle = new GUIStyle();
        AlphaStyle.normal.background = Resources.Load("Textures/touming") as Texture2D;

        boxStyle = new GUIStyle();
        boxStyle.normal.background = Resources.Load("Textures/d_box") as Texture2D;
        boxStyle.normal.textColor = Color.white;
        boxStyle.border = new RectOffset(3, 3, 3, 3);
        boxStyle.margin = new RectOffset(5, 5, 5, 5);
        boxStyle.fontSize = 25;
        boxStyle.fontStyle = FontStyle.Bold;
        boxStyle.font = Resources.Load("Fonts/GAU_Root_Nomal") as Font;
        boxStyle.alignment = TextAnchor.MiddleCenter;

        richTextStyle_Mid = new GUIStyle();
        richTextStyle_Mid.richText = true;
        richTextStyle_Mid.normal.textColor = Color.white;
        richTextStyle_Mid.alignment = TextAnchor.MiddleCenter;

        richTextStyle_Left = new GUIStyle();
        richTextStyle_Left.richText = true;

        headerStyle = new GUIStyle();
        headerStyle.border = new RectOffset(3, 3, 3, 3);
        headerStyle.fontSize = 17;
        headerStyle.fontStyle = FontStyle.Bold;

        smallHeaderStyle = new GUIStyle();
        smallHeaderStyle.border = new RectOffset(3, 3, 3, 3);
        smallHeaderStyle.fontSize = 14;
        smallHeaderStyle.fontStyle = FontStyle.Bold;

        lineStyle = new GUIStyle();
        lineStyle.normal.background = boxStyle.normal.background;
        lineStyle.alignment = TextAnchor.MiddleCenter;

        foldoutWithRichText = new GUIStyle();
        foldoutWithRichText = EditorStyles.foldout;
        foldoutWithRichText.richText = true;

    }

}
