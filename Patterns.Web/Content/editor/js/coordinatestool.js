function CoordinatesTool(editor){
  var lblCoordinates = document.getElementById('lblCoordinates');
  var lastX;
  var lastY;

  editor.addHandler('mousemove', showCoordinates);
  
  function showCoordinates(x, y) {
    var size = editor.getCanvases()[0].getState().getSize();
    if ((x != lastX) || (y != lastY)) {
      lblCoordinates.innerHTML = ((x % size.w + 1) + ', ' + (y % size.h + 1));
    }
    lastX = x;
    lastY = y;
  }
}