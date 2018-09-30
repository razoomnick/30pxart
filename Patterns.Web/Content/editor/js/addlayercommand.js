function AddLayerCommand(layersManager, layer) {

  this.redo = function () {
    layersManager.restoreLayer(layer);
    layersManager.setCurrentLayer(layer);
  };

  this.undo = function () {
    layersManager.deleteLayers([layer]);
  };
}