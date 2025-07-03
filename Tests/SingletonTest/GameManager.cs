using UnityEngine;
using Kuro.UnityUtils.DesignPatterns;

namespace Kuro.UnityUtils
{
    public class GameManager : MonoSingleton<GameManager>
    {
        protected override void Awake()
        {
            base.Awake(); // Call the base Awake method to ensure the singleton is initialized properly
            Persistent(); // Make this instance persistent across scene loads

            Debug.Log("GameManager Awake called. Singleton instance initialized.");
        }

        [SerializeField] private string playerName = "DefaultPlayer";
        public string PlayerName => playerName;
        public void SetPlayerName(string name) => playerName = name;
    }
}
