using GabrielBertasso.BackendIntegration.DTOs;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GabrielBertasso.BackendIntegration
{
    public class AccountManager : Singleton<AccountManager>
    {
        public override bool IsPersistent => true;

        [SerializeField] private string _baseUrl = "http://localhost:5218";

        public AuthenticationResponse Session { get; private set; }
        public bool IsLoggedIn => Session != null && !string.IsNullOrEmpty(Session.token);

        public event Action<AuthenticationResponse> SessionChanged;

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this)
            {
                return;
            }

            LoadSession();
        }

        public void Register(RegisterRequest registerRequest, Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(RegisterCoroutine(registerRequest, onSuccess, onError));
        }

        public IEnumerator RegisterCoroutine(RegisterRequest registerRequest, Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            yield return Post("/api/account/register", JsonUtility.ToJson(registerRequest), false, onSuccess, onError);
        }

        public void Login(LoginRequest loginRequest, Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(LoginCoroutine(loginRequest, onSuccess, onError));
        }

        public IEnumerator LoginCoroutine(LoginRequest loginRequest, Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            yield return Post("/api/account/login", JsonUtility.ToJson(loginRequest), false, onSuccess, onError);
        }

        public void Logout(Action onDone = null)
        {
            StartCoroutine(LogoutCoroutine(onDone));
        }

        public IEnumerator LogoutCoroutine(Action onDone)
        {
            using UnityWebRequest request = new UnityWebRequest($"{_baseUrl}/api/account/logout", "POST");

            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Authorization", $"Bearer {Session.token}");

            yield return request.SendWebRequest();

            ClearSession();

            onDone?.Invoke();

            Debug.Log("[Backend] Logout done.");
        }

        public void RefreshToken(Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            StartCoroutine(RefreshTokenCoroutine(onSuccess, onError));
        }

        public IEnumerator RefreshTokenCoroutine(Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            RefreshTokenRequest refreshTokenRequest = new RefreshTokenRequest(Session.token, Session.refreshToken);

            yield return Post("/api/account/refresh-token", JsonUtility.ToJson(refreshTokenRequest), false, onSuccess, onError);
        }

        public IEnumerator EnsureValidToken(Action<bool> onResult = null)
        {
            if (!IsLoggedIn)
            {
                onResult?.Invoke(false);
                yield break;
            }

            if (Session.ExpirationUtc > DateTime.UtcNow)
            {
                onResult?.Invoke(true);
                yield break;
            }

            bool ok = false;

            yield return RefreshTokenCoroutine(_ => ok = true, _ => ClearSession());

            onResult?.Invoke(ok);
        }

        private IEnumerator Post(string path, string json, bool auth, Action<AuthenticationResponse> onSuccess = null, Action<string> onError = null)
        {
            using UnityWebRequest request = UnityWebRequest.Post($"{_baseUrl}{path}", json, "application/json");

            if (auth)
            {
                request.SetRequestHeader("Authorization", $"Bearer {Session.token}");
            }

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                string msg = string.IsNullOrEmpty(request.downloadHandler.text) ? request.error : request.downloadHandler.text;
                onError?.Invoke(msg);

                yield break;
            }

            Session = JsonUtility.FromJson<AuthenticationResponse>(request.downloadHandler.text);
            SessionChanged?.Invoke(Session);

            Debug.Log($"[Backend] Session changed: name={Session.name}, email={Session.email}, expiration={Session.expiration}");

            SaveSession();

            onSuccess?.Invoke(Session);
        }

        private void SaveSession()
        {
            PlayerPrefs.SetString("auth_session", JsonUtility.ToJson(Session));
            PlayerPrefs.Save();
        }

        private void LoadSession()
        {
            string raw = PlayerPrefs.GetString("auth_session", "");

            if (!string.IsNullOrEmpty(raw))
            {
                Session = JsonUtility.FromJson<AuthenticationResponse>(raw);
                SessionChanged?.Invoke(Session);
            }
        }

        private void ClearSession()
        {
            Session = null;
            SessionChanged?.Invoke(Session);

            PlayerPrefs.DeleteKey("auth_session");
        }
    }
}
