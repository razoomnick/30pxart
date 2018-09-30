function FillTool(editor, colorTool){
  var isCurrent;
  
  editor.addHandler('click', fillPrimary);

  this.setCurrent = function(current) {
    isCurrent = current;
  };
  
  function fillPrimary(x, y){
    if (isCurrent) {
      var currentCanvases = editor.getCurrentCanvases();
      if (currentCanvases.length != 1) {
        alert('Could not complete your request because no layers are selected');
      } else {
        var currentCanvas = currentCanvases[0];
        var color = colorTool.getColor();
        var state = editor.getState().copy();
        fill(state, x, y, color);
        var command = new CanvasStateCommand(state, currentCanvas);
        editor.addHistoryPoint(command);
      }
    }
  }
  
  function fill(state, x, y, color){
    var oldColor = editor.getColor(x, y);
    var size = state.getSize();
    var changedCells = [];
    var dictionary = {};
    var add = function(x, y){
      if (!dictionary[x % size.w + '_' + y % size.h]){
        dictionary[(x + size.w) % size.w + '_' + (y + size.h) % size.h] = true;
        changedCells.push({ x: (x + size.w) % size.w, y: (y + size.h) % size.h });
      }
    };
    add(x, y);
    for (var i = 0; i < changedCells.length; i++){
      var xx = changedCells[i].x;
      var yy = changedCells[i].y;
      if (state.getColor((xx - 1 + size.w) % size.w, yy) == oldColor){
        add(xx - 1, yy);
      }
      if (state.getColor(xx,(yy - 1 + size.h) % size.h) == oldColor){
        add(xx, yy - 1);
      }
      if (state.getColor((xx + 1) % size.w, yy) == oldColor){
        add(xx + 1, yy);
      }
      if (state.getColor(xx, (yy + 1) % size.h) == oldColor){
        add(xx, yy + 1);
      }
    }
    for (i = 0; i < changedCells.length; i++){
      xx = changedCells[i].x;
      yy = changedCells[i].y;
      editor.setColor(xx, yy, color);
    }
  }
}