using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zork;
using Zork.Common;

public class UnityOutputService : MonoBehaviour, IOutputService
{
    [SerializeField]
    private int MaxEntries = 60;

    [SerializeField]
    private Transform OutputTextContainer;

    [SerializeField]
    private TextMeshProUGUI TextLinePrefab;

    [SerializeField]
    private TextMeshProUGUI NewLinePrefab;

    public UnityOutputService() => mEntries = new List<GameObject>();

    public void Clear()
    {
        mEntries.ForEach(entry => Destroy(entry));
        mEntries.Clear();
    }
    public void Write(string value) => ParseAndWriteLine(value);

    public void WriteLine(string value) => ParseAndWriteLine(value);

    private void ParseAndWriteLine(string value)
    {
        string[] delimiters = { "\n" };

        var lines = value.Split(delimiters, StringSplitOptions.None);
        foreach (var line in lines)
        {
            if (mEntries.Count >= MaxEntries)
            {
                var entry = mEntries.First();
                Destroy(entry);
                mEntries.Remove(entry);
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                WriteNewLine();
            }
            else
            {
                WriteTextLine(line);
            }
        }
    }

    private void WriteNewLine()
    {
        var newLine = Instantiate(NewLinePrefab);
        newLine.transform.SetParent(OutputTextContainer, false);
        mEntries.Add(newLine.gameObject);
    }

    private void WriteTextLine(string value)
    {
        var textLine = Instantiate(TextLinePrefab);
        textLine.transform.SetParent(OutputTextContainer, false);
        textLine.text = value;
        mEntries.Add(TextLinePrefab.gameObject);
    }

    private readonly List<GameObject> mEntries;
}