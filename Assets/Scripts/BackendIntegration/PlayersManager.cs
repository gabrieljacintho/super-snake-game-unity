using GabrielBertasso.BackendIntegration.DTOs;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GabrielBertasso.BackendIntegration
{
    public class PlayersManager : Singleton<PlayersManager>
    {
        public override bool IsPersistent => true;

        [SerializeField] private string _baseUrl = "http://localhost:5218";

        public event Action<int> HighscoreUpdated;

        public void GetHighscore(Action<PlayerResponse> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(GetHighscoreCoroutine(onSuccess, onError));
        }

        public IEnumerator GetHighscoreCoroutine(Action<PlayerResponse> onSuccess = null, Action<string> onError = null)
        {
            yield return Send("GET", null, onSuccess, onError);
        }

        public void UpdateHighscore(int newHighscore, Action<PlayerResponse> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(UpdateHighscoreCoroutine(newHighscore, onSuccess, onError));
        }

        public IEnumerator UpdateHighscoreCoroutine(int newHighscore, Action<PlayerResponse> onSuccess = null, Action<string> onError = null)
        {
            yield return Send("PUT", newHighscore.ToString(), playerResponse =>
            {
                onSuccess?.Invoke(playerResponse);
                HighscoreUpdated?.Invoke(newHighscore);
            }, onError);
        }

        public IEnumerator Send(string method, string json, Action<PlayerResponse> onSuccess = null, Action<string> onError = null)
        {
            bool tokenOk = false;

            yield return AccountManager.Instance.EnsureValidToken(ok => tokenOk = ok);

            if (!tokenOk)
            {
                onError?.Invoke("Session expired. Please log in again.");
                yield break;
            }

            using UnityWebRequest request = new UnityWebRequest($"{_baseUrl}/api/players/highscore", method);

            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Authorization", $"Bearer {AccountManager.Instance.Session.token}");

            if (!string.IsNullOrEmpty(json))
            {
                request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                request.SetRequestHeader("Content-Type", "application/json");
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                onError?.Invoke(request.error);
                yield break;
            }

            PlayerResponse response = JsonUtility.FromJson<PlayerResponse>(request.downloadHandler.text);

            onSuccess?.Invoke(response);

            Debug.Log($"[Backend] Highscore updated: {response.highscore}");
        }
    }
}