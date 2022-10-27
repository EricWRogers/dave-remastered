using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Color_Picker : MonoBehaviour
{
    public List<Material> baseColors;
    public List<Material> destroyedColors;
    private int choice = -1;
    // Start is called before the first frame update
    void Start()
    {
        pickColor();
    }

    public void paintObject(GameObject go, bool destroyed)
    {
        if (go.TryGetComponent<MeshRenderer>(out MeshRenderer temp))
        {
            if (!destroyed)
            {
                temp.material = baseColors[choice];
            }
            else
            {
                temp.material = destroyedColors[choice];
            }
        }
        foreach (Transform child in go.transform)
        {
            if (child.TryGetComponent<MeshRenderer>(out temp))
            {
                Debug.Log("choice" + choice);
                if (!destroyed)
                {
                    temp.material = baseColors[choice];
                }
                else
                {
                    temp.material = destroyedColors[choice];
                }
            }
            else
            {
                foreach (Transform subchild in child.transform)

                    if (subchild.TryGetComponent<MeshRenderer>(out temp))
                    {
                        Debug.Log("choice" + choice);
                        if (!destroyed)
                        {
                            temp.material = baseColors[choice];
                        }
                        else
                        {
                            temp.material = destroyedColors[choice];
                        }
                    }

            }
        }
    }

    public void pickColor()
    {
        choice = -1;
        while (choice < 0 || choice > baseColors.Count - 1)
        {
            choice = (int)Random.Range(0.0f, baseColors.Count + 1);
        }
        paintObject(gameObject, false);
    }
}
