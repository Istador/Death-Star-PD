// C# example:
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using System.Collections.Generic;


public class UVGImportWindow : EditorWindow {

	string path = "";
	float scale = 1f / 11f;

	List<Field> fields = new List<Field>();
	bool[] faces = new bool[]{false, false, false, false, false, false};
	bool useFaces = true;
	
	// Add menu named "My Window" to the Window menu
	[MenuItem ("Window/Import UVG")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		/*UVGImportWindow window = (UVGImportWindow)*/ EditorWindow.GetWindow (typeof (UVGImportWindow));
	}
	
	void OnGUI () {
		GUILayout.Label ("Import UVG", EditorStyles.boldLabel);
		//myString = EditorGUILayout.TextField ("Text Field", myString);
		

		//myBool = EditorGUILayout.Toggle ("Toggle", myBool);
		//myFloat = EditorGUILayout.Slider ("Slider", myFloat, -3, 3);
		if(GUILayout.Button("Open File")){
			path = EditorUtility.OpenFilePanel("Choose file to import","/Assets/UVGs","uvg");
		}

		GUILayout.TextArea(path);

		scale = EditorGUILayout.FloatField("Scale: ", scale);

		if(GUILayout.Button("Import")){
			fields.Clear();
			readFile ();
			createMesh();
		}
	}

	private void readFile(){

		StreamReader sr = new StreamReader(path);
		string fileContents = sr.ReadToEnd();
		sr.Close();
		
		string[] lines = fileContents.Split(";\n"[0]); // TODO: \n wird nicht benutzt. gewollt?
		foreach (string line in lines) {
			int x = 0;
			int y = 0;
			int z = 0;
			string c = "";
			bool done = true;
			string[] parts = line.Split(':');
			if(parts.Length >= 2){
				string[] coords = parts[0].Split(',');
				if(coords.Length == 3){
					done = int.TryParse(coords[0], out x);
					done = int.TryParse(coords[1], out y);
					done = int.TryParse(coords[2], out z);
					if(done != true){
						Debug.LogWarning("Could not parse position " + parts[0]);
					}
				}else{
					Debug.LogWarning("Could not parse position " + parts[0]);
					done = false;
				}
				c = parts[1];
			}else{
				Debug.LogWarning("Could not parse block " + line);
				done = false;
			}
			if(parts.Length >= 3){
				char[] fac = parts[2].ToCharArray();
				for(int i = 0; i < 6; i++){
					faces[i] = false;
				}
				foreach(char ch in fac){

					if(ch == '0') faces[0] = true;
					else if(ch == '1') faces[1] = true;
					else if(ch == '2') faces[2] = true;
					else if(ch == '3') faces[3] = true;
					else if(ch == '4') faces[4] = true;
					else if(ch == '5') faces[5] = true;

				}
			}else{
				useFaces = false;
			}
			if(done == true){
				Field f = new Field(new Vector3(x,z,y), 1, c);
				f.Faces = useFaces ? faces : new bool[]{true, true, true, true, true, true};
				fields.Add(f);
			}
		}
	}

	private void createMesh(){
		List<Vector3> verts = new List<Vector3>();

		List<int> triangles = new List<int>();
		List<Color> colors = new List<Color>();
		int counter = 0;

		foreach(Field f in fields){
			//verts
			verts.Add(f.Position);
			verts.Add(f.Position + new Vector3(1,0,0));
			verts.Add(f.Position + new Vector3(1,1,0));
			verts.Add(f.Position + new Vector3(0,1,0));

			verts.Add(f.Position + new Vector3(0,0,1));
			verts.Add(f.Position + new Vector3(1,0,1));
			verts.Add(f.Position + new Vector3(1,1,1));
			verts.Add(f.Position + new Vector3(0,1,1));

			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);
			colors.Add(f.Color);

			//Front
			if(f.Faces[0]){
				triangles.Add(counter + 2);
				triangles.Add(counter + 1);
				triangles.Add(counter + 0);

				triangles.Add(counter + 3);
				triangles.Add(counter + 2);
				triangles.Add(counter + 0);
			}
			//Left
			if(f.Faces[1]){				
				triangles.Add(counter + 4);
				triangles.Add(counter + 3);
				triangles.Add(counter + 0);
				
				triangles.Add(counter + 7);
				triangles.Add(counter + 3);
				triangles.Add(counter + 4);
			}
			//Back
			if(f.Faces[2]){				
				triangles.Add(counter + 5);
				triangles.Add(counter + 7);
				triangles.Add(counter + 4);
				
				triangles.Add(counter + 5);
				triangles.Add(counter + 6);
				triangles.Add(counter + 7);
			}
			//Right
			if(f.Faces[3]){				
				triangles.Add(counter + 5);
				triangles.Add(counter + 2);
				triangles.Add(counter + 6);
				
				triangles.Add(counter + 1);
				triangles.Add(counter + 2);
				triangles.Add(counter + 5);
			}
			//Top
			if(f.Faces[4]){				
				triangles.Add(counter + 3);
				triangles.Add(counter + 7);
				triangles.Add(counter + 2);
				
				triangles.Add(counter + 7);
				triangles.Add(counter + 6);
				triangles.Add(counter + 2);
			}
			//Bottom
			if(f.Faces[5]){				
				triangles.Add(counter + 0);
				triangles.Add(counter + 1);
				triangles.Add(counter + 4);
				
				triangles.Add(counter + 1);
				triangles.Add(counter + 5);
				triangles.Add(counter + 4);
			}
			counter += 8;
		}

		GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		string[] ar = path.Split('/');
		go.name = ar[ar.Length -1];

		Mesh m = new Mesh();
		m.name = go.name;

		m.vertices = verts.ToArray();
		//m.uv = uvs.ToArray();
		m.colors = colors.ToArray();
		
		m.triangles = triangles.ToArray();
		m.RecalculateNormals();

		go.GetComponent<MeshFilter>().mesh = m;
		go.GetComponent<MeshFilter>().sharedMesh = m;
		Material mat = (Material) AssetDatabase.LoadAssetAtPath("Assets/Importer/CubeMaterial.mat", typeof(Material));
		go.renderer.material = mat;
		go.transform.localScale = new Vector3(scale, scale, scale);

		PrefabUtility.CreatePrefab("Assets/UVGs/" + go.name + ".prefab", go);
		AssetDatabase.AddObjectToAsset (m, "Assets/UVGs/" + go.name + ".prefab");
	}


}
#endif