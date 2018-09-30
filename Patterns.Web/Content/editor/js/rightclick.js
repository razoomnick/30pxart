(function($){
  $.fn.rightclick = function(fn){ 
    $(this).mousedown(function(e){
      if (e.button == 2){
        fn && fn(e);
        return true;
      }
      return false;
    });
    return this;
  };
})(jQuery);
