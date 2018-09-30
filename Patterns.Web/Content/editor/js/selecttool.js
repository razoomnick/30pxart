function SelectTool(editor){
  var isCurrent;
  var selection = getDefaultSelection();
  var isInProgress = false;
  var xStart;
  var yStart;
  var copyTool = new CopyTool(editor, this);
  var command;

  var actionButtons = {
    btnReflectHorizontal: new ToolButton('#btnReflectHorizontal', new ReflectTool(editor, this), false, ['ctrl+left', 'ctrl+right']),
    btnReflectVertical: new ToolButton('#btnReflectVertical', new ReflectTool(editor, this, true), false, ['ctrl+down', 'ctrl+up']),
    btnCopy: new ToolButton('#btnCopy', copyTool, false, ['ctrl+c']),
    btnPaste: new ToolButton('#btnPaste', new PasteTool(editor, copyTool), false, ['ctrl+v']),
    btnDeselect: new ToolButton('#btnDeselect', new DeselectTool(editor, this), false, ['ctrl+d']),
    btnDelete: new ToolButton('#btnDelete', new DeleteTool(editor, this), false, ['del'])
  };

  var mouseDownHandler = getMouseDownHandler(this);
  editor.addHandler('mousedown', mouseDownHandler);
  editor.addHandler('mouseup', mouseUp);
  editor.addHandler('mouseleave', mouseUp);
  editor.addHandler('mousemove', mouseMove);
  updateButtonsVisibility();
  
  function getMouseDownHandler(selectTool) {
    return function (x, y) {
      mouseDown(selectTool, x, y);
    };
  }

  function mouseDown(selectTool, x, y){
    if (isCurrent){
      isInProgress = true;
      command = new SelectCommand(selectTool);
      selection = getDefaultSelection();
      xStart = x;
      yStart = y;
      setSelection(getSelection());
    }
  }
  
  function getDefaultSelection() {
    return { xStart: -1, yStart: -1, xEnd: -1, yEnd: -1 };
  }
  
  function mouseMove(x, y){
    if (isCurrent && isInProgress) {
      selection.xStart = xStart;
      selection.yStart = yStart;
      selection.xEnd = x;
      selection.yEnd = y;
      setSelection(getSelection());
    }
  }

  function mouseUp(x, y){
    if (isCurrent && isInProgress){
      isInProgress = false;
      setSelection(getSelection());
      command.finish();
      editor.addHistoryPoint(command);
    }
  }

  function updateButtonsVisibility() {
    var display = isCurrent ? 'block' : 'none';
    $('#plhSelectionButtons').css('display', display);
  }
  
  this.setCurrent = function(current){
    isCurrent = current;
    updateButtonsVisibility();
  };
  
  function getSelection() {
    var copy = {
      xStart: selection.xStart,
      yStart: selection.yStart,
      xEnd: selection.xEnd,
      yEnd: selection.yEnd
    };
    if (copy.xStart > copy.xEnd) {
      copy.xStart = selection.xEnd;
      copy.xEnd = selection.xStart;
    }
    if (copy.yStart > copy.yEnd) {
      copy.yStart = selection.yEnd;
      copy.yEnd = selection.yStart;
    }
    return copy;
  }

  this.getSelection = getSelection;
  
  function setSelection(newSelection){
    selection = newSelection;
    editor.select(getSelection());
  };

  this.setSelection = setSelection;
  
  this.deselect = function(){
    selection = { xStart: -1, yStart: -1, xEnd: -1, yEnd: -1 };
    editor.select(getSelection());
  };
}