function DeleteLayersCommand(layersManager, layers) {

  this.undo = function () {
    for (var i = 0; i < layers.length; i++) {
      layersManager.restoreLayer(layers[i]);
      layersManager.setCurrentLayer(layers[i], i != 0);
    }
  };

  this.redo = function() {
    layersManager.deleteLayers(layers);
  };
}