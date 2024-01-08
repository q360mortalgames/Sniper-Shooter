using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class autotypetext : MonoBehaviour
{

    public float letterPause = 0.2f;
    public AudioClip sound;

    string message;

    // Use this for initialization
    void Start()
    {
        message = GetComponent<Text>().text;
        GetComponent<Text>().text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in message.ToCharArray())
        {
            GetComponent<Text>().text += letter;
            //if (sound)
            //    audio.PlayOneShot(sound);
            yield return 0;
            yield return new WaitForSeconds(0.03f);
        }
    }
}