using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainEndGameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI gameResult;
    [SerializeField] private GameObject sunkenSubPrefab;
    [SerializeField] private GameObject treasureChestPrefab;
    
    
    void Start()
    {
        if (GameOptions.gameState == Gamestate.SubOwn)
        {
            Instantiate(treasureChestPrefab);
            gameResult.text = "Submarine survived!";

        } else if (GameOptions.gameState == Gamestate.PhoneOwn)
        {
            Instantiate(sunkenSubPrefab);
            gameResult.text = "Submarine destroyed!";
        }

        var audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null)
        {
            Destroy(audioManager.transform.parent.gameObject);
        }
    }

    public void QuitToMenu()
    {
        var serverManager = FindObjectOfType<ServerManager>();
        if (serverManager != null)
        {
            serverManager.Send("Quit");
            Destroy(serverManager.gameObject);
        }

        SceneManager.LoadScene(0);
    }

    public void PlayAgain()
    {
        var serverManager = FindObjectOfType<ServerManager>();
        if (serverManager != null)
        {
            serverManager.Send("PlayAgain");
        }

        SceneManager.LoadScene("MainGameScene");
        
    }
    
    
}
