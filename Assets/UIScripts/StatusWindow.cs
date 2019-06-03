using UnityEngine;
using UnityEngine.UI;

public class StatusWindow : MonoBehaviour
{
    public Text generationNumber;

    public void SetGenerationNumber(int g)
    {
        this.generationNumber.text = g.ToString();
    }
}
