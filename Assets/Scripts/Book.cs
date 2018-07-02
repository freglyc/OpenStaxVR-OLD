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
    public int bundleNumber = 0; // Allows you to choose the bundle number

    private int totalBundles = 2; // Total number of bundles in the textbook
	private int localCurrentPage; // Keeps track of the current page in the asset bundle: 0-9
	private int globalCurrentPage; // Keeps track of the current page in the textbook: 0-x
    private AssetBundle currentBundle; // Holds the current bundle

    /* Loads the current set of 10 pages from the current asset bundle*/
    private AssetBundle LoadBundle()
    {
    	string bundleCreation = bookChoices + "-" + bundleNumber.ToString(); // Creates the bundle name
        AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleCreation)); // Gets the bundle

        // Loads the pages
        for(int i = 0; i < 10; i++){
            int temp = i + bundleNumber*10;
            string str = temp.ToString();
            var page = Instantiate(bundle.LoadAsset<GameObject>(str), BookInterface.transform.position, BookInterface.transform.rotation); // Places the book in the scene
            page.transform.parent = Parent.transform; // Makes Book the parent of page
            page.transform.Rotate(0,180,0); // Rotates the page to correct position
        }
        return bundle;
    }

    /* Updates the current page displayed*/
    public void UpdatePages()
    {
        // Makes any page other than current invisible and makes current visible
        foreach (Transform child in transform)
        {
            int index = child.GetSiblingIndex();
            if (index+bundleNumber*10 != globalCurrentPage)
            {
                child.gameObject.SetActive(false);
            }
            else
            {
                child.gameObject.SetActive(true);
            }
        }
    }

    /* Removes pages from scene */
    public void DestoryPages()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    /* Returns the current number of pages loaded in the scene */
    private int TotalPageCount()
    {
        return transform.childCount;
    }

    /* Goes to the next page */
    public void TurnNextPage()
    {
        /* 
        Checks if on the last page in the current bundle
        If so goes to the next bundle, if not just turns page
        */
        if (localCurrentPage + 1 > TotalPageCount()-1)
        {   
            /* 
            Checks if the bundle is the last bundle in the textbook
            If so goes to first bundle, if not goes to next bundle
            */ 
            if (bundleNumber < totalBundles - 1)
            {
                bundleNumber += 1;
                globalCurrentPage += 1;
            }
            else
            {
                bundleNumber = 0;
                globalCurrentPage = 0;
            }

            localCurrentPage = 0;
            DestoryPages(); // Removes pages from scene
            currentBundle.Unload(false); // Unloads bundle memory
            currentBundle = LoadBundle(); // Loads the next bundle
        }
        else
        {
            localCurrentPage += 1;
            globalCurrentPage += 1;
        }

        UpdatePages();

        Debug.Log(localCurrentPage);
        Debug.Log(globalCurrentPage);
    }

    /* Goes back a page */
    public void TurnBackPage()
    {   
        /* 
        Checks if on the first page in the current bundle
        If so goes to the next bundle, if not just goes back one page
        */
    	if (localCurrentPage - 1 < 0)
        {
            /* 
            Checks if the bundle is the first bundle in the textbook
            If so goes to last bundle, if not goes back one bundle
            */ 
            if (bundleNumber > 0)
            {
                bundleNumber -= 1;
                globalCurrentPage -= 1;
            }
            else
            {
                bundleNumber = totalBundles-1;
                globalCurrentPage = totalBundles*10-1;
            }

            localCurrentPage = 9;
            DestoryPages(); // Removes pages from scene
            currentBundle.Unload(false); // Unloads bundle memory
            currentBundle = LoadBundle(); // Loads the next bundle
        }
        else
        {
            localCurrentPage -= 1;
            globalCurrentPage -= 1;
        }

        UpdatePages();

        Debug.Log(localCurrentPage);
        Debug.Log(globalCurrentPage);
    }


	/* Checks for saved progress and displays pages */
	void Start()
	{
        currentBundle = LoadBundle();
        UpdatePages();
	}
}