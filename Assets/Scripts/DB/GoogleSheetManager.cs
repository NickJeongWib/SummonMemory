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
    // true면 GoogleSheet, false면 local에 있는 json
    [SerializeField] bool isAccessGoogleSheet = true; 
    [Tooltip("Google sheet appsscript webapp url")]
    // GoogleSheet Url주소
    [SerializeField] string googleSheetUrl;           
    [Tooltip("Google sheet avail sheet tabs. seperate `/`. For example `Sheet1/Sheet2`")] 
    // 가져올 시트 이름 /로 분리
    [SerializeField] string availSheets;              
    [Tooltip("For example `/GenerateGoogleSheet`")] // 생성시킬 폴더 주소
    [SerializeField] string generateFolderPath = "/GenerateGoogleSheet";
    [Tooltip("You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`")] 
    // GoogleSheetManager.SO<GoogleSheetSO>()사용하여 접근가능
    public ScriptableObject googleSheetSO;

    // json파일 주소
    string JsonPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetJson.json";
    // GoogleSheetClass클래스 주소
    string ClassPath => $"{Application.dataPath}{generateFolderPath}/GoogleSheetClass.cs";
    // ScriptableObject 주소
    string SOPath => $"Assets{generateFolderPath}/GoogleSheetSO.asset"; 

    string[] availSheetArray; // 시트이름들 배열로 저장하기 위해
    string json;              // json
    bool refeshTrigger;       // Fetch눌렀을 때 인스펙터에 값의 변동을 확인하고 새 SO를 만들기 위한
    public static GoogleSheetManager Inst;  // 싱글톤패턴에 사용된 

    #region Init
    // 싱글톤
    private void Awake()
    {
        Inst = GetInstance();
    }

    // 시작과 동시에 googleSheetSO 반환하기 위한 함수 미리 메모리 할당
    public static T SO<T>() where T : ScriptableObject
    {
        if (GetInstance().googleSheetSO == null)
        {
            // googleSheetSO가 아예 없을 경우 null 반환
            return null;
        }

        // googleSheetSO를 T 타입으로 캐스팅해 반환
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
    // 구글 스프레드 시트 값 받아오기
    [ContextMenu("FetchGoogleSheet")]
    async void FetchGoogleSheet()
    {
        //Init
        // 구글 스프레드 시트의 시트 이름을 쪼개서 배열에 저장한다
        availSheetArray = availSheets.Split('/');

        // 구글시트로 연결할건지?
        if (isAccessGoogleSheet)
        {
            // googleSheetUrl의 스프레드 시트의 데이터를 비동기 로드한다. 
            Debug.Log($"Loading from google sheet..");
            json = await LoadDataGoogleSheet(googleSheetUrl);
        }
        // 아니라면 로컬에서 json 파일 로드
        else
        {
            Debug.Log($"Loading from local json..");
            json = LoadDataLocalJson();
        }
        // LoadDataGoogleSheet함수가 종료가 되면 확인해서 return;
        if (json == null) return;

        // 파일이 변동사항이 있는지 체크
        bool isJsonSaved = SaveFileOrSkip(JsonPath, json);
        // C# 클래스 생성
        string allClassCode = GenerateCSharpClass(json);
        // ClassPath와 allClassCode 변동사항이 있는지 체크
        bool isClassSaved = SaveFileOrSkip(ClassPath, allClassCode);

        // 변경점이 있다면
        if (isJsonSaved || isClassSaved)
        {
            // 변수들의 값이 바뀌었으니 OnValidate 함수가 실행할 수 있게 true
            refeshTrigger = true;
            // 갱신
            UnityEditor.AssetDatabase.Refresh();
        }
        // 변경점이 없다면 GoogleSheetSO 생성
        else
        {
            // GoogleSheetSO 생성하고 시트의 데이터를 다 옮겼다고 Debug
            CreateGoogleSheetSO();
            Debug.Log($"Fetch done.");
        }
    }

    async Task<string> LoadDataGoogleSheet(string url)
    {
        // Http 요청을 보낼 수 있는 객체 생성
        using (HttpClient client = new HttpClient())
        {
            try
            {
                // 비동기로 지정한 url에 Get 요청
                // 서버에서 반환한 데이터를 바이트 배열(byte[])로 받음
                byte[] dataBytes = await client.GetByteArrayAsync(url);
                // dataBytes UTF8 인코딩하여 return
                return Encoding.UTF8.GetString(dataBytes);
            }
            catch (HttpRequestException e)
            {
                // 요청 실패
                Debug.LogError($"Request error: {e.Message}");
                return null;
            }
        }
    }

    string LoadDataLocalJson()
    {
        // 로컬에 JsonPath가 존재하면
        if (File.Exists(JsonPath))
        {
            // JsonPath의 값을 읽어서 return
            return File.ReadAllText(JsonPath);
        }

        // 존재하지 않으면 파일이 존재하지 않아서 null로 return
        Debug.Log($"File not exist.\n{JsonPath}");
        return null;
    }

    bool SaveFileOrSkip(string path, string contents)
    {
        // 파일 위치 지정
        string directoryPath = Path.GetDirectoryName(path);
        // 파일 위치가 존재하지 않다면
        if (!Directory.Exists(directoryPath))
        {
            // directoryPath 위치로 파일만듬
            Directory.CreateDirectory(directoryPath);
        }

        // 파일이 해당 path 존재하고 path의 파일을 모두 텍스트로 매개변수로 받은 contents와 같으면 false
        // path가 json의 주소 일 것이고 contents는 시트에서 로드해 온 string 값이니까 이미 존재하고 똑같은지 확인 
        if (File.Exists(path) && File.ReadAllText(path).Equals(contents))
            return false;

        // 위에 if문이 안들어갔다면 path의 값과 contents의 값이 다르기 때문에 덮어쓰기
        File.WriteAllText(path, contents);
        return true;
    }

    // 시트가 존재하는지 체크하기위한 함수
    bool IsExistAvailSheets(string sheetName)
    {
        // availSheetArray에 매개변수로 받은 sheetName이 존재하는지 체크
        return Array.Exists(availSheetArray, x => x == sheetName);
    }

    string GenerateCSharpClass(string jsonInput)
    {
        // jsonInput파싱해서 jsonObject로 설정
        JObject jsonObject = JObject.Parse(jsonInput);
        // 문자열을 효율적으로 조립할 때 쓰는 클래스
        StringBuilder classCode = new();

        // 네임스페이스 코드 기입
        classCode.AppendLine("using System;\nusing System.Collections.Generic;\nusing UnityEngine;\n");
        // 만들 클래스에 대한 설명 글 추가
        classCode.AppendLine("/// <summary>You must approach through `GoogleSheetManager.SO<GoogleSheetSO>()`</summary>");
        // ScriptableObject를 상속 받는 GoogleSheetSO 클래스로 생성
        classCode.AppendLine("public class GoogleSheetSO : ScriptableObject\n{");

        // Scriptable Object 생성
        // 매개변수로 받은 구글 스프레드 시트 갯수 만큼 반복
        foreach (var sheet in jsonObject)
        {
            // 클래스의 이름을 해당 시트의 이름으로 초기화
            string className = sheet.Key;
            // 만약 클래스 이름이 존재하지 않으면 false로 return되서 true가 되니까 continue
            if (!IsExistAvailSheets(className))
                continue;

            // public List<{시트 이름}> {시트 이름}List; 추가
            classCode.AppendLine($"\tpublic List<{className}> {className}List;");
        }
        // 시트의 리스트를 다 뽑았으니 중괄호 닫기
        classCode.AppendLine("}\n");

        // Class 생성
        foreach (var jObject in jsonObject)
        {
            // 클래스 이름 생성
            string className = jObject.Key;
            // 만약 클래스 이름이 존재하지 않으면 false로 return되서 true가 되니까 continue
            if (!IsExistAvailSheets(className))
                continue;

            // 해당 시트 안의 데이터들을 배열 형태로 저장
            var items = (JArray)jObject.Value;
            // items의 첫번째 인덱스를 JObject로 변환
            var firstItem = (JObject)items[0];
            // 시트 이름의 클래스 선언
            classCode.AppendLine($"[Serializable]\npublic class {className}\n{{");

            // int > float > bool > string
            int itemIndex = 0;
            // propertyCount는 세로 열 크기
            int propertyCount = ((JObject)items[0]).Properties().Count();
            // propertyCount크기만큼 배열 초기화
            string[] propertyTypes = new string[propertyCount];

            // item 열 데이터의 수 만큼 반복 ex) 캐릭터 24종이니까 24번 반복
            foreach (JToken item in items)
            {
                // 시트의 데이터는 0부터 시작
                itemIndex = 0;

                // 행 크기 만큼 반복
                // 시트안의 데이터 타입 정해주기
                foreach (var property in ((JObject)item).Properties())
                {
                    // 변수의 타입 정해주기
                    string propertyType = GetCSharpType(property.Value.Type);
                    // 바로 위에서 저장한 타입은 foreach문이 한번 돌면 이전 데이터값이 되기때문에 propertyTypes[itemIndex]로 기억해두기
                    string oldPropertyType = propertyTypes[itemIndex];

                    // oldPropertyType이 없다면
                    if (oldPropertyType == null)
                    {
                        // 현재 foreach문의 itemIndex번째 인덱스의 변수타입을 propertyType으로 일단 설정
                        propertyTypes[itemIndex] = propertyType;
                    }
                    // 이전 타입이 int면
                    else if (oldPropertyType == "int")
                    {
                        // propertyType int면
                        if (propertyType == "int")
                        {
                            // 현재 인덱스의 propertyTypes 타입  int + int = int 상관x
                            propertyTypes[itemIndex] = "int";
                        }
                        // propertyType float면
                        else if (propertyType == "float")
                        {
                            // 현재 인덱스의 propertyTypes 타입 float + int = float이니까
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType bool이면
                        else if (propertyType == "bool")
                        {
                            // 현재 인덱스의 propertyTypes 타입 bool + int = string으로 처리
                            propertyTypes[itemIndex] = "string";
                        }
                        // propertyType string 이면
                        else if (propertyType == "string")
                        {
                            // 현재 인덱스의 propertyTypes 타입 string 유지
                            propertyTypes[itemIndex] = "string";
                        }

                    }
                    // 이전 타입이 float면
                    else if (oldPropertyType == "float")
                    {
                        // propertyType int면
                        if (propertyType == "int")
                        {
                            // 현재 인덱스의 propertyTypes 타입 int + float = float으로 변환
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType float면
                        else if (propertyType == "float")
                        {
                            // 현재 인덱스의 propertyTypes 타입 float + float = float
                            propertyTypes[itemIndex] = "float";
                        }
                        // propertyType bool이면
                        else if (propertyType == "bool")
                        {
                            // 현재 인덱스의 propertyTypes 타입 bool + float = string으로 변환
                            propertyTypes[itemIndex] = "string";
                        }
                        // propertyType string 이면
                        else if (propertyType == "string")
                        {
                            // 현재 인덱스의 propertyTypes 타입 string + float = string으로 변환
                            propertyTypes[itemIndex] = "string";
                        }

                    }
                    // 이전 타입이 bool면
                    else if (oldPropertyType == "bool")
                    {
                        if (propertyType == "int")
                        {
                            // 현재 인덱스의 propertyTypes 타입 int + bool = string으로 변환
                            propertyTypes[itemIndex] = "string";
                        }
                        else if (propertyType == "float")
                        {
                            // 현재 인덱스의 propertyTypes 타입 float + bool = string으로 변환
                            propertyTypes[itemIndex] = "string";
                        }
                        else if (propertyType == "bool")
                        {
                            // 현재 인덱스의 propertyTypes 타입 bool + bool = bool
                            propertyTypes[itemIndex] = "bool";
                        }
                        else if (propertyType == "string")
                        {
                            // 현재 인덱스의 propertyTypes 타입 string + bool = string으로 변환
                            propertyTypes[itemIndex] = "string";
                        }
                           
                    }
                    // 인덱스 증가
                    itemIndex++;
                }
            }

            // string으로 데이터 타입 분류가 끝났으니 이제 타입을 정해주기 위해 0으로 초기화
            itemIndex = 0;
            foreach (var property in firstItem.Properties())
            {
                // 변수명 
                string propertyName = property.Name;
                // 변수 타입
                string propertyType = propertyTypes[itemIndex];
                // 클래스에 작성
                classCode.AppendLine($"\tpublic {propertyType} {propertyName};");
                // 완료 됬으면 다음 인덱스에 접근하기 위해 itemIndex++
                itemIndex++;
            }

            // 클래스를 다 작성 했으니 중괄호 닫기
            classCode.AppendLine("}\n");
        }
        // classCode return;
        return classCode.ToString();
    }

    // 시트에서 변수 데이터를 가져올 때 어떤 데이터 타입인지 정해주기 위한 함수
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

    // ScriptableObject 생성
    bool CreateGoogleSheetSO()
    {
        // GoogleSheetSO타입이 null이면 false로 return
        if (Type.GetType("GoogleSheetSO") == null)
            return false;

        // GoogleSheetSO 이름으로 SO 생성
        googleSheetSO = ScriptableObject.CreateInstance("GoogleSheetSO");
        // jsonObject은 JObject로 json을 파싱한 
        JObject jsonObject = JObject.Parse(json);
        try
        {
            // 시트갯수 만큼 반복  
            foreach (var jObject in jsonObject)
            {
                // className은 시트이름 클래스
                string className = jObject.Key;
                // className존재하지않는 시트면 continue로 넘어가기
                if (!IsExistAvailSheets(className))
                    continue;

                // classType은 className의 타입으로
                Type classType = Type.GetType(className);
                // List<classType> 타입 생성
                Type listType = typeof(List<>).MakeGenericType(classType);
                // 런타임에서 리스트 인스턴스 생성
                IList listInst = (IList)Activator.CreateInstance(listType);
                // 시트 안에서 값들
                var items = (JArray)jObject.Value;
                // 열의 갯수만큼 반복
                foreach (var item in items)
                {
                    // classType으로 오브젝트 생성
                    object classInst = Activator.CreateInstance(classType);
                    // 행의 갯수만큼 반복
                    foreach (var property in ((JObject)item).Properties())
                    {
                        // 해당 classType의 필드에 property.Name를 찾음 
                        FieldInfo fieldInfo = classType.GetField(property.Name);
                        // value는 property.Value를 fieldInfo.FieldType으로 데이터 타입을 변환해준다.
                        object value = Convert.ChangeType(property.Value.ToString(), fieldInfo.FieldType);
                        // 변환된 value를 classInst에 설정해준다.
                        fieldInfo.SetValue(classInst, value);
                    }
                    // 리스트에 추가
                    listInst.Add(classInst);
                }
                // googleSheetSO의 {className}List의 필드의 타입을 listInst으로 변환해준다.
                googleSheetSO.GetType().GetField($"{className}List").SetValue(googleSheetSO, listInst);
            }
        }
        catch (Exception e)
        {
            // 파싱 실패 오류
            Debug.LogError($"CreateGoogleSheetSO error: {e.Message}");
        }

        // print 출력
        print("CreateGoogleSheetSO");
        // googleSheetSO SO를 SOPath 위치에 생성
        UnityEditor.AssetDatabase.CreateAsset(googleSheetSO, SOPath);
        // 에셋데이터를 확실히 저장하기 위해
        UnityEditor.AssetDatabase.SaveAssets();
        return true;
    }

    void OnValidate()
    {
        // FetchGoogleSheet를 눌러 시트에 대한 값이 변동 되었다면 refeshTrigger true가 되어
        if (refeshTrigger)
        {
            // 구글 시트 ScriptableObject가 성공적으로 생성됬는지 알기위해
            bool isCompleted = CreateGoogleSheetSO();
            // 성공적으로 생성이 되었다면
            if (isCompleted)
            {
                // 시트에 작성된 데이터 값들로 변경되었으니 false로 바꿔줌
                refeshTrigger = false;
                // Fetch완료
                Debug.Log($"Fetch done.");
            }
        }
    }
#endif
}