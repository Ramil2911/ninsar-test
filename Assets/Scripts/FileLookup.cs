using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class FileLookup : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private BlocksController blocksController;
    void Start()
    {
        if (inputField == null)
            inputField = GetComponent<TMP_InputField>();
        
        inputField.text = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.txt")[0];
        blocksController.Read();
    }
}
