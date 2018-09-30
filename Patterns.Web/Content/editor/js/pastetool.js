function PasteTool(editor, copyTool) {
  
  this.setCurrent = function (current) {
    if (current) {
      var stateCopy = copyTool.getStateCopy();
      if (stateCopy) {
        editor.addLayer(stateCopy);
      }
    }
  };
}