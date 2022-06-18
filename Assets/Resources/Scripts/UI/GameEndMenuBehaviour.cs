using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEndMenuBehaviour : MonoBehaviour
{
    int score;
    [SerializeField] GameObject gameEndMenu;
    [SerializeField] Text scoreText;
    bool gameEnded = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnded)
            return;
        if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.leftButton.isPressed || Mouse.current.rightButton.isPressed || Mouse.current.middleButton.isPressed)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void showGameEndScreen(int score)
    {
        if (gameEnded)
            return;
        scoreText.text = score.ToString();
        gameEnded = true;
        gameEndMenu.SetActive(true);
    }
}
