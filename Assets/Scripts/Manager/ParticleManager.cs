using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParticleManager : MonoBehaviour
{
    public GameObject[] particleList;
    public string particleName;
    public bool isParticle;
    public List<GameObject> curParticleList;
    public List<string> curParticleNameList;
    public GameObject clickParticle;
    // Start is called before the first frame update
    private void Awake() {
        particleList = Resources.LoadAll<GameObject>("Images/Effect");
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0)){
            // GameObject a = Instantiate(clickParticle);
            // Debug.Log(Input.mousePosition);
            // Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            // a.transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // a.transform.SetParent(this.transform, false);
            // a.transform.localScale = new Vector3(1, 1, 1);

            Vector3 clickPosition = 
            new Vector3(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height/2, Input.mousePosition.z + 1);
            
            GameObject par = Instantiate(clickParticle, clickPosition, Quaternion.identity);
            par.transform.SetParent(this.transform, false);
        }
    }

    public void ParticleOn(string name){
        Debug.Log(name);
        GameObject a = Instantiate(FindParticle(name), parent: this.transform);

        isParticle = true;
        curParticleList.Add(a);
        curParticleNameList.Add(a.name);
    }

    public void ParticleOff(string name){
        for(int i = curParticleList.Count - 1; i >= 0; i--){
            GameObject part = curParticleList[i];

            if(part.name == name + "(Clone)"){
                    curParticleList.Remove(part);
                    curParticleNameList.Remove(part.name);
                    Destroy(part);
            }
        }

        if(curParticleList.Count == 0){
            isParticle = false;
        }
    }

    GameObject FindParticle(string name){
        foreach (GameObject i in particleList)
        {
            if(i.name == name){
                return i;
            }
        }

        Debug.LogFormat(this, "{0}이라는 파티클을 찾을 수 없습니다.", name);
        return null;
    }
}
