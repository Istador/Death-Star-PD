function makeBlock(x,y,z,c){
	var uniforms = {myColor: { type: "c", value: new THREE.Color( c )}, myTransparenz:{type: "f", value: 1.0}};
	var material = new THREE.ShaderMaterial({
		uniforms: uniforms,
		vertexShader: document.getElementById('2d-vertex-shader').innerHTML,
		fragmentShader: document.getElementById('2d-fragment-shader').innerHTML
	});
	//var material = new THREE.MeshBasicMaterial({"color" : c});
	var geometry = new THREE.BoxGeometry(1,1,1);
	var cube = new THREE.Mesh( geometry, material );
	cube.position = new THREE.Vector3(x, y, z);
	
	scene.add(cube);
	return cube;
}