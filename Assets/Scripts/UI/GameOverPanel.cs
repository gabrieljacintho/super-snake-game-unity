using GabrielBertasso.Core;
using UnityEngine;

namespace GabrielBertasso.UI
{
    public class GameOverPanel : MonoBehaviour
    {
        public KeyCode restartKey = KeyCode.Return;
        public Audio restartAudio;

        [Space]
        public KeyCode homeKey = KeyCode.Escape;
        public Audio homeAudio;


        private void Update()
        {
            if (GameManager.Instance == null) return;

            if (Input.GetKeyDown(restartKey))
            {
                GameManager.Instance.StartGame();
                restartAudio.Play();
            }
            else if (Input.GetKeyDown(homeKey))
            {
                GameManager.Instance.BackToHome();
                homeAudio.Play();
            }
        }
    }
}
