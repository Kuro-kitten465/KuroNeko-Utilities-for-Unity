using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils
{
    public class SingletonClient : MonoBehaviour
    {
        [SerializeField] private string newPlayerName = "NewPlayer";

        private void OnGUI()
        {
            GUILayout.Label("Singleton Client");
            GUILayout.Label($"Player Name: {GameManager.Instance.PlayerName}");

            if (GUILayout.Button("Change Player Name"))
            {
                GameManager.Instance.SetPlayerName(newPlayerName);
            }
        }
    }
}
