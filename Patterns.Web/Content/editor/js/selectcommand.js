function SelectCommand(selectTool) {
  var initialSelection = selectTool.getSelection();
  var newSelection;

  this.finish = function() {
    newSelection = selectTool.getSelection();
  };

  this.undo = function () {
    selectTool.setSelection(initialSelection);
  };

  this.redo = function() {
    selectTool.setSelection(newSelection);
  };
}