using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    // References to everything
    public Player player;
    public List<GameObject> enemies;

    public int enemiesLeft;

    // Start is called before the first frame update
    void Start()
    {
        if (enemies == null)
        {
            enemies = new List<GameObject>();
        }
        // set everything's gameref to this, just in case
        player.gameRef = this;
        foreach (GameObject e in enemies)
        {
            //...
        }
    }

    // Update is called once per frame
    void Update()
    {      
    }

    public void onPlayerKill()
    {
        // Popup a loss screen or something
        // For now, crash the game
        int x = 0;
        x /= 0;
    }

    public void onEnemyKill()
    {
        --enemiesLeft;
        if (enemiesLeft == 0)
        {
            // Win the game or level if we're doing that
        }
    }

}
