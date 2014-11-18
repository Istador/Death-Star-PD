var colorList = {};
var geometryList = {};
var materials =[];
var material = new THREE.MeshFaceMaterial(materials);

function getMaterialIndex(color){
	if(colorList[color] == null){
		var uniforms = {color: { type: "c", value: new THREE.Color( convertHex(color) )}, ambient: {type: "v3", value: ambient}};
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

function getGeometry(path){
	if(geometryList[path] == null || typeof(geometryList[path]) === "undefined"){
		loader.load( 'Models/Wald.js', function ( object ) {
			geometryList[path] = object;
		} );
		console.debug("Loaded Graphic");
	}
	return geometryList[path];
}

function convertHex(hex){
    hex = hex.replace('#','0x');
    return parseInt(hex, 16);
}

function hexToVec3(hex){
	var r = parseInt(hex.substring(0,2),16)/255;
	var g = parseInt(hex.substring(2,2),16)/255;
	var b = parseInt(hex.substring(4,2),16)/255;
	return new THREE.Vector3(r,g,b);
}

function myCallback(){
	this.value = "";
	this.onloaded = function(){
		
	}
}

function loadUGVC(file, x, y, z){	
	var s = "";
	var xmlhttp = new XMLHttpRequest();
	var call = new myCallback();
	xmlhttp.onreadystatechange = function(){
		if (xmlhttp.readyState == 4 && xmlhttp.status == 200){
			call.value = buildUGVC(xmlhttp.responseText, x,y,z);
			call.onloaded();
		}
	}
	xmlhttp.open("GET",file,true);
	xmlhttp.send();
	return call;
}

function buildUGVC(s, xa, ya, za){
	var model = new THREE.Geometry;
	var d = 1/13;
	var ar = s.split(";");
	var uvs = [new THREE.Vector2(0.0, 0.0), new THREE.Vector2(1.0, 0.0), new THREE.Vector2(1.0,1.0)];
	var uvs2 = [new THREE.Vector2(0.0, 0.0), new THREE.Vector2(0.0, 1.0), new THREE.Vector2(1.0,1.0)];
	for(var i = 0; i < ar.length; i++){
		if(ar[i] != ""){
			var line = ar[i].split(":");
			var c = line[1];
			var p = line[0].split(",");
			var f = line[2].split("");
			var x = parseInt(p[0]);
			var y = parseInt(p[1]);
			var z = parseInt(p[2]);
			
			var geo = new THREE.Geometry();
			geo.vertices.push(new THREE.Vector3(0,0,0));
			geo.vertices.push(new THREE.Vector3(d,0,0));
			geo.vertices.push(new THREE.Vector3(0,d,0));
			geo.vertices.push(new THREE.Vector3(0,0,d));
			geo.vertices.push(new THREE.Vector3(d,d,0));
			geo.vertices.push(new THREE.Vector3(0,d,d));
			geo.vertices.push(new THREE.Vector3(d,0,d));
			geo.vertices.push(new THREE.Vector3(d,d,d));
			
			for(var j = 0; j < f.length; j++){
				if(f[j] == "1"){
					//links
					geo.faces.push(new THREE.Face3(0,1,3));
					geo.faces.push(new THREE.Face3(3,1,6));
				}else if(f[j] == "5"){
					//unten
					geo.faces.push(new THREE.Face3(2,1,0));
					geo.faces.push(new THREE.Face3(1,2,4));
				}else if(f[j] == "3"){
					//rechts
					geo.faces.push(new THREE.Face3(5,7,4));
					geo.faces.push(new THREE.Face3(5,4,2));
				}else if(f[j] == "4"){
					//oben
					geo.faces.push(new THREE.Face3(5,6,7));
					geo.faces.push(new THREE.Face3(5,3,6));
				}else if(f[j] == "0"){
					//vorne
					geo.faces.push(new THREE.Face3(1,7,6));
					geo.faces.push(new THREE.Face3(1,4,7));
				}else if(f[j] == "2"){
					//hinten
					geo.faces.push(new THREE.Face3(3,2,0));
					geo.faces.push(new THREE.Face3(3,5,2));
				}
				geo.faceVertexUvs[0].push(uvs);
				geo.faceVertexUvs[0].push(uvs2);
			}
			
			for ( var j = 0; j < geo.faces.length; j ++ ) {
				geo.faces[j].materialIndex = getMaterialIndex(c);
			}
			var cube = new THREE.Mesh( geo );
			
			cube.position = new THREE.Vector3(d*x + xa, d*y + ya, d*z +za);
			THREE.GeometryUtils.merge(model, cube);
		}
	}
	return new THREE.Mesh(model, new THREE.MeshFaceMaterial(materials));
}

function rgb2hex(rgb) {
    if (/^#[0-9A-F]{6}$/i.test(rgb)) return rgb;

    rgb = rgb.match(/^rgb\((\d+),\s*(\d+),\s*(\d+)\)$/);
    function hex(x) {
        return ("0" + parseInt(x).toString(16)).slice(-2);
    }
    return hex(rgb[1]) + hex(rgb[2]) + hex(rgb[3]);
}