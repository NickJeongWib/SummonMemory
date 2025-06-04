using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    Character SelectCharacter;
    public Character Get_SelectChar { get => SelectCharacter; set => SelectCharacter = value; }
    public static GameManager Instance;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            // Debug.Log(1);
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            // Debug.Log(2);
            Destroy(this);
        }
    }
}
