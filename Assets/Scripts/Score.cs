using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    public Text score;

    // Start is called before the first frame update
    void Start()
    {
        int correct = GenerateShapes.correctScore;

        score.text = "Your score is: " + correct + "/15";
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
