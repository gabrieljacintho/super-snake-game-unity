using GabrielBertasso.BackendIntegration.DTOs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GabrielBertasso.Assets.Scripts.BackendIntegration
{
    public class PlayerItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _highscoreText;
        [SerializeField] private Button _deleteButton;

        public void Initialize(PlayerResponse playerResponse)
        {

        }
    }
}