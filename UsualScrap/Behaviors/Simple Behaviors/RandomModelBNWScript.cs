using System.Collections.Generic;
using UnityEngine;

namespace UsualScrap.Behaviors
{
    public class RandomModelBNWScript : PhysicsProp
    {
        public bool changeColor;

        List<Transform> transformList = new List<Transform>();

        List<GameObject> gameObjects = new List<GameObject>();

        public void Awake()
        {

            foreach (Transform t in GetComponentsInChildren<Transform>())
            {
                if (t.name.Contains("Body"))
                {
                    transformList.Add(t);
                }
                else
                {
                    continue;
                }
            }

            foreach (Transform h in transformList)
            {
                gameObjects.Add(h.gameObject);
            }

            int random1 = Random.Range(0, gameObjects.Count);

            int currentobject1 = 0;

            int random2 = Random.Range(1, 3);

            foreach (GameObject gameobject in gameObjects)
            {
                if (gameobject.GetComponent<MeshRenderer>() != null)
                {
                    if (currentobject1 == random1)
                    {
                        gameobject.SetActive(true);
                    }
                    else
                    {
                        gameobject.SetActive(false);
                    }

                    if (random2 == 1 && changeColor)
                    {
                        gameobject.GetComponent<MeshRenderer>().material.color = Color.white;
                    }
                    
                }
                currentobject1++;
            }
        }
    }
}
