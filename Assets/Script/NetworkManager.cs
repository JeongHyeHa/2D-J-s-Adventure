using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkManager
{
    //// 서버의 IP주소를 사용할 것(내부IP)
    //private string url = "http://localhost:5054/api/scores/";
    //private string url = "http://cglabhospital.iptime.org:40001/api/scores/";
    private string url = "http://dduki.iptime.org:40001/api/scores/";

    // 특정 ID의 데이터 조회
    public IEnumerator CoGetPlayerById(int userId, System.Action<Score> onSuccess)
    {
        string apiUrl = url + userId;

        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + webRequest.error);
                onSuccess?.Invoke(null);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                Score retrievedScore = JsonConvert.DeserializeObject<Score>(jsonResponse);
                onSuccess?.Invoke(retrievedScore);
            }
        }
    }

    // 회원가입(데이터 추가)
    public IEnumerator CoPostPlayer(Score newScore, System.Action onSuccess)
    {
        string jsonData = JsonConvert.SerializeObject(newScore);

        using (UnityWebRequest webRequest = new UnityWebRequest($"{url}signup", "POST"))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                Debug.Log("회원가입 성공!");
                onSuccess?.Invoke();
            }
        }
    }

    // 성적 업데이트
    public IEnumerator CoPostScore(int userId, int newScore, Action onScoreSaved)
    {
        string jsonData = JsonConvert.SerializeObject(new { user_id = userId, gameScore = newScore });

        using (UnityWebRequest webRequest = new UnityWebRequest($"{url}updateScore", "POST"))
        {
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Score updated successfully!");
                onScoreSaved?.Invoke();
            }
        }
    }


    // 시도횟수 업데이트
    public IEnumerator CoPostTryCount(int userId, int newTryCount)
    {
        // 서버에서 기대하는 필드 이름과 일치시켜 JSON 데이터 생성
        string jsonData = JsonConvert.SerializeObject(new { user_id = userId, tryCount = newTryCount });

        using (UnityWebRequest webRequest = new UnityWebRequest($"{url}updateTryCount", "POST"))
        {
            // JSON 데이터를 UTF-8 형식으로 전송
            webRequest.SetRequestHeader("Content-Type", "application/json");
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            webRequest.downloadHandler = new DownloadHandlerBuffer();

            // 웹 요청 전송
            yield return webRequest.SendWebRequest();

            // 오류 처리
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log("Try count updated successfully!");
            }
        }
    }

    // 시도횟수 가져오기
    public IEnumerator CoGetTrycountById(int userId, Action<Score> onSuccess)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{url}userId"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // 서버에서 응답 받은 데이터를 JSON으로 변환하여 Score 객체에 저장
                string jsonResponse = webRequest.downloadHandler.text;
                Score playerData = JsonUtility.FromJson<Score>(jsonResponse);
                onSuccess?.Invoke(playerData); // 가져온 데이터를 onSuccess 콜백으로 전달
            }
        }
    }

    // 점수 상위 10명의 데이터 가져오기
    public IEnumerator CoGetTop10Scores(Action<List<Score>> onSuccess)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get($"{url}top10"))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                onSuccess?.Invoke(null);
            }
            else
            {
                string jsonResponse = webRequest.downloadHandler.text;
                List<Score> top10Scores = JsonConvert.DeserializeObject<List<Score>>(jsonResponse);
                onSuccess?.Invoke(top10Scores);  
            }
        }
    }


}

[System.Serializable]
public class Score
{
    public int user_id { get; set; }
    public string department { get; set; } = string.Empty;
    public string user_name { get; set; } = string.Empty;
    public int tryCount { get; set; }
    public int user_score { get; set; }
}
