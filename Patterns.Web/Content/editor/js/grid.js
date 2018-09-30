function Grid(holder, xStep, yStep, dashLength, spaceLength, offset) {
  var canvasTools = new CanvasTools();
  var context = canvasTools.createContext(holder);
  context.canvas.style.zIndex = 1000;

  drawGrid();

  function drawGrid() {
    context.clearRect(0, 0, context.canvas.width, context.canvas.height);
    canvasTools.drawCustomGrid(context, xStep, yStep, '#cccccc', dashLength, spaceLength, offset);
  }

  this.setVisibility = function(visible) {
    context.canvas.style.visibility = visible ? 'visible' : 'hidden';
  };

  this.resize = function(newXStep, newYStep) {
    xStep = newXStep;
    yStep = newYStep;
    drawGrid();
  };
}