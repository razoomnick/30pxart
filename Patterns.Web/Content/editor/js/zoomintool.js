function ZoomInTool(editor){
  this.setCurrent = function(current) {
    if (current) {
      var pixelSize = editor.getPixelSize();
      if (pixelSize < 30) {
        pixelSize += 3;
        editor.setPixelSize(pixelSize);
      }
    }
  };
}