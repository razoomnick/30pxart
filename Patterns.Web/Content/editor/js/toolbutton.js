function ToolButton(selector, tool, hasState, shortcut, callback) {
  var isCurrent = false;
  var element = $(selector);
  var handler = function (e) {
    (!callback || callback()) && setCurrent(!hasState || !isCurrent);
    e.preventDefault && e.preventDefault();
  };

  element.click(handler);
  
  if (shortcut) {
    Mousetrap.bind(shortcut, handler);
  }

  var strShortcut = '';
  if (shortcut) {
    strShortcut = ('+' + shortcut)
      .replace(/(\+|\,).{1}/g, function(s) { return s.substr(0, 1) + s.substr(1, 1).toUpperCase() + s.substr(2); })
      .replace(/,/g, ' or ')
      .substr(1);
    strShortcut = ' (' + strShortcut + ')';
  }
  var title = element.attr('title');
  element.attr('title', title + strShortcut);

  function setCurrent(current) {
    if (isCurrent != current) {
      isCurrent = current;
      if (hasState) {
        if (isCurrent) {
          element.addClass('current');
        } else {
          element.removeClass('current');
        }
      }
    }
    if (tool) {
      tool.setCurrent(isCurrent);
    }
  }

  this.setCurrent = setCurrent;

  this.getCurrent = function () {
    return isCurrent;
  };
}