function UndoTool(editor){
  this.setCurrent = function(current) {
    if (current) {
      editor.undo();
    }
  };
}