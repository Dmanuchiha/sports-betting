using FPSPrototype.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FPSPrototype.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private HealthShieldSystem playerVitals;

        private void Start()
        {
            playerVitals.OnDied += HandlePlayerDeath;
        }

        private void OnDestroy()
        {
            if (playerVitals != null)
            {
                playerVitals.OnDied -= HandlePlayerDeath;
            }
        }

        private void HandlePlayerDeath()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
