/*
Book Class
*/

using System;
using UnityEditor;
using UnityEngine;

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public class Book : MonoBehaviour
{

    public GameObject Parent; // Parent of pages
    public GameObject BookInterface; // Book interface

    public string bookChoices; // Allows you to choose which book
    private int _currentPage = 0;
    private int _progress = 0;
    private AssetBundle _previousBundle;
    private AssetBundle _currentBundle;
    private AssetBundle _nextBundle;
    public int TotalPages = 5;

    /* Returns the current number of pages loaded in the scene */
    private int TotalPageCount()
    {
        return TotalPages;
    }

    /* Loads the current set of 10 pages from the current asset bundle*/
    private AssetBundle LoadBundle(int pageNumber)
    {

        string bundleCreation = bookChoices + "-" + pageNumber.ToString(); // Creates the bundle name
        Debug.Log(bundleCreation);
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleCreation)); // Gets the bundle
      
        return bundle;
    }
    private void InstantiateObject(int sibIndex, AssetBundle bundle, int pageNumber)
    {
        string str = pageNumber.ToString();
        var page = Instantiate(bundle.LoadAsset<GameObject>(str), BookInterface.transform.position, BookInterface.transform.rotation); // Places the book in the scene
        page.transform.parent = Parent.transform;
        page.transform.Rotate(0, 180, 0);

        if (sibIndex != -1)
        {
            page.transform.SetSiblingIndex(sibIndex);
        }
        

    }

    /* Updates the current page displayed*/
    public void UpdatePages()
    {
        // Makes any page other than current invisible and makes current visible
        foreach (Transform child in transform)
        {
            string name = child.name;
            string num = name.Split('(')[0];
            int index = 0;
            Int32.TryParse(num, out index);
            if (index == _currentPage)
            {
                child.gameObject.SetActive(true);
            }
            else
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private int NextPage(int currentPage) {
        if (currentPage == TotalPageCount() - 1) {
            return 0;
        } else {
            currentPage++;
            return currentPage;
        }
    }

    private int PreviousPage(int currentPage) {
        if (currentPage == 0) {
            return TotalPageCount() - 1;
        } else {
            currentPage--;
            return currentPage;
        }
    }

    /* Goes to the next page */
    public void TurnNextPage()
    {
        _currentPage = NextPage(_currentPage);
        SaveProgress();

        GameObject toDestroy = transform.GetChild(0).gameObject;
        Destroy(toDestroy);
        _previousBundle.Unload(false);

        _previousBundle = _currentBundle;
        _currentBundle = _nextBundle;
        _nextBundle = LoadBundle(NextPage(_currentPage));
        InstantiateObject(-1, _nextBundle, NextPage(_currentPage));
        UpdatePages();
    }

    /* Goes back a page */
    public void TurnBackPage()
    {
        _currentPage = PreviousPage(_currentPage);
        SaveProgress();

        GameObject toDestroy = transform.GetChild(2).gameObject;
        Destroy(toDestroy);
        _nextBundle.Unload(false);

        _nextBundle = _currentBundle;
        _currentBundle = _previousBundle;
        _previousBundle = LoadBundle(PreviousPage(_currentPage));
        InstantiateObject(0, _previousBundle, PreviousPage(_currentPage));
        UpdatePages();
    }

    public void GoToPage(int pageNumber)
    {
        _currentPage = pageNumber;
        SaveProgress();

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        _previousBundle.Unload(false);
        _currentBundle.Unload(false);
        _nextBundle.Unload(false);

        _previousBundle = LoadBundle(PreviousPage(_currentPage));
        InstantiateObject(0, _previousBundle, PreviousPage(_currentPage));
        _currentBundle = LoadBundle(_currentPage);
        InstantiateObject(1, _currentBundle, _currentPage);
        _nextBundle = LoadBundle(NextPage(_currentPage));
        InstantiateObject(2, _nextBundle, NextPage(_currentPage));
        UpdatePages();
    }
    
    /**
     * Save progress.
     */
    private void SaveProgress()
    {
        _progress = _currentPage;
        string currentDir = Directory.GetCurrentDirectory();
        string fileName = "envStatus.txt";
        string fullPath = currentDir + "/" + fileName;
        string bookStatus = "Progress:" + _progress;
        try
        {
            File.WriteAllText(fullPath, bookStatus);
            Debug.Log("Saved!");
        }
        catch (Exception e)
        {
            Debug.Log("Failed to save progress!");
        }
    }
    
    /*
     * Load progress.
     */
    private void LoadProgress()
    {
        // The path of the txt file storing game status.
        string currentDir = Directory.GetCurrentDirectory();
        string fileName = "envStatus.txt";
        string fullPath = currentDir + "/" + fileName;
        try
        {
            int bookStatus;
            // Try reading the progress.
            if (!Int32.TryParse(File.ReadAllText(fullPath).Split(':')[1], out bookStatus))
            {
                // Reset everything.
                _progress = 0;
                _currentPage = 0;
            }
            else
            {
                // Reload game.
                _progress = bookStatus;
                _currentPage = _progress;
            }
        }
        catch (Exception e)
        {
            // Create an empty file.
            File.Create(fullPath);
            _progress = 0;
            _currentPage = 0;
        }
    }


    /* Checks for saved progress and displays pages */
    void Start()
    {
        LoadProgress();
        _previousBundle = LoadBundle(PreviousPage(_currentPage));
        InstantiateObject(0, _previousBundle, PreviousPage(_currentPage));
        _currentBundle = LoadBundle(_currentPage);
        InstantiateObject(1, _currentBundle, _currentPage);
        _nextBundle = LoadBundle(NextPage(_currentPage));
        InstantiateObject(2, _nextBundle, NextPage(_currentPage));
        UpdatePages();
    }

}