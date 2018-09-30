function DeselectTool(editor, selectTool){
  this.setCurrent = function(current){
    if (current) {
      var command = new SelectCommand(selectTool);
      selectTool.setSelection({ xStart: -1, yStart: -1, xEnd: -1, yEnd: -1 });
      command.finish();
      editor.addHistoryPoint(command);
    }
  };
}