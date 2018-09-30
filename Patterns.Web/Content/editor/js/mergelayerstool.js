function MergeLayersTool(layersManager) {

  this.setCurrent = function(current) {
    if (current) {
      var currentLayers = layersManager.getCurrentLayers();
      var mergedState = merge(currentLayers);
      layersManager.deleteCurrentLayers(true);
      layersManager.addLayer(mergedState);
    }
  };

  function merge(layers) {
    sortByIndex(layers);
    var result = layers[0].getState().copy();
    var colors = result.getColors();
    for (var i = 0; i < layers.length; i++) {
      var layerColors = layers[i].getState().getColors();
      for (var x = 0; x < colors.length; x++) {
        colors[x] = colors[x] & 0xff ? colors[x] : layerColors[x];
      }
    }
    return result;
  }
  
  function sortByIndex(layers) {
    function comparer(x, y) {
      var indexX = x.getIndex();
      var indexY = y.getIndex();
      return indexX - indexY;
    }
    layers.sort(comparer);
  }
}