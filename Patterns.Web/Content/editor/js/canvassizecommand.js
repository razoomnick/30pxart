function CanvasSizeCommand(editor) {
  var canvases = editor.getCanvases();
  var initialStates = [];
  var newStates = [];
  
  for (var i = 0; i < canvases.length; i++) {
    initialStates.push(canvases[i].getState().copy());
  }

  this.finish = function () {
    for (i = 0; i < canvases.length; i++) {
      newStates.push(canvases[i].getState().copy());
    }
  };

  this.undo = function () {
    for (i = 0; i < canvases.length; i++) {
      canvases[i].setState(initialStates[i]);
      canvases[i].updatePreview();
    }
    updateGrid();
  };

  this.redo = function () {
    for (i = 0; i < canvases.length; i++) {
      canvases[i].setState(newStates[i]);
      canvases[i].updatePreview();
    }
    updateGrid();
  };
  
  function updateGrid() {
    var grid = editor.getGrid();
    var cellSize = editor.getCellSize();
    var size = editor.getCanvases()[0].getState().getSize();
    grid.resize(cellSize * size.w, cellSize * size.h);
  }
}