using UnityEngine;
using System.Collections;

public class TextSample : MonoBehaviour {
	
	TextMesh textMesh;
	
	// Use this for initialization
	void Start () {
		textMesh = (TextMesh)GetComponent("TextMesh");
	}
	
	// Update is called once per frame
	void Update () {
		textMesh.text =  "public class Hello {\n"
						+"  public static void main (String[] args) {\n"
						+"    System.out.println(\"Hell World\");\n"
						+"  }\n"
						+"}";
	}
}
