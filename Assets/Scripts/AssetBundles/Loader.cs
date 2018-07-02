// Makes use of Unity asset bundles to load 10 pages into the viewer at a time.

using System.IO;
using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour
{

    private AssetBundle bundle; // Holds the current bundle 
    public GameObject Book; // Links the book to the active pages
    public GameObject BI; // Links the book interface

    private string currentTextbook = "soci"; // Current Textbook
    private int currentBundle = 0; // Current bundle 

    private int localCurrentPage; // Current page in the bundle
    private int globalCurrentPage; // Current page in the textbook




    private void Start()
    {
        string bundleCreation = currentTextbook + "-" + currentBundle.ToString(); // Creates the bundle name
        bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, bundleCreation)); // Gets the bundle

        // Loads the pages
        for(int i = 0; i < 10; i++){
            string str = i.ToString();
            var page = Instantiate(bundle.LoadAsset<GameObject>(str), BI.transform.position, BI.transform.rotation);
            page.transform.parent = Book.transform; // Makes Book the parent of page
            page.transform.Rotate(0,180,0); // Rotates the page to correct position
        }
        // bundle.Unload(false);
    }
}