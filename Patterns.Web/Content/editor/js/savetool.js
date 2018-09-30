function SaveTool(editor, api) {
  var holder = $('#plhSaveShadow');
  var popup = $('#plhSavePopup');
  var txtName = $('#txtName');
  var chkDraft = $('#chkDraft');
  var selFilterName = $('#selFilterName');
  var rbScale = $('input[name=rbScale]');
  var btnSave = $('#btnSaveSubmit');
  var plhPreview = $('#plhPreview');
  
  holder.click(hide);
  popup.click(stop);
  Mousetrap.bind('esc', hide);
  btnSave.click(save);
  rbScale.click(preview);
  selFilterName.change(preview);

  this.setCurrent = function (current) {
    holder.show();
    preview();
  };

  function preview() {
    var filterName = selFilterName.val();
    var sizeVisibility = filterName != 'none' && filterName != 'smoothing'
      ? 'none'
      : '';
    $('#plhSizeSelector').css('display', sizeVisibility);
    function callback(base64Img) {
      plhPreview.css('background', 'url(' + base64Img + ')');
    };
    var apiPattern = getApiPattern();
    api.previewPattern(apiPattern, callback);
  }
  
  function hide() {
    holder.hide();
  }
  
  function stop(e) {
    e.stopPropagation();
  }
  
  function save() {
    function saved(savedPattern) {
      document.location.href = savedPattern.url;
    }
    function notSaved() {
      btnSave.click(save);
    }
    var apiPattern = getApiPattern();
    api.savePattern(apiPattern, saved, notSaved);
    btnSave.unbind("click");
  }
  
  function getApiPattern() {
    var name = txtName.val();
    var isDraft = chkDraft.is(':checked');
    var scale = $('input[name=rbScale]:checked').val();
    var filterName = selFilterName.val();
    var canvases = getApiStateCanvases();
    var layersVisibility = getApiStateLayersVisibility();
    var apiPattern = {
      canvases: canvases,
      name: name,
      isDraft: isDraft,
      scale: scale,
      filterName: filterName,
      layersVisibility:layersVisibility
    };
    if ($('#rbOverride').is(':checked')) {
      apiPattern.id = $('#txtPatternId').val();
    }
    return apiPattern;
  }

  function getApiStateCanvases() {
    var sortedLayers = getSortedLayers();
    var canvases = [];
    for (var i = 0; i < sortedLayers.length; i++) {
      if (sortedLayers[i]) {
        var canvas = getBase64Image(sortedLayers[i]);
        canvases.push(canvas);
      }
    }
    return canvases;
  }

  function getApiStateLayersVisibility() {
    var sortedLayers = getSortedLayers();
    var visibilities = [];
    for (var i = 0; i < sortedLayers.length; i++) {
      if (sortedLayers[i]) {
        visibilities.push(sortedLayers[i].getVisibility());
      }
    }
    return visibilities;
  }

  function getBase64Image(layer) {
    var imageData = layer.getStateImageData();
    var size = layer.getState().getSize();
    var newCanvas = document.createElement("canvas");
    newCanvas.width = imageData.width;
    newCanvas.height = imageData.height;    
    var newContext = newCanvas.getContext("2d");
    newContext.putImageData(imageData, 0, 0);
    var canvas = document.createElement("canvas");
    canvas.width = size.w;
    canvas.height = size.h;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(newCanvas, 0, 0, size.w, size.h);
    var dataUrl = canvas.toDataURL("image/png");
    return dataUrl.replace(/^data:image\/(png|jpg);base64,/, "");
  }
  
  function getSortedLayers() {
    var sortedLayers = [];
    var layers = editor.getCanvases();
    for (var i = 0; i < layers.length; i++) {
      var index = layers[i].lastIndex;
      sortedLayers[index] = layers[i];
    }
    return sortedLayers;
  }
}