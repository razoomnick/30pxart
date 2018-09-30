function CanvasPreview(holder, canvas) {
  var canvasTools = new CanvasTools();
  holder = $(holder);
  var previewItemHolder;
  var canvasHolder;
  var context;
  var chkVisible;
  var isCurrent;

  function init() {
    previewItemHolder = $('<div class="preview-item"/>')
      .click(clickHandler);
    chkVisible = $('<input type="checkbox" checked/>')
      .appendTo(previewItemHolder)
      .addClass('mousetrap')
      .click(changeVisibility);
    canvasHolder = $('<div class="canvas-holder"/>')
      .appendTo(previewItemHolder);
    var currentItem = $(".preview-item.current", holder).first();
    if (currentItem.length > 0) {
      currentItem.before(previewItemHolder);
    } else {
      holder.prepend(previewItemHolder);
    }
    context = canvasTools.createContext(canvasHolder);
    $('.nano').nanoScroller();
  }
  
  function changeVisibility(e) {
    canvas.setVisibility(chkVisible.is(':checked'));
    e.stopPropagation();
  }

  this.fill = function(imageData) {
    var scale = canvasHolder.width() / imageData.width;
    var newCanvas = document.createElement('canvas');
    newCanvas.width = imageData.width;
    newCanvas.height = imageData.height;
    var newContext = newCanvas.getContext("2d");
    newContext.putImageData(imageData, 0, 0);
    context.canvas.height = imageData.height * scale;
    context.drawImage(newCanvas, 0, 0, imageData.width * scale, imageData.height * scale);
  };

  function clickHandler(e) {
    canvas.trySetCurrent(e.ctrlKey);
  }

  this.setCurrent = function(current) {
    isCurrent = current;
    if (current) {
      previewItemHolder.addClass("current");
    } else {
      previewItemHolder.removeClass("current");
    }
  };

  this.getCurrent = function() {
    return isCurrent;
  };

  this.getIndex = function() {
    return previewItemHolder.index();
  };

  this.setIndex = function(index) {
    if (index > 0) {
      var prev = $('.preview-item', holder).eq(index - 1);
      prev.after(previewItemHolder.detach());
    } else {
      holder.prepend(previewItemHolder.detach());
    }
  };

  this.destroy = function() {
    previewItemHolder.css('display', 'none');
  };

  this.restore = function() {
    previewItemHolder.css('display', 'block');
  };
  
  init(this);
}