function DeleteLayerTool(layersManager) {

  this.setCurrent = function (current) {
    if (current) {
      layersManager.deleteCurrentLayers();
    }
  };
}