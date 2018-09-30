function ZoomOutTool(editor){
  this.setCurrent = function(current) {
    if (current) {
      var pixelSize = editor.getPixelSize();
      if (pixelSize > 3) {
        pixelSize -= 3;
        editor.setPixelSize(pixelSize);
      }
    }
  };
}