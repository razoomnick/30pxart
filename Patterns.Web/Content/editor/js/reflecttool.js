function ReflectTool(editor, selectTool, isVertical) {
  this.setCurrent = function(current) {
    if (current) {
      var currentCanvases = editor.getCurrentCanvases();
      if (currentCanvases.length != 1) {
        alert('Could not complete your request because no layers are selected');
      } else {
        var currentCanvas = currentCanvases[0];
        var state = currentCanvas.getState().copy();
        var initialState = state.copy();
        var selection = selectTool.getSelection();
        if (isVertical) {
          reflectVertical(state, selection);
        } else {
          reflectHorizontal(state, selection);
        }
        editor.setState(state);
        var command = new CanvasStateCommand(initialState, currentCanvas);
        editor.addHistoryPoint(command);
      }
    }
  };
  
  function reflectHorizontal(state, selection) {
    var xStart = selection.xStart;
    var xEnd = selection.xEnd;
    var extraPixel = (xEnd - xStart) % 2;
    var size = state.getSize();
    if (xEnd - xStart > size.w) {
      xStart += ~~(((xEnd - xStart) % size.w) / 2);
      xEnd = xStart + size.w + extraPixel;
    }
    var dx = xEnd - xStart;
    var dy = Math.min(selection.yEnd - selection.yStart + 1, size.h);
    for (var y = 0; y < dy; y++) {
      for (var x = 0; x < dx / 2; x++) {
        var x1 = (xStart + x) % size.w;
        var x2 = (xStart + dx - x) % size.w;
        var y1 = (selection.yStart + y) % size.h;
        var temp = state.getColor(x2, y1);
        state.setColor(x2, y1, state.getColor(x1, y1));
        state.setColor(x1, y1, temp);
      }
    }
  }
  
  function reflectVertical(state, selection) {
    var yStart = selection.yStart;
    var yEnd = selection.yEnd;
    var extraPixel = (yEnd - yStart) % 2;
    var size = state.getSize();
    if (yEnd - yStart > size.h) {
      yStart += ~~(((yEnd - yStart) % size.h) / 2);
      yEnd = yStart + size.h + extraPixel;
    }
    var dy = yEnd - yStart;
    var dx = Math.min(selection.xEnd - selection.xStart + 1, size.w);
    for (var x = 0; x < dx; x++) {
      for (var y = 0; y < dy / 2; y++) {
        var y1 = (yStart + y) % size.h;
        var y2 = (yStart + dy - y) % size.h;
        var x1 = (selection.xStart + x) % size.w;
        var temp = state.getColor(x1, y2);
        state.setColor(x1, y2, state.getColor(x1, y1));
        state.setColor(x1, y1, temp);
      }
    }
  }
}