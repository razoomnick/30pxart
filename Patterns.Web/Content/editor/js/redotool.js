function RedoTool(editor){
  this.setCurrent = function (current) {
    if (current) {
      editor.redo();
    }
  };
}