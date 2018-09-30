function MoveTool(editor, selectTool){
  var isCurrent;
  var isInProgress;
  var xStart;
  var yStart;
  var initialState;
  var initialSelection;
  var moveAll;
  var lastX;
  var lastY;

  editor.addHandler('mousedown', mouseDown);
  editor.addHandler('mousemove', mouseMove);
  editor.addHandler('mouseup', mouseUp);
  editor.addHandler('mouseleave', mouseUp);

  function getArrowKeyHandler(dx, dy) {
    return function() {
      mouseDown(0, 0);
      mouseMove(dx, dy);
      mouseUp();
    };
  }

  this.setCurrent = function (current) {
    isCurrent = current;
    if (isCurrent) {
      Mousetrap.bind('up', getArrowKeyHandler(0, -1));
      Mousetrap.bind('down', getArrowKeyHandler(0, 1));
      Mousetrap.bind('right', getArrowKeyHandler(1, 0));
      Mousetrap.bind('left', getArrowKeyHandler(-1, 0));
    } else {
      Mousetrap.unbind('up');
      Mousetrap.unbind('down');
      Mousetrap.unbind('right');
      Mousetrap.unbind('left');
    }
  };

  function mouseDown(x, y) {
    if (isCurrent) {
      isInProgress = true;
      xStart = x;
      yStart = y;
      lastX = x;
      lastY = y;
      initialState = editor.getState().copy();
      var size = initialState.getSize();
      initialSelection = selectTool.getSelection();
      if (initialSelection.xStart == -1 && initialSelection.xEnd == -1) {
        initialSelection = {
          xStart: 0,
          yStart: 0,
          xEnd: size.w,
          yEnd: size.h
        };
        moveAll = true;
      } else {
        moveAll = false;
      }
    }
  }

  function mouseUp() {
    if (isCurrent && isInProgress) {
      isInProgress = false;
      var currentCanvas = editor.getCurrentCanvases()[0];
      var command = new CanvasStateCommand(initialState, currentCanvas);
      editor.addHistoryPoint(command);
    }
  }

  function mouseMove(x, y) {
    if (isCurrent && isInProgress) {
      if (lastX != x || lastY != y) {
        var newSelection = {
          xStart: initialSelection.xStart + x - xStart,
          yStart: initialSelection.yStart + y - yStart,
          xEnd: initialSelection.xEnd + x - xStart,
          yEnd: initialSelection.yEnd + y - yStart,
        };
        var newState = moveChunk(initialState, initialSelection, x - xStart, y - yStart);
        editor.setState(newState);
        if (!moveAll) {
          selectTool.setSelection(newSelection);
        }
        lastX = x;
        lastY = y;
      }
    }
  }

  function moveChunk(state, selection, dx, dy) {
    var size = state.getSize();
    dx = (size.w * 1000 + dx) % size.w;
    dy = (size.w * 1000 + dy) % size.h;
    var newState = state.copy();
    var chunk = getChunk(state, selection);
    var chunkSize = chunk.getSize();
    for (var i = 0; i < chunkSize.w; i++) {
      for (var j = 0; j < chunkSize.h; j++) {
        var x = (selection.xStart + i + size.w * 1000) % size.w;
        var y = (selection.yStart + j + size.h * 1000) % size.h;
        newState.setColor(x, y, 0);
      }
    }
    for (i = 0; i < chunkSize.w; i++) {
      for (j = 0; j < chunkSize.h; j++) {
        x = (selection.xStart + dx + i + size.w * 1000) % size.w;
        y = (selection.yStart + dy + j + size.h * 1000) % size.h;
        var color = chunk.getColor(i, j);
        if (color & 0xff) {
          newState.setColor(x, y, color);
        }
      }
    }
    return newState;
  }

  function getChunk(state, selection) {
    var size = state.getSize();
    var dx = selection.xEnd - selection.xStart >= size.w
      ? size.w
      : selection.xEnd - selection.xStart + 1;
    var dy = selection.yEnd - selection.yStart >= size.h
      ? size.h
      : selection.yEnd - selection.yStart + 1;
    var chunk = new ColorsArray(dx, dy);
    for (var i = 0; i < dx; i++) {
      for (var j = 0; j < dy; j++) {
        var x = (selection.xStart + i + size.w * 1000) % size.w;
        var y = (selection.yStart + j + size.h * 1000) % size.h;
        chunk.setColor(i, j, state.getColor(x, y));
      }
    }
    return chunk;
  }
}