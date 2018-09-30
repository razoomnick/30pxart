function ResizeTool(editor, selectTool){
  this.setCurrent = function(current){
    if (current) {
      var selection = selectTool.getSelection();
      if (selection.yStart >= 0 && selection.xStart >= 0) {
        var command = new CanvasSizeCommand(editor);
        resize(selection);
        command.finish();
        editor.addHistoryPoint(command);
      }
    }
  };

  function resizeCanvas(state, selection) {
    var width = selection.xEnd - selection.xStart + 1;
    var height = selection.yEnd - selection.yStart + 1;
    var newState = new ColorsArray(width, height);
    var oldSize = state.getSize();
    for (var i = 0; i < width; i++) {
      for (var j = 0; j < height; j++) {
        newState.setColor(i, j, state.getColor((selection.xStart + i) % oldSize.w, (selection.yStart + j) % oldSize.h));
      }
    }
    return newState;
  }
  
  function resize(selection) {
    if (selection.yStart >= 0 && selection.xStart >= 0) {
      var canvases = editor.getCanvases();
      for (var i = 0; i < canvases.length; i++) {
        var state = canvases[i].getState();
        var newState = resizeCanvas(state, selection);
        canvases[i].setState(newState);
        canvases[i].updatePreview();
      }
      var grid = editor.getGrid();
      var cellSize = editor.getCellSize();
      var width = Math.abs(selection.xEnd - selection.xStart) + 1;
      var height = Math.abs(selection.yEnd - selection.yStart) + 1;
      grid.resize(cellSize * width, cellSize * height);
    }
  };
}