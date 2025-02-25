using UnityEngine.SceneManagement;
using UnityEngine;

namespace Core
{
    
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
    
}
