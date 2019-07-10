using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour {

    [SerializeField] string sceneName;

	void Start () {
        SceneManager.LoadSceneAsync(sceneName);
	}
}
