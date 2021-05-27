using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileManager: MonoBehaviour
{
    #region Fields
    string rootFilePath;
    //DirectoryInfo directoryInfo;


    #endregion Fields

    #region Properties

    #endregion Properties

    // Start is called before the first frame update
    void Start()
    {
        rootFilePath = "C:\\ProgramData\\CSEC Visualizer\\";
        //directoryInfo = new DirectoryInfo(rootFilePath);
        //Directory.CreateDirectory(rootFilePath + "\\Test Folder");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            //directoryInfo.CreateDirectory("\\Test Folder");

            List<string> fileData = new List<string>();
            fileData.Add("Line one. Some new data.");
            fileData.Add("Line two. Some other data.");
            fileData.Add("Line three. Here is a really long line of data that takes up almost half of my screen.");

            WriteFile("Test File", fileData, "Test Folder\\");
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            List<string> fileData = new List<string>();

            fileData = ReadFile("Test File", "Test Folder\\");

            for(int i = 0; i < fileData.Count; i++)
            {
                Debug.Log(fileData[i]);
            }
        }
    }
    /// <summary>
    /// Reads a file with the name _fileName at _filePathExtension. Returns the fileData as an array.
    ///     Lines starting with '#' are ignored.
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_fileData"></param>
    /// <param name="_filePathExtension"></param>
    /// <returns></returns>
    public List<string> ReadFile(string _fileName, string _filePathExtension)
    {
        List<string> fileData = new List<string>();
        string filePath = rootFilePath + _filePathExtension + _fileName;

        try
        {
            // Reads the file and adds aplicable lines to fileData.
            using (StreamReader reader = new StreamReader(filePath))
            {
                while (reader.Peek() > -1)
                {
                    string line = reader.ReadLine();

                    if(line[0] != '#')
                    {
                        fileData.Add(line);
                    }
                }
            }

            Debug.Log("File read successfully!");
            return fileData;
        }
        catch(FileNotFoundException e)
        {
            Debug.Log("The file at filepath " + filePath + " could not be found!");
            Debug.Log(e);
        }

        return null;
    }

    /// <summary>
    /// Returns the UpdateDataPacket from a JSON file.
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_filePathExtension"></param>
    /// <returns></returns>
    public UpdateDataPacket CreateDataFromJSON(string _fileName, string _filePathExtension)
    {
        string filePath = rootFilePath + _filePathExtension + _fileName;
        Debug.Log("JSON File Path: " + filePath);

        string JSONString = null;

        foreach(string line in ReadFile(_fileName, _filePathExtension))
        {
            JSONString += line;
        }

        UpdateDataPacket dataPacket = JsonUtility.FromJson<UpdateDataPacket>(JSONString);

        Debug.Log("Data packet successfully created from JSON.");

        return dataPacket;
    }

    /// <summary>
    /// Writes a file with the name _fileName and content _fileData to a file at _filePathExtension
    /// </summary>
    /// <param name="_fileName"></param>
    /// <param name="_fileData"></param>
    /// <param name="_filePathExtension"></param>
    public void WriteFile(string _fileName, List<string> _fileData, string _filePathExtension)
    {
        string filePath = rootFilePath + _filePathExtension + _fileName;

        // Writes out each index of the array as a line in the file.
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for(int i = 0; i < _fileData.Count; i++)
            {
                writer.WriteLine(_fileData[i]);
            }
        }

        Debug.Log("File " + _fileName + " successfully saved.");
    }

    /// <summary>
    /// Writes update data packets to a json format.
    /// </summary>
    /// <param name="fileName">The new file's name.</param>
    /// <param name="data">The data to convert to json format.</param>
    public void SaveToJSON(string fileName, List<UpdateDataPacket> data)
    {
        string filePath = "C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\" + fileName;

        // First check if the directory exists, or if we need to make it.
        if (Directory.Exists("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\") == false)
        {
            Directory.CreateDirectory("C:\\ProgramData\\CSEC Visualizer\\Infrastructure\\Database\\");
        }

        try
        {
            using (StreamWriter sw = File.CreateText(filePath))
            {
                foreach (UpdateDataPacket packet in data)
                {
                    sw.WriteLine(packet.ConvertToJSON());
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public void GenerateDatabase()
    {
        List<string> directories = new List<string>();


    }

    public bool IsFolderEmpty(string _filePathExtension)
    {

        return false;
    }
}