function CanvasStateCommand(initialState, canvas) {
  var compressedDifference = getCompressedDifference();

  function getCompressedDifference() {
    var newState = canvas.getState();
    var initialColors = initialState.getColors();
    var newColors = newState.getColors();
    var arrayCompressor = new ArrayCompressor();
    var buffer = new ArrayBuffer(newColors.length * 4);
    var difference = new Uint32Array(buffer);
    for (var i = 0; i < newColors.length; i++) {
      difference[i] = newColors[i] - initialColors[i];
    }
    var result = arrayCompressor.compress(difference);
    return result;
  }

  this.undo = function () {
    apply(function (a, b) { return (a - b) >>> 0; });
  };

  this.redo = function() {
    apply(function(a, b) { return (a + b) >>> 0; });
  };
  
  function apply(operation) {
    var state = canvas.getState().copy();
    var colors = state.getColors();
    var arrayCompressor = new ArrayCompressor();
    var difference = arrayCompressor.decompress(compressedDifference);
    for (var i = 0; i < colors.length; i++) {
      colors[i] = operation(colors[i], difference[i]);
    }
    canvas.setState(state);
    canvas.updatePreview();
  }
}