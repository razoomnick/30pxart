function Toolbox(editor, layersManager, api) {
  var colorTool = new ColorTool();
  var selectTool = new SelectTool(editor);
  new CoordinatesTool(editor);
  
  var stateButtons = {
    btnGrid : new ToolButton('#btnGrid', new GridTool(editor), true, 'ctrl+\''),
    btnSharp : new ToolButton('#btnSharp', new SharpTool(editor), true, 'ctrl+;')
  };
  

  var actionButtons = {
    btnResize : new ToolButton('#btnResize', new ResizeTool(editor, selectTool), false, 'ctrl+alt+c'),
    btnUndo : new ToolButton('#btnUndo', new UndoTool(editor), false, ['ctrl+z', 'ctrl+alt+z']),
    btnRedo : new ToolButton('#btnRedo', new RedoTool(editor), false, ['ctrl+y', 'ctrl+shift+z']),
    btnSave : new ToolButton('#btnSave', new SaveTool(editor, api), false, 'ctrl+s'),
    btnZoomIn : new ToolButton('#btnZoomIn', new ZoomInTool(editor, api), false, 'z'),
    btnZoomOut : new ToolButton('#btnZoomOut', new ZoomOutTool(editor, api), false, 'x'),
    btnAddLayer: new ToolButton('#btnAddLayer', new AddLayerTool(layersManager), false, 'shift+n'),
    btnDeleteLayer: new ToolButton('#btnDeleteLayer', new DeleteLayerTool(layersManager), false),
    btnMergeLayers: new ToolButton('#btnMergeLayers', new MergeLayersTool(layersManager), false, 'ctrl+e')
  };

  var toolButtons = {
    btnBrush : new ToolButton('#btnBrush', new BrushTool(editor, colorTool), true, 'b', disableTools),
    btnFill : new ToolButton('#btnFill', new FillTool(editor, colorTool), true, 'g', disableTools),
    btnPick : new ToolButton('#btnPick', new PickTool(editor, colorTool), true, 'i', disableTools),
    btnSelect : new ToolButton('#btnSelect', selectTool, true, 'm', disableTools),
    btnErase: new ToolButton('#btnErase', new EraseTool(editor), true, 'e', disableTools),
    btnMove: new ToolButton('#btnMove', new MoveTool(editor, selectTool, colorTool), true, 'v', disableTools)
  };
  
  toolButtons.btnBrush.setCurrent(true);
  stateButtons.btnGrid.setCurrent(true);
  stateButtons.btnSharp.setCurrent(true);
  
  function disableTools(){
    for (var b in toolButtons){
      toolButtons[b].setCurrent(false);
    }
    return true;
  }
}