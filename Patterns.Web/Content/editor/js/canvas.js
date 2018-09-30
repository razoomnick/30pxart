function Canvas(holder, previewHolder, width, height, cellSize, editor) {
  var canvasPreview = new CanvasPreview(previewHolder, this);
  var canvasTools = new CanvasTools();
  var context = canvasTools.createContext(holder);
  var colors = new ColorsArray(width, height);
  var visibility = true;

  this.getState = function () {
    return colors;
  };

  this.setState = function (newState, forced) {
    var newSize = newState.getSize();
    var oldSize = colors.getSize();
    if (newSize.w != oldSize.w || newSize.h != oldSize.h) {
      colors = new ColorsArray(newSize.w, newSize.h);
      context.clearRect(0, 0, context.canvas.width, context.canvas.height);
    }
    var rawColors = colors.getColors();
    var newRawColors = newState.getColors();
    for (var i = 0; i < newSize.w; i++) {
      for (var j = 0; j < newSize.h; j++) {
        var pos = j * newSize.w + i;
        if ((newRawColors[pos] != rawColors[pos]) || forced) {
          rawColors[pos] = newRawColors[pos];
          if (rawColors[pos] & 0xff) {
            context.fillStyle = colors.getHtmlColor(i, j);
            context.fillRect(i * cellSize, j * cellSize, cellSize, cellSize);
          } else {
            context.clearRect(i * cellSize, j * cellSize, cellSize, cellSize);
          }
        }
      }
    }
    var repeatsX = ~~(context.canvas.width / newSize.w / cellSize) + 1;
    var repeatsY = ~~(context.canvas.height / newSize.h / cellSize) + 1;
    var imgData = context.getImageData(0, 0, newSize.w * cellSize, newSize.h * cellSize);
    for (i = 0; i < repeatsX; i++) {
      for (j = 0; j < repeatsY; j++) {
        context.putImageData(imgData, i * newSize.w * cellSize, j * newSize.h * cellSize);
      }
    }
  };

  function setColor(x, y, color) {
    var size = colors.getSize();
    x = (x + 1000 * size.w) % size.w;
    y = (y + 1000 * size.h) % size.h;
    if (color !== colors.getColor(x, y)) {
      colors.setColor(x, y, color);
      var repeatsX = ~~(context.canvas.width / size.w / cellSize) + 1;
      var repeatsY = ~~(context.canvas.height / size.h / cellSize) + 1;
      if (color & 0xff) {
        context.fillStyle = colors.getHtmlColor(x, y);
      } 
      for (var i = 0; i < repeatsX; i++) {
        for (var j = 0; j < repeatsY; j++) {
          if (color & 0xff) {
            context.fillRect((i * size.w + x) * cellSize, (j * size.h + y) * cellSize, cellSize, cellSize);
          } else {
            context.clearRect((i * size.w + x) * cellSize, (j * size.h + y) * cellSize, cellSize, cellSize);
          }
        }
      }
    }
  }

  this.setColor = setColor;

  this.getColor = function (x, y) {
    var size = colors.getSize();
    return colors.getColor(x % size.w, y % size.h);
  };

  this.getPixelSize = function () {
    return cellSize;
  };

  this.setPixelSize = function (size) {
    if (size != cellSize) {
      cellSize = size;
      this.setState(this.getState(), true);
    }
  };

  this.updatePreview = function () {
    var imageData = this.getStateImageData();
    canvasPreview.fill(imageData);
  };

  this.getStateImageData = function() {
    var size = colors.getSize();
    return context.getImageData(0, 0, size.w * cellSize, size.h * cellSize);
  };

  this.setCurrent = function (current) {
    canvasPreview.setCurrent(current);
  };

  this.getCurrent = function () {
    return canvasPreview.getCurrent();
  };

  this.trySetCurrent = function (ctrlKey) {
    editor.setCurrentLayer(this, ctrlKey);
  };

  this.getIndex = function () {
    return canvasPreview.getIndex();
  };

  this.setZIndex = function (zIndex) {
    context.canvas.style.zIndex = zIndex;
  };

  this.setVisibility = function (visible) {
    visibility = visible;
    context.canvas.style.display = visible
      ? 'block'
      : 'none';
  };

  this.getVisibility = function() {
    return visibility;
  };

  this.destroy = function () {
    canvasPreview.destroy();
    context.canvas.style.display = 'none';
  };

  this.restore = function() {
    canvasPreview.restore();
    context.canvas.style.display = 'block';
  };

  this.setIndex = function(index) {
    canvasPreview.setIndex(index);
  };
}