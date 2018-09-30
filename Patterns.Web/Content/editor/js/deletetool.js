function DeleteTool(editor, selectTool) {  
  this.setCurrent = function (current) {
    if (current) {
      var currentCanvases = editor.getCurrentCanvases();
      if (currentCanvases.length != 1) {
        alert('Could not complete your request because no layers are selected');
      } else {
        var currentCanvas = currentCanvases[0];
        var state = currentCanvas.getState().copy();
        var initialState = state.copy();
        var selection = selectTool.getSelection();
        var size = state.getSize();
        for (var i = selection.xStart; i <= selection.xEnd; i++) {
          for (var j = selection.yStart; j <= selection.yEnd; j++) {
            var x = i % size.w;
            var y = j % size.h;
            state.setColor(x, y, 0);
          }
        }
        editor.setState(state);
        var command = new CanvasStateCommand(initialState, currentCanvas);
        editor.addHistoryPoint(command);
      }
    }
  };
}