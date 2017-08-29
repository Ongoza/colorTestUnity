using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class myInit : MonoBehaviour {
	public Camera cam;
	private float[][] arrLoc = new float[8][];
	private float[][] arrColor = new float[8][];
	private GameObject[] listCards = new GameObject[8];
	private int[] selNames = new int[8];
	private List<int> animateTransparent = new List<int>();
	private int selNamesCounter;
	private GameObject root;
	private TextMesh textObject1;
	private TextMesh textObject2;
	private TextMesh textObject3;

	// Use this for initialization
	void Start () {
		textObject1 = GameObject.Find("myLog1").GetComponent<TextMesh>();
		textObject2 = GameObject.Find("myLog2").GetComponent<TextMesh>();
		textObject3 = GameObject.Find("myLog3").GetComponent<TextMesh>();
		textObject1.text = "Start";
		root = new GameObject();
		root.name = "FFaa";
		root.transform.position = new Vector3(0, 0, -60f);
//		root.transform.rotation.Set(0, 0, 1,0.2f);
		root.transform.rotation = Quaternion.AngleAxis(-90, Vector3.left);
		createArrays ();
		//Debug.Log("Hello");
		createBaseObjs();
	}

		private void createArrays(){
			// {"gray","blue","green","red","yellow","purple","brown","black" };
			arrColor[0] = new float[]{171, 171, 171};
			arrColor[1] = new float[]{0, 0, 128};
			arrColor[2] = new float[]{3, 114, 21};
			arrColor[3] = new float[]{246, 6, 22};
			arrColor[4] = new float[]{251, 251, 2};
			arrColor[5] = new float[]{139, 0, 139};
			arrColor[6] = new float[]{139, 69, 19};
			arrColor[7] = new float[]{0, 0, 0};
			float[] xLoc = {-3f,-1f,1f,3f};
			float[] yLoc = {1f,-1f};
			for (int i = 0; i < 2; i++){ int h=4;
				for (int j = 0; j < h; j++){ int k = i*h+j;
					arrLoc[k]=new float[]{xLoc[j],yLoc[i]};
				}}
		}


	private void createBaseObjs(){
		//Debug.Log("start create cards 2");
		for (int i=0;i<8;i++) {
			createCard(i,new Color(arrColor[i][0]/255f,arrColor[i][1]/255f,arrColor[i][2]/255f,1f));
			listCards[i].transform.position = root.transform.position;
			listCards[i].transform.rotation = root.transform.rotation;
			listCards[i].transform.Translate (arrLoc[i][0]*10,0,arrLoc[i][1]*10);
		}
	}

	private void createCard(int i, Color color){
		GameObject item = GameObject.CreatePrimitive(PrimitiveType.Plane);
		//Debug.Log("card="+"i "+color+" "+position[0]*10+" "+position[1]*10);
		item.GetComponent<Renderer>().material.SetFloat("_Mode", 2.0f);
		item.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
//		item.GetComponent<Renderer>().material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
		item.GetComponent<Renderer>().material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
		item.GetComponent<Renderer>().material.SetInt("_ZWrite", 0);
		item.GetComponent<Renderer>().material.DisableKeyword("_ALPHATEST_ON");
		item.GetComponent<Renderer>().material.EnableKeyword("_ALPHABLEND_ON");
		item.GetComponent<Renderer>().material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
		item.GetComponent<Renderer>().material.renderQueue = 3000;
		item.GetComponent<Renderer>().material.SetColor("_Color", color);
		item.name = i.ToString();
		item.transform.localScale =new Vector3(1.3f, 1.3f, 1.3f);
		item.AddComponent<BoxCollider>();
		listCards[i] = item;
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Escape)){Application.Quit();}
		if (Input.GetMouseButtonDown (0)) { 
			textObject1.text = "Click";
			Ray ray = cam.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				textObject2.text = "Click id:"+hit.collider.gameObject.name;
				//	Debug.Log (hit.collider.gameObject.name);
				int i = int.Parse(hit.collider.gameObject.name);
				//	animateTransparent.Add (i);
				Color colorStart = listCards [i].GetComponent<Renderer> ().material.color;
				//Debug.Log("animate "+i+" "+colorStart);
				Color colorEnd = new Color (colorStart.r, colorStart.g, colorStart.b, 0.01f);
//				listCards [i].GetComponent<Renderer> ().material.color = colorEnd;
				StartCoroutine (FadeTo (i, 1.0f));
			}
		}
	}
		
	IEnumerator FadeTo(int i, float duration){
		//float duration = 2; // This will be your time in seconds.
		float smoothness = 0.05f;
		Color  colorStart = listCards [i].GetComponent<Renderer> ().material.color;
		Color  colorEnd = new Color (colorStart.r,colorStart.g,colorStart.b,0);
		float progress = 0; 
		float increment = smoothness/duration; //The amount of change to apply.
		while(progress < 1){
			//Debug.Log("animate 33="+listCards [i].GetComponent<Renderer> ().material.color.a);
			progress += increment;
			textObject3.text = "Click 3 i:"+progress;
//			item.GetComponent<Renderer>().material.color = Color.Lerp(colorStart,colorEnd,progress);
			listCards [i].GetComponent<Renderer> ().material.color = Color.Lerp(colorStart,colorEnd,progress);
			yield return new WaitForSeconds (smoothness);
			};
		Destroy(listCards[i]);
		yield return null;
		}



}
	