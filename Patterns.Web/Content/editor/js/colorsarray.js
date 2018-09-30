function ColorsArray(w, h, clrs) {
  var colors = clrs;

  if (!colors) {
    var buffer = new ArrayBuffer(w * h * 4);
    colors = new Uint32Array(buffer);
  }

  this.getColor = function(x, y) {
    return colors[y * w + x];
  };

  this.setColor = function(x, y, color) {
    colors[y * w + x] = color;
  };

  this.getHtmlColor = function(x, y) {
    var color = this.getColor(x, y);
    return '#' + ('000000' + (color >>> 8).toString(16)).substr(-6);
  };

  this.getSize = function() {
    return { w: w, h: h };
  };
  
  this.copy = function () {
    var bfr = new ArrayBuffer(colors.length * 4);
    var colorsCopy = new Uint32Array(colors.length);
    colorsCopy.set(colors, 0);
    return new ColorsArray(w, h, colorsCopy);
  };

  this.getColors = function() {
    return colors;
  };
} 