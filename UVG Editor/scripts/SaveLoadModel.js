		function exportModel(){
			//0 = vorne, 1 = links , 2 = hinten, 3 = rechts, 4 = oben, 5 = unten
			var out = "";
			var keys = Object.keys(blocks);
			for(var i = 0; i < keys.length; i++){
				out += keys[i].toString() + ":" + colors[keys[i]].toString(16) + ":";
				var k = keys[i].split(",");
				k[0] = parseInt(k[0]);
				k[1] = parseInt(k[1]);
				k[2] = parseInt(k[2]);
				
				var check = false;
				if( typeof(getBlock(k[0] +1, k[1], k[2])) === "undefined" || getBlock(k[0] +1, k[1], k[2]) == null){
					out += "0"
					check = true;
				}
				if( typeof(getBlock(k[0], k[1] - 1, k[2])) === "undefined" || getBlock(k[0] , k[1]-1, k[2]) == null){
					out += "1"
					check = true;
				}
				if( typeof(getBlock(k[0] -1, k[1], k[2])) === "undefined" || getBlock(k[0] -1, k[1], k[2]) == null){
					out += "2"
					check = true;
				}
				if( typeof(getBlock(k[0], k[1] +1, k[2])) === "undefined" || getBlock(k[0] , k[1]+1, k[2]) == null){
					out += "3"
					check = true;
				}
				if( typeof(getBlock(k[0], k[1], k[2] +1)) === "undefined" || getBlock(k[0] , k[1], k[2]+1) == null){
					out += "4"
					check = true;
				}
				if( typeof(getBlock(k[0], k[1], k[2]-1)) === "undefined" || getBlock(k[0] , k[1], k[2]-1) == null){
					out += "5"
					check = true;
				}
				if(check == false){
					out += "x";
				}
				out += ";\n";
			}
			document.getElementById("safeCode").value = out;
		}
		
		function safe(){
			var out = autoSafe();
			document.getElementById("safeCode").value = out;
			document.getElementById("safeCodeHolder").style.display = "block";
			getImageData = true;
			render();
			
			var xmlhttp = new XMLHttpRequest();
			xmlhttp.open("POST","/saveModel",true);
			xmlhttp.setRequestHeader("Content-type","application/json");
			var json = {"content" : out, "name" : document.getElementById("name").value, "picture": imgData};
			if (modelId != "null"){
				json["id"] = modelId;
			}
			xmlhttp.send(JSON.stringify(json));
			return out;
		}
		
		function autoSafe(){
			var out = "camPos:" + rotation + "," + radius + "," + camera.position.z + "," + center.x + "," + center.y + "," + center.z + ";\n";
			var keys = Object.keys(blocks);
			for(var i = 0; i < keys.length; i++){
				if(colors[keys[i]] != null){
					var col = colors[keys[i]].toString(16);
					while(col.length < 6){
						col = "0" + col;
					}
					out += keys[i].toString() + ":" + col + ";\n";
				}
			}
			return out;
		}
		
		function load(){
			autoLoad(document.getElementById("safeCode").value);
			actionPerformed()
		}
		
		function autoLoad(s){			
			document.getElementById("safeCode").value = s;
			if( s != null){
				reset();
				var ar = s.split(";\n");
				for(var i = 0; i < ar.length; i++){
					if(ar[i] != ""){
						var line = ar[i].split(":");
						if(line[0] == "camPos"){
							// var p = line[1].split(",");
							// rotation = parseFloat(p[0]);
							// radius = parseFloat(p[1]);
							// camera.position.z = parseInt(p[2]);
							// center = new THREE.Vector3(parseInt(p[3]), parseInt(p[4]), parseInt(p[5]));
							// CalcRotation();
							// camera.lookAt(center);
						}else{
							var c = parseInt(line[1],16);
							var p = line[0].split(",");
							var x = parseInt(p[0]);
							var y = parseInt(p[1]);
							var z = parseInt(p[2]);
							
							//console.debug("Load: " + x + ", " + y + ", " + z);
							addBlock(x,y,z,c);
						}
					}
				}
			}	
		}
		
		function reset(){
			var keys = Object.keys(blocks);
			for(var i = 0; i < keys.length; i++){
				var k = keys[i].split(",");
				removeBlock(k[0], k[1], k[2]);
			}
			colors = {};
			blocks = {};
			cubeModeSelections = [];
		}
		
		function download(filename) {
		  var text = document.getElementById("safeCode").value;
		  var pom = document.createElement('a');
		  pom.setAttribute('href', 'data:text/plain;charset=utf-8,' + encodeURIComponent(text));
		  pom.setAttribute('download', filename);
		  pom.click();
		}