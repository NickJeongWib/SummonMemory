using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Linq;

public class GoogleSheetManager : MonoBehaviour
{
    [Tooltip("true: google sheet, false: local json")]
    // true�� GoogleSheet, false�� local�� �ִ� json
    [SerializeField] bool isAccessGoogleSheet = true; 
    [Tooltip("Google sheet appsscript webapp url")]
    // GoogleSheet Url�ּ�
    [SerializeField] string googleSheetUrl;           
    [Tooltip("Google sheet avail sheet tabs. seperate `/`. For example `Sheet1/Sheet2`")] 
    // ������ ��Ʈ �̸� /�� �и�
    [SerializeField] string availSheets;              
    [Tooltip("For example `/GenerateGoogleSheet`")] // ������ų ���� �ּ�
    [SerializeField] string generateFolderPath = "/GenerateGoogleSheet";
    [Tooltip("You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`")] 
    // GoogleSheetManager.SO<GoogleSheetSO>()����Ͽ� ���ٰ���
    public ScriptableObject googleSheetSO;

    // json���� �ּ�
    string JsonPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetJson.json";
    // GoogleSheetClassŬ���� �ּ�
    string ClassPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetClass.cs";
    // ScriptableObject �ּ�
    string SOPath => $"Assets{generateFolderPath}/GoogleSheetSO.asset"; 

    string[] availSheetArray; // ��Ʈ�̸��� �迭�� �����ϱ� ����
    string json;              // json
    bool refeshTrigger;       // Fetch������ �� �ν����Ϳ� ���� ������ Ȯ���ϰ� �� SO�� ����� ����
    public static GoogleSheetManager Inst;  // �̱������Ͽ� ���� 

    #region Init
    // �̱���
    private void Awake()
    {
        Inst = GetInstance();
    }

    // ���۰� ���ÿ� googleSheetSO ��ȯ�ϱ� ���� �Լ� �̸� �޸� �Ҵ�
    public static T SO<T>() where T : ScriptableObject
    {
        if (GetInstance().googleSheetSO == null)
        {
            // googleSheetSO�� �ƿ� ���� ��� null ��ȯ
            return null;
        }

        // googleSheetSO�� T Ÿ������ ĳ������ ��ȯ
        return GetInstance().googleSheetSO as T;
    }

    static GoogleSheetManager GetInstance()
    {
        if (Inst == null)
        {
            Inst = FindFirstObjectByType<GoogleSheetManager>();
        }
        return Inst;
    }
    #endregion

#if UNITY_EDITOR
    // ���� �������� ��Ʈ �� �޾ƿ���
    [ContextMenu("FetchGoogleSheet")]
    async void FetchGoogleSheet()
    {
        //Init
        // ���� �������� ��Ʈ�� ��Ʈ �̸��� �ɰ��� �迭�� �����Ѵ�
        availSheetArray = availSheets.Split('/');

        // ���۽�Ʈ�� �����Ұ���?
        if (isAccessGoogleSheet)
        {
            // googleSheetUrl�� �������� ��Ʈ�� �����͸� �񵿱� �ε��Ѵ�. 
            Debug.Log($"Loading from google sheet..");
            json = await LoadDataGoogleSheet(googleSheetUrl);
        }
        // �ƴ϶�� ���ÿ��� json ���� �ε�
        else
        {
            Debug.Log($"Loading from local json..");
            json = LoadDataLocalJson();
        }
        // LoadDataGoogleSheet�Լ��� ���ᰡ �Ǹ� Ȯ���ؼ� return;
        if (json == null) return;

        // ������ ���������� �ִ��� üũ
        bool isJsonSaved = SaveFileOrSkip(JsonPath, json);
        // C# Ŭ���� ����
        string allClassCode = GenerateCSharpClass(json);
        // ClassPath�� allClassCode ���������� �ִ��� üũ
        bool isClassSaved = SaveFileOrSkip(ClassPath, allClassCode);

        // �������� �ִٸ�
        if (isJsonSaved || isClassSaved)
        {
            // �������� ���� �ٲ������ OnValidate �Լ��� ������ �� �ְ� true
            refeshTrigger = true;
            // ����
            UnityEditor.AssetDatabase.Refresh();
        }
        // �������� ���ٸ� GoogleSheetSO ����
        else
        {
            // GoogleSheetSO �����ϰ� ��Ʈ�� �����͸� �� �Ű�ٰ� Debug
            CreateGoogleSheetSO();
            Debug.Log($"Fetch done.");
        }
    }

    async Task<string> LoadDataGoogleSheet(string url)
    {
        // Http ��û�� ���� �� �ִ� ��ü ����
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // �񵿱�� ������ url�� Get ��û
                // �������� ��ȯ�� �����͸� ����Ʈ �迭(byte[])�� ����
                byte[] dataBytes = await client.GetByteArrayAsync(url);
                // dataBytes UTF8 ���ڵ��Ͽ� return
                return Encoding.UTF8.GetString(dataBytes);
            }
            catch (HttpRequestException e)
            {
                // ��û ����
                Debug.LogError($"Request error: {e.Message}");
                return null;
            }
        }
    }

    string LoadDataLocalJson()
    {
        // ���ÿ� JsonPath�� �����ϸ�
        if (File.Exists(JsonPath))
        {
            // JsonPath�� ���� �о return
            return File.ReadAllText(JsonPath);
        }

        // �������� ������ ������ �������� �ʾƼ� null�� return
        Debug.Log($"File not exist.\n{JsonPath}");
        return null;
    }

    bool SaveFileOrSkip(string path, string contents)
    {
        // ���� ��ġ ����
        string directoryPath = Path.GetDirectoryName(path);
        // ���� ��ġ�� �������� �ʴٸ�
        if (!Directory.Exists(directoryPath))
        {
            // directoryPath ��ġ�� ���ϸ���
            Directory.CreateDirectory(directoryPath);
        }

        // ������ �ش� path �����ϰ� path�� ������ ��� �ؽ�Ʈ�� �Ű������� ���� contents�� ������ false
        // path�� json�� �ּ� �� ���̰� contents�� ��Ʈ���� �ε��� �� string ���̴ϱ� �̹� �����ϰ� �Ȱ����� Ȯ�� 
        if (File.Exists(path) && File.ReadAllText(path).Equals(contents))
            return false;

        // ���� if���� �ȵ��ٸ� path�� ���� contents�� ���� �ٸ��� ������ �����
        File.WriteAllText(path, contents);
        return true;
    }

    // ��Ʈ�� �����ϴ��� üũ�ϱ����� �Լ�
    bool IsExistAvailSheets(string sheetName)
    {
        // availSheetArray�� �Ű������� ���� sheetName�� �����ϴ��� üũ
        return Array.Exists(availSheetArray, x => x == sheetName);
    }

    string GenerateCSharpClass(string jsonInput)
    {
        // jsonInput�Ľ��ؼ� jsonObject�� ����
        JObject jsonObject = JObject.Parse(jsonInput);
        // ���ڿ��� ȿ�������� ������ �� ���� Ŭ����
        StringBuilder classCode = new();

        // ���ӽ����̽� �ڵ� ����
        classCode.AppendLine("using System;\nusing System.Collections.Generic;\nusing UnityEngine;\n");
        // ���� Ŭ������ ���� ���� �� �߰�
        classCode.AppendLine("/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>");
        // ScriptableObject�� ��� �޴� GoogleSheetSO Ŭ������ ����
        classCode.AppendLine("public class GoogleSheetSO : ScriptableObject\n{");

        // Scriptable Object ����
        // �Ű������� ���� ���� �������� ��Ʈ ���� ��ŭ �ݺ�
        foreach (var sheet in jsonObject)
        {
            // Ŭ������ �̸��� �ش� ��Ʈ�� �̸����� �ʱ�ȭ
            string className = sheet.Key;
            // ���� Ŭ���� �̸��� �������� ������ false�� return�Ǽ� true�� �Ǵϱ� continue
            if (!IsExistAvailSheets(className))
                continue;

            // public List<{��Ʈ �̸�}> {��Ʈ �̸�}List; �߰�
            classCode.AppendLine($"\tpublic List<{className}> {className}List;");
        }
        // ��Ʈ�� ����Ʈ�� �� �̾����� �߰�ȣ �ݱ�
        classCode.AppendLine("}\n");

        // Class ����
        foreach (var jObject in jsonObject)
        {
            // Ŭ���� �̸� ����
            string className = jObject.Key;
            // ���� Ŭ���� �̸��� �������� ������ false�� return�Ǽ� true�� �Ǵϱ� continue
            if (!IsExistAvailSheets(className))
                continue;

            // �ش� ��Ʈ ���� �����͵��� �迭 ���·� ����
            var items = (JArray)jObject.Value;
            // items�� ù��° �ε����� JObject�� ��ȯ
            var firstItem = (JObject)items[0];
            // ��Ʈ �̸��� Ŭ���� ����
            classCode.AppendLine($"[Serializable]\npublic class {className}\n{{");

            // int > float > bool > string
            int itemIndex = 0;
            // propertyCount�� ���� �� ũ��
            int propertyCount = ((JObject)items[0]).Properties().Count();
            // propertyCountũ�⸸ŭ �迭 �ʱ�ȭ
            string[] propertyTypes = new string[propertyCount];

            // item �� �������� �� ��ŭ �ݺ� ex) ĳ���� 24���̴ϱ� 24�� �ݺ�
            foreach (JToken item in items)
            {
                // ��Ʈ�� �����ʹ� 0���� ����
                itemIndex = 0;

                // �� ũ�� ��ŭ �ݺ�
                // ��Ʈ���� ������ Ÿ�� �����ֱ�
                foreach (var property in ((JObject)item).Properties())
                {
                    // ������ Ÿ�� �����ֱ�
                    string propertyType = GetCSharpType(property.Value.Type);
                    // �ٷ� ������ ������ Ÿ���� foreach���� �ѹ� ���� ���� �����Ͱ��� �Ǳ⶧���� propertyTypes[itemIndex]�� ����صα�
                    string oldPropertyType = propertyTypes[itemIndex];

                    // oldPropertyType�� ���ٸ�
                    if (oldPropertyType == null)
                    {
                        // ���� foreach���� itemIndex��° �ε����� ����Ÿ���� propertyType���� �ϴ� ����
                        propertyTypes[itemIndex] = propertyType;
                    }
                    // ���� Ÿ���� int��
                    else if (oldPropertyType == "int")
                    {
                        // propertyType int��
                        if (propertyType == "int")
                        {
                            // ���� �ε����� propertyTypes Ÿ��  int + int = int ���x
                            propertyTypes[itemIndex] = "int";
                        }
                        // propertyType float��
                        else if (propertyType == "float")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� float + int = float�̴ϱ�
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType bool�̸�
                        else if (propertyType == "bool")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� bool + int = string���� ó��
                            propertyTypes[itemIndex] = "string";
                        }
                        // propertyType string �̸�
                        else if (propertyType == "string")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� string ����
                            propertyTypes[itemIndex] = "string";
                        }

                    }
                    // ���� Ÿ���� float��
                    else if (oldPropertyType == "float")
                    {
                        // propertyType int��
                        if (propertyType == "int")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� int + float = float���� ��ȯ
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType float��
                        else if (propertyType == "float")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� float + float = float
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType bool�̸�
                        else if (propertyType == "bool")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� bool + float = string���� ��ȯ
                            propertyTypes[itemIndex] = "string";
                        }
                        // propertyType string �̸�
                        else if (propertyType == "string")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� string + float = string���� ��ȯ
                            propertyTypes[itemIndex] = "string";
                        }

                    }
                    // ���� Ÿ���� bool��
                    else if (oldPropertyType == "bool")
                    {
                        if (propertyType == "int")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� int + bool = string���� ��ȯ
                            propertyTypes[itemIndex] = "string";
                        }
                        else if (propertyType == "float")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� float + bool = string���� ��ȯ
                            propertyTypes[itemIndex] = "string";
                        }
                        else if (propertyType == "bool")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� bool + bool = bool
                            propertyTypes[itemIndex] = "bool";
                        }
                        else if (propertyType == "string")
                        {
                            // ���� �ε����� propertyTypes Ÿ�� string + bool = string���� ��ȯ
                            propertyTypes[itemIndex] = "string";
                        }
                           
                    }
                    // �ε��� ����
                    itemIndex++;
                }
            }

            // string���� ������ Ÿ�� �з��� �������� ���� Ÿ���� �����ֱ� ���� 0���� �ʱ�ȭ
            itemIndex = 0;
            foreach (var property in firstItem.Properties())
            {
                // ������ 
                string propertyName = property.Name;
                // ���� Ÿ��
                string propertyType = propertyTypes[itemIndex];
                // Ŭ������ �ۼ�
                classCode.AppendLine($"\tpublic {propertyType} {propertyName};");
                // �Ϸ� ������ ���� �ε����� �����ϱ� ���� itemIndex++
                itemIndex++;
            }

            // Ŭ������ �� �ۼ� ������ �߰�ȣ �ݱ�
            classCode.AppendLine("}\n");
        }
        // classCode return;
        return classCode.ToString();
    }

    // ��Ʈ���� ���� �����͸� ������ �� � ������ Ÿ������ �����ֱ� ���� �Լ�
    string GetCSharpType(JTokenType jsonType)
    {
        switch (jsonType)
        {
            case JTokenType.Integer:
                return "int";
            case JTokenType.Float:
                return "float";
            case JTokenType.Boolean:
                return "bool";
            default:
                return "string";
        }
    }

    // ScriptableObject ����
    bool CreateGoogleSheetSO()
    {
        // GoogleSheetSOŸ���� null�̸� false�� return
        if (Type.GetType("GoogleSheetSO") == null)
            return false;

        // GoogleSheetSO �̸����� SO ����
        googleSheetSO = ScriptableObject.CreateInstance("GoogleSheetSO");
        // jsonObject�� JObject�� json�� �Ľ��� 
        JObject jsonObject = JObject.Parse(json);
        try
        {
            // ��Ʈ���� ��ŭ �ݺ�  
            foreach (var jObject in jsonObject)
            {
                // className�� ��Ʈ�̸� Ŭ����
                string className = jObject.Key;
                // className���������ʴ� ��Ʈ�� continue�� �Ѿ��
                if (!IsExistAvailSheets(className))
                    continue;

                // classType�� className�� Ÿ������
                Type classType = Type.GetType(className);
                // List<classType> Ÿ�� ����
                Type listType = typeof(List<>).MakeGenericType(classType);
                // ��Ÿ�ӿ��� ����Ʈ �ν��Ͻ� ����
                IList listInst = (IList)Activator.CreateInstance(listType);
                // ��Ʈ �ȿ��� ����
                var items = (JArray)jObject.Value;
                // ���� ������ŭ �ݺ�
                foreach (var item in items)
                {
                    // classType���� ������Ʈ ����
                    object classInst = Activator.CreateInstance(classType);
                    // ���� ������ŭ �ݺ�
                    foreach (var property in ((JObject)item).Properties())
                    {
                        // �ش� classType�� �ʵ忡 property.Name�� ã�� 
                        FieldInfo fieldInfo = classType.GetField(property.Name);
                        // value�� property.Value�� fieldInfo.FieldType���� ������ Ÿ���� ��ȯ���ش�.
                        object value = Convert.ChangeType(property.Value.ToString(), fieldInfo.FieldType);
                        // ��ȯ�� value�� classInst�� �������ش�.
                        fieldInfo.SetValue(classInst, value);
                    }
                    // ����Ʈ�� �߰�
                    listInst.Add(classInst);
                }
                // googleSheetSO�� {className}List�� �ʵ��� Ÿ���� listInst���� ��ȯ���ش�.
                googleSheetSO.GetType().GetField($"{className}List").SetValue(googleSheetSO, listInst);
            }
        }
        catch (Exception e)
        {
            // �Ľ� ���� ����
            Debug.LogError($"CreateGoogleSheetSO error: {e.Message}");
        }

        // print ���
        print("CreateGoogleSheetSO");
        // googleSheetSO SO�� SOPath ��ġ�� ����
        UnityEditor.AssetDatabase.CreateAsset(googleSheetSO, SOPath);
        // ���µ����͸� Ȯ���� �����ϱ� ����
        UnityEditor.AssetDatabase.SaveAssets();
        return true;
    }

    void OnValidate()
    {
        // FetchGoogleSheet�� ���� ��Ʈ�� ���� ���� ���� �Ǿ��ٸ� refeshTrigger true�� �Ǿ�
        if (refeshTrigger)
        {
            // ���� ��Ʈ ScriptableObject�� ���������� ��������� �˱�����
            bool isCompleted = CreateGoogleSheetSO();
            // ���������� ������ �Ǿ��ٸ�
            if (isCompleted)
            {
                // ��Ʈ�� �ۼ��� ������ ����� ����Ǿ����� false�� �ٲ���
                refeshTrigger = false;
                // Fetch�Ϸ�
                Debug.Log($"Fetch done.");
            }
        }
    }
#endif
}