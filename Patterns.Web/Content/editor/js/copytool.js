function CopyTool(editor, selectTool) {
  var stateCopy;
  
  this.setCurrent = function (current) {
    if (current) {
      var state = editor.getState();
      var selection = selectTool.getSelection();
      stateCopy = getCopy(state, selection);
    }
  };

  this.getStateCopy = function() {
    return stateCopy;
  };
  
  function getCopy(state, selection) {
    var size = state.getSize();
    var copy = new ColorsArray(size.w, size.h);

    var dx = selection.xEnd - selection.xStart >= size.w
      ? size.w
      : selection.xEnd - selection.xStart + 1;
    var dy = selection.yEnd - selection.yStart >= size.h
      ? size.h
      : selection.yEnd - selection.yStart + 1;
    for (var i = 0; i < dx; i++) {
      for (var j = 0; j < dy; j++) {
        var x = (selection.xStart + i) % size.w;
        var y = (selection.yStart + j) % size.h;
        copy.setColor(x, y, state.getColor(x, y));
      }
    }
    return copy;
  }
}