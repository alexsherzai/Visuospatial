using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GenerateShapes : MonoBehaviour
{

    private static int questionNum = 1;
    private float timeBetween = 0.0f;
    private int direction;
    private List<int> directions = new List<int>();

    public GameObject totalShape;

    public Text questionNumText;

    private static System.Random rng = new System.Random();

    public List<GameObject> optionsShapes = new List<GameObject>();

    public static int correctScore = 0;
    private string correctButton;
    public static string clickedButton;
    public static Boolean buttonClicked = false;

    public static void Shuffle(List<GameObject> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            GameObject value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> listCubes = new List<GameObject>();
        listCubes.Clear();

        foreach (Transform child in totalShape.GetComponentInChildren<Transform>())
        {
            listCubes.Add(child.gameObject);
        }
    }

    void resetPos()
    {
        Vector3 pos1 = new Vector3(-7.5f, -3f, -4.189f);
        Vector3 pos2 = new Vector3(-2.75f, -3f, -4.189f);
        Vector3 pos3 = new Vector3(2.75f, -3f, -4.189f);
        Vector3 pos4 = new Vector3(7.5f, -3f, -4.189f);

        List<Vector3> positions = new List<Vector3> { pos1, pos2, pos3, pos4 };
        for(int i = 0; i < optionsShapes.Count; i++)
        {
            optionsShapes[i].transform.position = positions[i];
        }
    }

    void generateShape(GameObject totalStructure, List<int> directionList)
    {
        List<GameObject> listCubes = new List<GameObject>();
        listCubes.Clear();

        //resetPos();

        foreach (Transform child in totalStructure.GetComponentInChildren<Transform>())
        {
            listCubes.Add(child.gameObject);
        }

        Vector3 distanceTraveled = new Vector3(0.0f, 0.0f, 0.0f);

        for (int i = 1; i < listCubes.Count; i++)
        {
            GameObject cubeCenter = listCubes[i - 1];
            if (i < listCubes.Count - 1)
            {
                if (((int) (directionList[i - 1] / 2)) == ((int) directionList[i] / 2))
                {
                    directionList[i] += 2;
                }
                directionList[i] %= 6;
            }
            switch (directionList[i - 1])
            {
                case 0: //TOP
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    distanceTraveled += new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    break;
                case 1: //BOTTOM
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    distanceTraveled -= new Vector3(0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.y, 0.0f);
                    break;
                case 2: //RIGHT
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    distanceTraveled += new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    break;
                case 3: //LEFT
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    distanceTraveled -= new Vector3(cubeCenter.GetComponent<BoxCollider>().bounds.size.x, 0.0f, 0.0f);
                    break;
                case 4: //FRONT
                    listCubes[i].transform.position = cubeCenter.transform.position - new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    distanceTraveled -= new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    break;
                case 5: //BEHIND
                    listCubes[i].transform.position = cubeCenter.transform.position + new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    distanceTraveled += new Vector3(0.0f, 0.0f, cubeCenter.GetComponent<BoxCollider>().bounds.size.z);
                    break;
            }
        }

        //Debug.Log(totalStructure.ToString() + ": " + distanceTraveled);
        //totalStructure.transform.position -= distanceTraveled;

    }

    void generateOptions() {
        List<int> direction1 = new List<int>(directions);
        List<int> direction2 = new List<int>(directions);
        List<int> direction3 = new List<int>(directions);
        List<int> direction4 = new List<int>(directions);

        int temp = direction1[0];
        while(direction1[0] == temp)
        {
            direction1[0] = rng.Next(0, 6);
        }

        temp = direction2[1];
        while (direction2[1] == temp)
        {
            direction2[1] = rng.Next(0, 6);
        }

        temp = direction3[2];
        while (direction3[2] == temp)
        {
            direction3[2] = rng.Next(0, 6);
        }

        List<GameObject> options = optionsShapes;
        Shuffle(options);

        generateShape(options[0], direction1);
        generateShape(options[1], direction2);
        generateShape(options[2], direction3);
        generateShape(options[3], direction4);

        correctButton = options[3].name;
    }

    // Update is called once per frame
    void Update()
    {
        if(buttonClicked)
        {
            if(clickedButton.Split('-')[1].Equals(correctButton.Split('-')[1]))
            {
                correctScore++;
            }
            questionNum++;
            timeBetween = 0.0f;
            buttonClicked = false;
        }

        
        if (questionNum <= 15)
        {
            if(timeBetween == 0.0f)
            {
                int count = 0;

                System.Random rnd = new System.Random();

                while (count < 4)
                {
                    direction = rnd.Next(0, 5);
                    directions.Add(direction);
                    count++;
                }

                generateShape(totalShape, directions);
                generateOptions();


                questionNumText.text = "Question " + questionNum.ToString();
                timeBetween++;
                directions.Clear();
            }
        } else
        {
            SceneManager.LoadScene(1);
        }
    }
}
