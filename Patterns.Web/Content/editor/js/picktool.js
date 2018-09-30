function PickTool(canvas, colorTool){
  var isCurrent;
  
  canvas.addHandler('click', pickColor);

  this.setCurrent = function(current) {
    isCurrent = current;
  };
  
  function pickColor(x, y){
    if (isCurrent){
      var color = canvas.getColor(x, y);
      if (color & 0xff) {
        colorTool.setColor(color);
      }
    }
  }
}