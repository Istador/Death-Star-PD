var colorList = {};
var modelList = {}; //TODO: wird hier nicht verwendet?
var geometryList = {}; //TODO: wird hier nicht verwendet?
var materials =[];
//var material = new THREE.MeshFaceMaterial(materials);

//Gibt den Index des 
function getMaterialIndex(color){
	//Wenn Material für die gewünschte Farbe noch nicht existiert
	if(colorList[color] === undefined){
		//Erstelle ein neues Material
		var uniforms = {myColor: { type: "c", value: new THREE.Color( convertHex(color) )}};
		var itemMaterial = new THREE.ShaderMaterial({
			uniforms: uniforms,
			//TODO: mögliche Optimierung. Wenn sich der Inhalt von den DOM-Elementen nie ändert,
			//lieber in JavaScript cachen, statt jedes mal neu zu laden.
			vertexShader: document.getElementById('2d-vertex-shader').innerHTML,
			fragmentShader: document.getElementById('2d-fragment-shader').innerHTML
		});
		//speichere dieses für später
		materials.push(itemMaterial);
		//und merke den Index in einem assoziativen Array
		colorList[color] = materials.length -1;
	}
	//Index der Farbe zurückgeben
	return colorList[color];
}

//parst einen hexadezimalen String für Farben der Form "#rrggbb" zu einem Integer
function convertHex(hex){
	return parseInt(hex.substr(1), 16);
}