function BrushTool(editor, colorTool){
  var buttonsHolder = $('#plhBrushButtons');
  var isCurrent;
  var isInProgress;
  var currentBrush;
  var currentCanvas;
  var initialState;

  var brushes = [
    [[1]],
    [[0, 1, 0], [1, 1, 1], [0, 1, 0]],
    [[0, 1, 1, 1, 0], [1, 1, 1, 1, 1], [1, 1, 1, 1, 1], [1, 1, 1, 1, 1], [0, 1, 1, 1, 0]],
    [[0, 0, 1, 1, 1, 0, 0], [0, 1, 1, 1, 1, 1, 0], [1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1], [1, 1, 1, 1, 1, 1, 1], [0, 1, 1, 1, 1, 1, 0], [0, 0, 1, 1, 1, 0, 0]],
    [[0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0], [0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0], [0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0], [1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1], [0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0], [0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0], [0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0], [0, 0, 1, 1, 1, 0, 0, 0, 1, 1, 1, 0, 0], [0, 1, 1, 1, 0, 1, 0, 1, 0, 1, 1, 1, 0], [1, 1, 1, 0, 1, 1, 0, 1, 1, 0, 1, 1, 1], [0, 0, 0, 1, 1, 1, 0, 1, 1, 1, 0, 0, 0], [0, 0, 0, 1, 1, 0, 0, 0, 1, 1, 0, 0, 0], [0, 0, 0, 1, 0, 0, 0, 0, 0, 1, 0, 0, 0]]
  ];
  
  var brushButtons = [
    new ToolButton('#btnBrush1', null, true, null, disableBrushes),
    new ToolButton('#btnBrush3', null, true, null, disableBrushes),
    new ToolButton('#btnBrush5', null, true, null, disableBrushes),
    new ToolButton('#btnBrush7', null, true, null, disableBrushes),
    new ToolButton('#btnBrush8', null, true, null, disableBrushes)
  ];

  brushButtons[0].setCurrent(true);
  
  editor.addHandler('mousedown', mouseDown);
  editor.addHandler('mousemove', mouseMove);
  editor.addHandler('mouseup', mouseUp);
  editor.addHandler('mouseleave', mouseUp);

  this.setCurrent = function(current) {
    isCurrent = current;
    updateButtonsVisibility();
  };

  function updateButtonsVisibility() {
    var display = isCurrent ? 'block' : 'none';
    buttonsHolder.css('display', display);
  }

  function mouseDown(x, y, options) {
    if (isCurrent) {
      if (options.button == 0) {
        var currentCanvases = editor.getCurrentCanvases();
        if (currentCanvases.length != 1) {
          alert('Could not complete your request because no layers are selected');
        } else {
          currentCanvas = currentCanvases[0];
          initialState = currentCanvas.getState().copy();
          isInProgress = true;
          currentBrush = getCurrentBrush();
          var color = colorTool.getColor();
          paint(x, y, color);
        }
      }
    }
  }

  function getCurrentBrush() {
    var brush = brushes[0];
    for (var i = 0; i < brushButtons.length; i++) {
      if (brushButtons[i].getCurrent()) {
        brush = brushes[i];
      }
    }
    return brush;
  }

  function mouseUp() {
    if (isCurrent && isInProgress) {
      isInProgress = false;
      var command = new CanvasStateCommand(initialState, currentCanvas);
      editor.addHistoryPoint(command);
    }
  }

  function mouseMove(x, y){
    if (isCurrent && isInProgress){
      var color = colorTool.getColor();
      paint(x, y, color);
    }
  }

  function paint(x, y, color) {
    var w = currentBrush.length;
    var h = currentBrush[0].length;
    var offsetX = -~~(w / 2);
    var offsetY = -~~(h / 2);
    for (var i = 0 ; i < w ; i++) {
      for (var j = 0 ; j < h ; j++) {
        if (currentBrush[i][j]) {
          currentCanvas.setColor(x + i + offsetX, y + j + offsetY, color);
        }
      }
    }
  }
  
  function disableBrushes() {
    for (var i = 0; i < brushButtons.length; i++) {
      brushButtons[i].setCurrent(false);
    }
    return true;
  }
}