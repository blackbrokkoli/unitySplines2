using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{

    public int counter = 20;
    public GameObject splineObject1;
    public GameObject splineObject2;
    public GameObject splineObject3;

    //Abhaengig von der Kurve laeuft die Kurve von t = 0 bis
    //t = maxValueProgress den aktuellen Wert der Kurve erhaelt man ueber
    //die Funktion 'float GetMaxProgressValue()' der Klasse 'BSpline'
    public float maxValueProgress;

    public BSpline bspline;

    public float speed = .1f;
    public float step = .01f;

    // create a switch for the method of following
    public bool dumbFollow = false;

    // create a subclass storing a game object and a progress value
    public class GameObjectWrapper
    {
        public GameObject gameObject;
        public float progress;
        public float carSpeed  = 1;
        // public float carSpeed  = Random.Range(0.9f, 1.1f);
    }

    public List<GameObjectWrapper> cloneList = new List<GameObjectWrapper>();


    // create cloned object for amount counter
    public void Start () {
        for (int i = 0; i < counter; i++)
        {
            // create a new GameObjectWrapper
            GameObjectWrapper clone = new GameObjectWrapper();
            
            // create a new GameObject
            // randomly choose between the three spline objects
            int random = Random.Range(0, 3);
            if (random == 0)
            {
                clone.gameObject = Instantiate(splineObject1);
            }
            else if (random == 1)
            {
                clone.gameObject = Instantiate(splineObject2);
            }
            else
            {
                clone.gameObject = Instantiate(splineObject3);
            }
            // add a 'follow' tag to the new GameObject
            clone.gameObject.tag = "follow";
            cloneList.Add(clone);

            // replace the 'MainShader' material with a random color
            // clone.GetComponent<Renderer>().material.color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            

            clone.progress = 1f * bspline.GetMaxProgressValue() / (1f * counter / i);

            clone.gameObject.transform.position =  bspline.GetPoint(clone.progress);
            clone.gameObject.transform.LookAt(bspline.GetPoint(clone.progress + 0.01f));
           
        }
    }

    public void Update () {

        
        if (dumbFollow) {
            for (int i = 0; i < cloneList.Count; i++)
                    {
                        // for every object
                        // first look towards next object in the list, then
                        // if its the end of the list, move towards the first element in the list
                        if (i < cloneList.Count - 1)
                        {
                            cloneList[i].gameObject.transform.LookAt(cloneList[i + 1].gameObject.transform);
                        }
                        else
                        {
                            cloneList[i].gameObject.transform.LookAt(cloneList[0].gameObject.transform);
                        }
                        // next, move towards the next object in the list (or the first one if there is no next object)
                        if (i < cloneList.Count - 1)
                        {
                            cloneList[i].gameObject.transform.position = Vector3.MoveTowards(cloneList[i].gameObject.transform.position, cloneList[i + 1].gameObject.transform.position, speed);
                        }
                        else
                        {
                            cloneList[i].gameObject.transform.position = Vector3.MoveTowards(cloneList[i].gameObject.transform.position, cloneList[0].gameObject.transform.position, speed);
                        }
                    }
        } else {
              for (int i = 0; i < cloneList.Count; i++) {
                  // iterate the progress with regards to delta time
                    cloneList[i].progress += step * Time.deltaTime;
                  // for each object in the list
                    // then look towards the next value on the spline
                    // then move towards the next value on the spline
                    // if progress is greater than maxValueProgress, set progress to 0
                
                    if (cloneList[i].progress > bspline.GetMaxProgressValue())
                    {
                        cloneList[i].progress = 0f;
                    }
                    cloneList[i].gameObject.transform.LookAt(bspline.GetPoint(cloneList[i].progress));
                 
                    cloneList[i].gameObject.transform.position = Vector3.MoveTowards(cloneList[i].gameObject.transform.position, bspline.GetPoint(cloneList[i].progress), cloneList[i].carSpeed);

              }
        }
        
    }
}

