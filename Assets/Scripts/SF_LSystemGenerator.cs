using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformStack
{
    // Stack speichert Position und Rotation

    public Vector3 pos;
    public Quaternion rot;
}

public class SF_LSystemGenerator : MonoBehaviour
{
    // Stefan Friesen

    public Vector3 pos;
    public Quaternion rot;
    public GameObject branch;

    private Stack<TransformStack> transformStack;
    private string axiom = "X";
    private byte iterations = 3;
    private float length = 0.1f, angle = 20.0f, randomRot;
    private Dictionary<char, string> rules;

    // Start is called before the first frame update
    void Start()
    {
        transformStack = new Stack<TransformStack>();
        randomRot = UnityEngine.Random.Range(20.0f, 120.0f);

        rules = new Dictionary<char, string> {
            {'F', "FF"}, // 1 Zweig nach oben (+ Rendering)
            {'X', "[*+FX]X[+FX][/+F-FX]"} // Abzweigungen
        };

        string currString = axiom;

        for(int i = 0; i < iterations; i++) {
            foreach(char c in currString) {
                if(rules.ContainsKey(c)) {
                    currString += rules[c];
                } else {
                    currString += c.ToString();
                }
            }
        }

        foreach(char c in currString) {
            switch(c) {
                case 'F':
                    Vector3 initPos = transform.position;
                    transform.Translate(Vector3.up * length);

                    // Teil des Baums der hier gerendert werden soll:
                    GameObject treePart = Instantiate(branch);
                    LineRenderer lr = treePart.GetComponent<LineRenderer>();
                    lr.startWidth = 0.02f;
                    lr.endWidth = 0.02f;
                    lr.SetPosition(0, initPos);
                    lr.SetPosition(1, transform.position);
                    break;

                case 'X':
                    break;

                case '+':
                    transform.Rotate(Vector3.back * angle);
                    break;

                case '-':
                    transform.Rotate(Vector3.forward * angle);
                    break;

                case '*':
                    transform.Rotate(Vector3.up * randomRot);
                    break;

                case '/':
                    transform.Rotate(Vector3.down * randomRot);
                    break;


                case '[':
                    transformStack.Push(new TransformStack() {
                        pos = transform.position,
                        rot = transform.rotation
                    });
                    break;

                case ']':
                    TransformStack ts = transformStack.Pop();
                    transform.position = ts.pos;
                    transform.rotation = ts.rot;
                    break;

                default:
                    Debug.Log("L-Tree fehlerhaft");
                    break;
            }
        }
    }
}