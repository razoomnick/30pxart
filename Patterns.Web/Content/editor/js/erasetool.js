function EraseTool(editor) {

  var colorTool = {
    getColor: function () { return 0; },
    setCurrent: function(){}
  };

  var brushTool = new BrushTool(editor, colorTool);

  this.setCurrent = function(current) {
    brushTool.setCurrent(current);
  };
}