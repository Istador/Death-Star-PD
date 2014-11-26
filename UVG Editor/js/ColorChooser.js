	var colourTable = {};
	
	function getMousePos(canvas, evt) {
        var rect = canvas.getBoundingClientRect();
		var x = evt.clientX - rect.left;
		var y = evt.clientY - rect.top;
		if(x >= 0 && y >= 0){
			console.debug({x: x, y: y});
			return {x: x, y: y};
		}else{
			return null;
		}
      }
	  
      function drawColorSquare(canvas, color, imageObj) {		
		document.getElementById("colorTextfield").style.backgroundColor = color.getCSSHexadecimalRGB();
		document.getElementById("colorTextfield").value = color.getCSSHexadecimalRGB();
		colorHex = color.getCSSHexadecimalRGB();
		color = parseInt(color.getCSSHexadecimalRGB(),16);

      }
	  
      function initColorChooser() {
		console.debug("initializing...");
        var canvas = document.getElementById('colorCanvas');
        var context = canvas.getContext('2d');
        var mouseDown = false;
		var imageObj = canvas;

        context.lineWidth = 1;

        canvas.addEventListener('mousedown', function() {
          mouseDown = true;
        }, false);

        canvas.addEventListener('mouseup', function() {
          mouseDown = false;
        }, false);

        canvas.addEventListener('mousemove', function(evt) {
          var mousePos = getMousePos(canvas, evt);
          var color = undefined;

          if(mouseDown && mousePos !== null) {
            var x = mousePos.x;
            var y = mousePos.y;
			//var imageData = context.getImageData(x, y, 1, 1);
            //var data = imageData.data;
			
            //var red = data[0];
            //var green = data[1];
            //var blue = data[2];
            //var color = 'rgb(' + red + ',' + green + ',' + blue + ')';
            var color = colourTable[x + "," + y];//new RGBColour(red, green, blue);
            drawColorSquare(canvas, color, imageObj);
          }
        }, false);
		
		
        var w = canvas.width;
		var h = canvas.height;
		console.debug("Canvas size: " + w + ";" + h );
		
		var rgb;
		
		var colour = new HSVColour(0, 125, 125);

		for(var y = 0; y < h; y++){
			var hue = y * 360 / h;
			for(var x = 0; x < w; x++){
				if(x > w - 10){
					colour = new HSVColour(0, 0, y);
					colourTable[x + "," + y] = colour;
				}else if(x < w/2){
					colour = new HSVColour(hue, x*100/(w/2), 100);
					colourTable[x + "," + y] = colour;
				}else{
					colour = new HSVColour(hue, 100, 100 -((x- w/2)*100/(w/2)));
					colourTable[x + "," + y] = colour;
				}
				
				//colour.s = x * 255 / w;
				
				rgb = colour.getRGB();
				context.strokeStyle = "rgb(" + Math.floor(rgb["r"]) + "," + Math.floor(rgb["g"]) + "," + Math.floor(rgb["b"]) + ")";
				context.fillStyle = "rgb(" + Math.floor(rgb["r"]) + "," + Math.floor(rgb["g"]) + "," + Math.floor(rgb["b"]) + ")";
				context.fillRect(x,y,1,1);
			}
			//console.debug(rgb["g"]);
		}
        //drawColorSquare(canvas, 'white', imageObj);
      }

