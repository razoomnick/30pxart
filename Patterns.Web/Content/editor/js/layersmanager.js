function LayersManager(editor, holder, previewHolder, width, height, cellSize) {
  var canvases = [];
  var currentCanvases = [];
  var plhLayersContent = document.getElementById('plhLayersContent');
  Sortable.create(plhLayersContent, { onUpdate: getSortableHandler(this) });

  this.addLayer = function (state) {
    var size = canvases.length == 0
      ? { w: width, h: height }
      : canvases[0].getState().getSize();
    var canvas = new Canvas(holder, previewHolder, size.w, size.h, cellSize, this);
    canvas.updatePreview();
    canvases.push(canvas);
    this.setCurrentLayer(canvas);
    this.sortLayers();
    if (state) {
      canvas.setState(state);
      canvas.updatePreview();
    }
    if (canvases.length > 1) {
      var command = new AddLayerCommand(this, canvas);
      editor.addHistoryPoint(command);
    }
  };

  this.setCurrentLayer = function (canvas, ctrlKey) {
    if (!ctrlKey) {
      currentCanvases = [canvas];
      for (var i = 0; i < canvases.length; i++) {
        canvases[i].setCurrent(canvases[i] == canvas);
      }
    } else {
      currentCanvases.push(canvas);
      canvas.setCurrent(true);
    }
  };

  function getSortableHandler(layersManager) {
    return function () { layersIndexesUpdated(layersManager); };
  }

  function layersIndexesUpdated(layersManager) {
    var command = new SortLayersCommand(layersManager);
    layersManager.sortLayers();
    command.finish();
    editor.addHistoryPoint(command);
  };

  this.sortLayers = function() {
    for (var i = 0; i < canvases.length; i++) {
      var index = canvases[i].getIndex();
      canvases[i].lastIndex = index;
      canvases[i].setZIndex(canvases.length - index);
    }
  };
  
  this.getLayers = function () {
    return canvases;
  };

  this.getCurrentLayers = function () {
    return currentCanvases;
  };
  
  this.setPixelSize = function (pixelSize) {
    cellSize = pixelSize;
    for (var i = 0; i < canvases.length; i++) {
      canvases[i].setPixelSize(cellSize);
    }
  };
  
  this.updatePreview = function () {
    for (var i = 0; i < currentCanvases.length; i++) {
      currentCanvases[i].updatePreview();
    }
  };

  this.getSize = function() {
    return canvases[0].getState().getSize();
  };
  
  this.deleteCurrentLayers = function (forced) {
    this.deleteLayers(currentCanvases, forced);
  };

  this.deleteLayers = function(layers, forced) {
    if (canvases.length > layers.length || forced) {
      var nextCurrentCanvas = this.getNextCurrentLayer(canvases, currentCanvases);
      for (var i = 0; i < currentCanvases.length; i++) {
        var currentPositionInArray = 0;
        for (var j = 0; j < canvases.length; j++) {
          if (canvases[j] == layers[i]) {
            currentPositionInArray = j;
          }
        }
        layers[i].setCurrent(false);
        layers[i].destroy();
        canvases.splice(currentPositionInArray, 1);
      }
      var command = new DeleteLayersCommand(this, layers);
      editor.addHistoryPoint(command);
      if (nextCurrentCanvas) {
        this.setCurrentLayer(nextCurrentCanvas);
      }
    } else {
      alert('Can\'t delete all layers');
    }
  };

  this.getNextCurrentLayer = function () {
    var minIndex = currentCanvases[0].getIndex();
    var nonCurrentLayers = [];
    for (var i = 0; i < canvases.length; i++) {
      var index = canvases[i].getIndex();
      var isCurrent = canvases[i].getCurrent();
      if (isCurrent) {
        if (index < minIndex) {
          minIndex = index;
        }
      } else {
        nonCurrentLayers.push(canvases[i]);
      }
    }
    var nextCurrentLayer = null;
    if (nonCurrentLayers.length > 0) {
      nextCurrentLayer = nonCurrentLayers[0];
      var nextCurrentLayerIndex = nextCurrentLayer.getIndex();
      for (i = 0; i < nonCurrentLayers.length; i++) {
        index = nonCurrentLayers[i].getIndex();
        if (index > minIndex && index < nextCurrentLayerIndex) {
          nextCurrentLayer = nonCurrentLayers[i];
          nextCurrentLayerIndex = index;
        }
      }
      if (nextCurrentLayerIndex < minIndex) {
        for (i = 0; i < nonCurrentLayers.length; i++) {
          index = nonCurrentLayers[i].getIndex();
          if (index > nextCurrentLayerIndex) {
            nextCurrentLayer = nonCurrentLayers[i];
            nextCurrentLayerIndex = index;
          }
        }
      }
    }
    return nextCurrentLayer;
  };

  this.restoreLayer = function(canvas) {
    canvases.push(canvas);
    canvas.restore();
  };
}