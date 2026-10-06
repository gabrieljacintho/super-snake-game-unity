using GabrielBertasso.BackendIntegration.DTOs;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GabrielBertasso.BackendIntegration
{
    public class LoginView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TMP_InputField _emailField;
        [SerializeField] private TMP_InputField _passwordField;
        [SerializeField] private Button _submitButton;
        [SerializeField] private TMP_Text _errorText;

        [Space]
        public UnityEvent OnSuccess;

        private void OnEnable()
        {
            SetInteractable(true);
            ResetTexts(string.Empty);

            _submitButton.onClick.AddListener(Submit);
        }

        private void OnDisable()
        {
            _submitButton.onClick.RemoveListener(Submit);
        }

        public void Submit()
        {
            SetInteractable(false);
            _errorText.text = string.Empty;

            LoginRequest loginRequest = new LoginRequest(_emailField.text, _passwordField.text);

            AccountManager.Instance.Login(loginRequest, _ =>
            {
                Login_Completed(string.Empty);
                OnSuccess?.Invoke();
            }, Login_Completed);
        }

        private void Login_Completed(string error)
        {
            SetInteractable(true);
            ResetTexts(error);
        }

        private void SetInteractable(bool interactable)
        {
            _canvasGroup.interactable = interactable;
        }

        private void ResetTexts(string error)
        {
            _emailField.text = string.Empty;
            _passwordField.text = string.Empty;
            _errorText.text = error;
        }
    }
}
