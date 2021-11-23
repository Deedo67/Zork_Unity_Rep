using System;
using UnityEngine;
using Zork.Common;
using TMPro;

public class UnityInputService : MonoBehaviour, IInputService
{
    [SerializeField]
    private TMP_InputField InputField;

    public event EventHandler<string> InputRecieved;

    private void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
          if (string.IsNullOrWhiteSpace(InputField.text) == false)
          {
              string inputString = InputField.text.Trim().ToUpper();
              InputRecieved?.Invoke(this, inputString);
          }

          InputField.text = string.Empty;
        }
    }
}
