function Editor(holder, previewHolder, width, height, cellSize, endpoint, canvases) {
  var layersManager = new LayersManager(this, holder, previewHolder, width, height, cellSize);
  var api = new Api(endpoint);
  var grid = new Grid(holder, width * cellSize, height * cellSize, 6, 3, 3);
  var pixelGrid = new Grid(holder, cellSize, cellSize, 1, 2, 2);
  var canvasTools = new CanvasTools();
  var selectLayerContext = canvasTools.createContext(holder, 0.5);
  var undoManager = new UndoManager();
  var privateSelection;
  var handlers = {};

  function init(editor) {
    document.addEventListener('contextmenu', function(e) {
      e.preventDefault();
    }, false);

    selectLayerContext.canvas.style.zIndex = 1001;
    var toolbox = new Toolbox(editor, layersManager, api);
    initMouseEvents(holder);
    if (canvases && canvases.length > 0) {
      for (var i = canvases.length - 1; i >= 0; i--) {
        var image = new Image();
        image.onload = function () {
          var canvas = document.createElement("canvas");
          var ctx = canvas.getContext("2d");
          ctx.drawImage(this, 0, 0);
          var ctxData = ctx.getImageData(0, 0, image.width, image.height);
          var buffer = new ArrayBuffer(ctxData.data.length);
          var colors = new Uint32Array(buffer);
          for (var j = 0; j < colors.length; j++) {
            colors[j] = (ctxData.data[j * 4] << 24)
              | (ctxData.data[j * 4 + 1] << 16)
              | (ctxData.data[j * 4 + 2] << 8)
              | ctxData.data[j * 4 + 3];
          }
          var state = new ColorsArray(image.width, image.height, colors);
          layersManager.addLayer(state);
          grid.resize(cellSize * image.width, cellSize * image.height);
        };
        image.src = "data:image/png;base64," + canvases[i];
      }
    } else {
      layersManager.addLayer();
    }
  }

  this.updatePreview = function() {
    layersManager.updatePreview();
  };

  this.setState = function(state) {
    var currentLayers = layersManager.getCurrentLayers();
    if (currentLayers.length !== 1) {
      alert('Could not complete your request because no layers are selected');
    } else {
      currentLayers[0].setState(state);
    }
  };

  this.getState = function () {
    var currentLayers = layersManager.getCurrentLayers();
    return currentLayers[0].getState();
  };

  this.setColor = function (x, y, color) {
    var currentLayers = layersManager.getCurrentLayers();
    currentLayers[0].setColor(x, y, color);
  };

  this.getColor = function(x, y) {
    var currentLayers = layersManager.getCurrentLayers();
    return currentLayers[0].getColor(x, y);
  };
  
  function initMouseEvents(element) {
    function getCellEventHandler(event) {
      return function (options) {
        var x = ~~(options.offsetX / cellSize);
        var y = ~~(options.offsetY / cellSize);
        cellEventHandler(event, x, y, options);
      };
    };

    $(element)
      .click(getCellEventHandler('click'))
      .rightclick(getCellEventHandler('rightclick'))
      .mousedown(getCellEventHandler('mousedown'))
      .mousemove(getCellEventHandler('mousemove'))
      .mouseup(getCellEventHandler('mouseup'))
      .mouseleave(getCellEventHandler('mouseleave'));
  }

  this.addHandler = function (event, handler) {
    if (!handlers[event]) {
      handlers[event] = [];
    }
    handlers[event].push(handler);
  };

  function cellEventHandler(event, x, y, options) {
    var clickHandlers = handlers[event];
    for (var i = 0; clickHandlers && i < clickHandlers.length; i++) {
      clickHandlers[i](x, y, options);
    }
  }
  
  this.select = function (selection) {
    if (selection) {
      var x = selection.xStart * cellSize;
      var y = selection.yStart * cellSize;
      var w = (selection.xEnd - selection.xStart + 1) * cellSize;
      var h = (selection.yEnd - selection.yStart + 1) * cellSize;
      selectLayerContext.fillStyle = '#cccccc';
      selectLayerContext.clearRect(0, 0, selectLayerContext.canvas.width, selectLayerContext.canvas.height);
      selectLayerContext.fillRect(x, y, w, h);
      privateSelection = selection;
    }
  };

  this.showGrid = function(visible) {
    grid.setVisibility(visible);
  };

  this.showPixelGrid = function(visible) {
    pixelGrid.setVisibility(visible);
  };

  this.addHistoryPoint = function(command) {
    layersManager.updatePreview();
    undoManager.add(command);
  };

  this.undo = function() {
    undoManager.undo();
  };

  this.redo = function() {
    undoManager.redo();
  };

  this.getPixelSize = function() {
    return cellSize;
  };

  this.setPixelSize = function(pixelSize) {
    cellSize = pixelSize;
    layersManager.setPixelSize(pixelSize);
    var size = layersManager.getSize();
    grid.resize(pixelSize * size.w, pixelSize * size.h);
    pixelGrid.resize(pixelSize, pixelSize);
    this.select(privateSelection);
  };

  this.getGrid = function() {
    return grid;
  };

  this.getCellSize = function() {
    return cellSize;
  };
  
  this.getCanvases = function () {
    return layersManager.getLayers();
  };

  this.getCurrentCanvases = function () {
    return layersManager.getCurrentLayers();
  };
  
  this.addLayer = function(state){
    layersManager.addLayer(state);  
  };
  
  init(this);
}