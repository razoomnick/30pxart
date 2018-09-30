function AddLayerTool(layersManager) {
  this.setCurrent = function(current) {
    if (current) {
      layersManager.addLayer();
    }
  };
}