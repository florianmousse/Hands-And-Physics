using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField] private AudioClip _compressClip, _uncompressClip;
    [SerializeField] private AudioSource _source;

    private static bool button1, button2, button3, button4, button5;

    public Text text;

    private static List<int> listNumbers;

    private static int numberOfButtonsPressed;


    [SerializeField] private float threshold = 0.1f;
    [SerializeField] private float deadZone = 0.025f;
    private bool isPressed;
    private Vector3 startPos;
    private ConfigurableJoint joint;

    public UnityEvent onPressed, onReleased;


    // Start is called before the first frame update
    void Start()
    {
        listNumbers = new List<int>();

        listNumbers = GenerateRandomOrder();

        allFalse();

        startPos = transform.localPosition;
        joint = GetComponent<ConfigurableJoint>();

        text.text = "You have to press the buttons to start the game. \nFind the right combination to win the game!";
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPressed && GetValue() + threshold >= 1) Pressed();

        if (isPressed && GetValue() - threshold <= 0) Released();
    }


    private float GetValue() {
        var value = Vector3.Distance(startPos, transform.localPosition) / joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone) value = 0;

        return Mathf.Clamp(value, -1f, 1f);
    }



    private void Pressed() {
        isPressed = true;
        onPressed.Invoke();
        _source.PlayOneShot(_compressClip);

        if (transform.name.Equals("Push1")) {
            if (listNumbers[numberOfButtonsPressed] == 1) {
                button1 = true;
                numberOfButtonsPressed++;
            }
            else allFalse();
        }

        if (transform.name.Equals("Push2")) {
            if (listNumbers[numberOfButtonsPressed] == 2) {
                button2 = true;
                numberOfButtonsPressed++;
            }
            else allFalse();
        }

        if (transform.name.Equals("Push3")) {
            if (listNumbers[numberOfButtonsPressed] == 3) {
                button3 = true;
                numberOfButtonsPressed++;
            }
            else allFalse();
        }

        if (transform.name.Equals("Push4")) {
            if (listNumbers[numberOfButtonsPressed] == 4) {
                button4 = true;
                numberOfButtonsPressed++;
            }
            else allFalse();
        }

        if (transform.name.Equals("Push5")) {
            if (listNumbers[numberOfButtonsPressed] == 5) {
                button5 = true;
                numberOfButtonsPressed++;
            }
            else allFalse();
        }
        
        buttonConfiguration(button1, "Push1");
        buttonConfiguration(button2, "Push2");
        buttonConfiguration(button3, "Push3");
        buttonConfiguration(button4, "Push4");
        buttonConfiguration(button5, "Push5");


        EndGame();
    }

    private void EndGame() {
        if (button1 && button2 && button3 && button4 && button5) {
            text.text = "You've won! \nPress any button again to start a new game.";
            listNumbers = GenerateRandomOrder();

            allFalse();
        }
        else {
            text.text = "Find the right combination by pressing the buttons to win the game!";
        }
    }
    
    public static List<int> GenerateRandomOrder() {
        List<int> list = new List<int>();

        for (int j = 0; j < 5; j++)
        {
            int rand = Random.Range(1, 6);
            while (list.Contains(rand)) {
                rand = Random.Range(1, 6);
            }
            list.Add(rand);
        }

        return list;
    }

    private static void allFalse() {
        numberOfButtonsPressed = 0;
        button1 = false;
        button2 = false;
        button3 = false;
        button4 = false;
        button5 = false;
    }

    private void buttonConfiguration(bool button, string push) {
        if (button) {
            GameObject.Find(push).GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.green);

            /*ConfigurableJoint configurableJoint = GameObject.Find(push).GetComponent<ConfigurableJoint>();

            JointDrive drive = new JointDrive();
            drive.positionSpring = 0;

            configurableJoint.yDrive = drive;*/
        } else {
            GameObject.Find(push).GetComponentInChildren<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

    private void Released() {
        isPressed = false;
        onReleased.Invoke();
        _source.PlayOneShot(_uncompressClip);
    }
}
