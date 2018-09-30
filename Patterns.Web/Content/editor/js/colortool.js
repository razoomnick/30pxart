function ColorTool() {
  var colorPicker = $('#colorPicker');
  var holder = $('#plhColorButtons');
  var colorButtons;
  var colorButton;
  var colors = ['#fa0009', '#88ca35', '#29a9e8', '#ffcb00', '#101010', '#ffffff'];
  createColorButtons();

  function createColorButtons() {
    colorButtons = [];
    for (var i = 0; i < colors.length; i++) {
      var buttonElement = $('<button/>', {
          'class' : 'color'
        })
        .css('background', colors[i])
        .appendTo(holder);
      var button = {
        element : buttonElement,
        color : colors[i]
      };
      colorButtons.push(button);
    }
    initColorButtonsClicks();
    selectColor(colorButtons[0]);
  }

  function initColorButtonsClicks() {
    var getLeftClickHandler = function (button) {
      return function () {
        selectColor(button);
      };
    };

    var getRightClickHandler = function (button) {
      return function () {
        pickCustomColor(button);
      };
    };

    for (var i = 0; i < colorButtons.length; i++) {
      colorButtons[i].element
      .unbind('click')
      .click(getLeftClickHandler(colorButtons[i]))
      .rightclick(getRightClickHandler(colorButtons[i]));
    }
  }

  function selectColor(button) {
    if (colorButton != button) {
      colorButton = button;
      for (var i = 0; i < colorButtons.length; i++) {
        colorButtons[i].element.removeClass('current');
      }
      button.element.addClass('current');
    } else {
      pickCustomColor(button);
    }
  }

  function pickCustomColor(button) {
    var colorHandler = function () {
      button.color = colorPicker.val();
      button.element.css('background', button.color);
    };
    colorPicker.unbind('change').bind('change', colorHandler);
    colorPicker.val(button.color);
    colorPicker.click();
  }

  this.getColor = function () {
    var htmlColor = colorButton.color;
    var color = parseInt(htmlColor.substring(1) + 'ff', 16);
    return color;
  };

  this.setColor = function (color) {
    var htmlColor = '#' + (color >>> 8).toString(16);
    colorButton.color = htmlColor;
    colorButton.element.css('background', htmlColor);
  };
}
