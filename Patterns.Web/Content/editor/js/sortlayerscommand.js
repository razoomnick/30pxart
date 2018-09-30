function SortLayersCommand(layersManager) {
  var initialSortedLayers = getSortedLayers();
  var newSortedLayers = [];

  function getSortedLayers() {
    var sortedLayers = [];
    var layers = layersManager.getLayers();
    for (var i = 0; i < layers.length; i++) {
      var index = layers[i].lastIndex;
      sortedLayers[index] = layers[i];
    }
    return sortedLayers;
  }

  this.finish = function() {
    newSortedLayers = getSortedLayers();
  };

  this.undo = function() {
    apply(initialSortedLayers);
  };

  this.redo = function() {
    apply(newSortedLayers);
  };
  
  function apply(sortedLayers) {
    for (var i = 0; i < sortedLayers.length; i++) {
      sortedLayers[i].setIndex(i);
    }
    layersManager.sortLayers();
  }
}