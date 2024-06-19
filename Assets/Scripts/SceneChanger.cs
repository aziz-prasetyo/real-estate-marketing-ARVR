using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneName;

    private void Awake()
    {
        // Ensure the script is attached to a UI Button
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(ChangeScene);
        }
        else
        {
            Debug.LogError("SceneChanger script needs to be attached to a GameObject with a Button component.");
        }
    }

    private void ChangeScene()
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError("Scene name is not set in the SceneChanger script.");
        }
    }
}
