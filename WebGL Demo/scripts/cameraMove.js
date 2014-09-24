var rotation = 0;
var topRotation = 0;
var radius = 100;

document.onkeydown = checkKey; //TODO: Wenn checkKey nirgends manuell aufgerufen wird, lieber als anonyme Funktion
function checkKey(e) {
	//Event von Argument, oder falls undefined, lade window.event
	e = e || window.event;
	
	//wenn window.event nicht undefined ist, und einen keyCode hat, führe die zugehörige Aktion aus
	if(e && e.keyCode) switch(e.keyCode){
	case  87:               radius -= 0.50; break;	// w
	case  83:               radius += 0.50; break;	// s
	case  65:             rotation += 0.04; break;	// a
	case  68:             rotation -= 0.04; break;	// d
	case  69:          topRotation += 0.04; break;	// e
	case  81:          topRotation -= 0.04; break;	// q
	case 100: moveAllBlocks([  0, -1,  0]); break;	// left
	case 105: moveAllBlocks([  0,  0,  1]); break;	// up
	case 102: moveAllBlocks([  0,  1,  0]); break;	// right
	case  99: moveAllBlocks([  0,  0, -1]); break;	// down
	case  98: moveAllBlocks([  1,  0,  0]); break;	// vor
	case 104: moveAllBlocks([ -1,  0,  0]); break;	// zurück
	default:  return; //keine Aktion ausführen
	}
	
	//Positioniere die Kamera neu, weil sich etwas geändert hat
	CalcRotation();
}

//Setzt die x, y und z Koordinaten der Kamera entsprechend ihrer Rotierung
function CalcRotation(){
	nradius = Math.cos(topRotation) * radius;
	camera.position.x = Math.cos(rotation) * nradius;
	camera.position.z = Math.sin(rotation) * nradius;
	camera.position.y = Math.sin(topRotation) * radius;
	camera.lookAt(center);
}