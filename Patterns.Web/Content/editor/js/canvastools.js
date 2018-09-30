function CanvasTools() {
  this.createContext = function(holder, alpha) {
    alpha = alpha !== undefined ? alpha : 1;
    holder = $(holder);
    var canvas = document.createElement('canvas');
    holder.append(canvas);
    canvas.width = holder.width();
    canvas.height = holder.height();
    var ctx = canvas.getContext('2d');
    ctx.globalAlpha = alpha;
    ctx.imageSmoothingEnabled = false;
    ctx.clearRect(0, 0, canvas.width, canvas.height);
    return ctx;
  };
  
  this.drawDashedLine = function (ctx, x1, y1, x2, y2, dashLen, spaceLen, offset) {
    dashLen = dashLen || 2;
    spaceLen = spaceLen || 2;
    var dx = x2 - x1;
    var dy = y2 - y1;
    var length = Math.sqrt(dx * dx + dy * dy);
    var ddx = dx / length;
    var ddy = dy / length;
    for (var i = 0; i < length; i++) {
      if (Math.abs(i - offset) % (dashLen + spaceLen) < dashLen) {
        ctx.fillRect(~~x1, ~~y1, 1, 1);
      }
      x1 += ddx;
      y1 += ddy;
    }
  };

  this.drawCustomGrid = function(ctx, xStep, yStep, color, dashLen, spaceLen, offset) {
    ctx.strokeStyle = color;
    ctx.fillStyle = color;
    ctx.beginPath();
    var w = ctx.canvas.width;
    var h = ctx.canvas.height;
    this.drawDashedLine(ctx, xStep - 0.5, 0, xStep - 0.5, h, dashLen, spaceLen, offset);
    var x = xStep * 2;
    var imgData = ctx.getImageData(xStep - 1, 0, 1, h);
    while (x < w) {
      ctx.putImageData(imgData, x - 1, 0);
      x += xStep;
    }
    this.drawDashedLine(ctx, 0, yStep - 0.5, w, yStep - 0.5, dashLen, spaceLen, offset);
    var y = 2 * yStep;
    imgData = ctx.getImageData(0, yStep - 1, w, 1);
    while (y < w) {
      ctx.putImageData(imgData, 0, y - 1);
      y += yStep;
    }
    ctx.stroke();
  };
}