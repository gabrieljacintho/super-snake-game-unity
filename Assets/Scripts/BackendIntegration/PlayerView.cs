using GabrielBertasso.BackendIntegration.DTOs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GabrielBertasso.BackendIntegration
{
    public class PlayerView : MonoBehaviour
    {
        [Header("Authenticated")]
        [SerializeField] private GameObject _authenticatedContent;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _highscoreText;
        [SerializeField] private Button _logoutButton;

        [Header("Not Authenticated")]
        [SerializeField] private GameObject _notAuthenticatedContent;

        private void OnEnable()
        {
            AccountManager.Instance.SessionChanged += SessionChanged;
            PlayersManager.Instance.HighscoreUpdated += HighscoreUpdated;
            SessionChanged(AccountManager.Instance.Session);

            _logoutButton.onClick.AddListener(Logout);
        }

        private void OnDisable()
        {
            AccountManager.Instance.SessionChanged -= SessionChanged;
            PlayersManager.Instance.HighscoreUpdated -= HighscoreUpdated;
            _logoutButton.onClick.RemoveListener(Logout);
        }

        public void UpdateView()
        {
            bool isAuthenticated = AccountManager.Instance.IsLoggedIn;

            _authenticatedContent.SetActive(isAuthenticated);
            _notAuthenticatedContent.SetActive(!isAuthenticated);

            if (isAuthenticated)
            {
                _nameText.text = AccountManager.Instance.Session.name;
                _highscoreText.text = string.Empty;
                PlayersManager.Instance.GetHighscore(player =>
                {
                    _highscoreText.text = player.highscore.ToString();
                });
            }
        }

        private void SessionChanged(AuthenticationResponse session)
        {
            UpdateView();
        }

        private void HighscoreUpdated(int newHighscore)
        {
            _highscoreText.text = newHighscore.ToString();
        }

        private void Logout()
        {
            AccountManager.Instance.Logout();
        }
    }
}
