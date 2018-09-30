function SaveTool() {
  this.save = function (url) {
    var image = new window.Image();
    $(image).bind('load', handleImageLoad);
    image.src = url;
  };

  function handleImageLoad(ev) {
    var canvas = document.createElement('canvas');
    var canvasWrapper = $(canvas).css('display', 'none').appendTo(document.body);
    canvas.height = 1080;
    canvas.width = 1920;
    var context = canvas.getContext('2d');
    for (var x = 0; x < canvas.width; x += ev.target.width) {
      for (var y = 0; y < canvas.height; y += ev.target.height) {
        context.drawImage(ev.target, x, y);
      }
    }
    var dataUrl = canvas.toDataURL();
    dataUrl = dataUrl.replace(/^data:image\/[^;]/, 'data:application/octet-stream;');
    var link = $('<a/>', { href: dataUrl, download: 'pattern ' + new Date() + '.png', id: 'lnkDownload' });
    link.css('display', 'none').appendTo(document.body);
    document.getElementById('lnkDownload').click();
    link.remove();
    canvasWrapper.remove();
  }
}