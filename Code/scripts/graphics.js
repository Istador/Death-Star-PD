var colorList = {};
var modelList = {};
var geometryList = {};
var materials =[];
//var material = new THREE.MeshFaceMaterial(materials);

function getMaterialIndex(color){
	if(colorList[color] == null){
		var uniforms = {myColor: { type: "c", value: new THREE.Color( convertHex(color) )}};
		var itemMaterial = new THREE.ShaderMaterial({
			uniforms: uniforms,
			vertexShader: document.getElementById('2d-vertex-shader').innerHTML,
			fragmentShader: document.getElementById('2d-fragment-shader').innerHTML
		});
		materials.push(itemMaterial);
		colorList[color] = materials.length -1;
		return materials.length -1;
	}else{
		return colorList[color];
	}
}

function convertHex(hex){
    hex = hex.replace('#','0x');
    return parseInt(hex, 16);
}