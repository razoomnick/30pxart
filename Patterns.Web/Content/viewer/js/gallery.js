function Gallery(endpoint) {
  var api = new Api(endpoint + "api/");
  var saveTool = new SaveTool();

  initAll();

  function initAll() {
    initLikes();
    initComments();
    initFollows();
    initMoreLinks();
    initChangeAvatarInputs();
    initDownloadLinks();
    initVkShareButtons();
    initSubmitLinks();
  }
  
  function initFollows() {
    function clickHandler(e) {
      var userName = e.target.dataset.userName;
      var currentUserName = e.target.dataset.currentUserName;
      var plhFollowers = $('#plhFollowers' + userName);
      var lblFollowersCount = $('#lblFollowersCount' + userName);
      var target = $(e.target);

      function followedCallback(apiFollower) {
        function renderFollower(html) {
          plhFollowers.prepend(html);
        }
        $.get(endpoint + 'users/' + apiFollower.subscriber.name + '/tile', renderFollower);
      }
      
      if (target.is('.followed')) {
        target.removeClass('followed');
        target.val('Follow');
        lblFollowersCount.html(parseInt(lblFollowersCount.html()) - 1);
        $('.followers [data-user-name=' + currentUserName + ']').remove();
        api.unfollow(userName);
      } else {
        target.addClass('followed');
        target.val('Unfollow');
        lblFollowersCount.html(parseInt(lblFollowersCount.html()) + 1);
        api.follow(userName, followedCallback);
      }
      return false;
    }

    $('.btn-follow').unbind('click').click(clickHandler);
  }
  
  function initLikes() {
    function clickHandler(e) {
      var patternId = e.target.dataset.patternId;
      var target = $(e.target);
      var lblLikesCount = $("#lblLikesCount" + patternId);

      if (target.is('.liked')) {
        target.removeClass('liked');
        lblLikesCount.html(parseInt(lblLikesCount.html()) - 1);
        api.dislike(patternId);
      } else {
        target.addClass('liked');
        lblLikesCount.html(parseInt(lblLikesCount.html()) + 1);
        api.like(patternId);
      }
      return false;
    }

    $('.like.enabled').unbind('click').click(clickHandler);
  }

  function initComments() {

    function clickHandler(e) {
      var patternId = e.target.dataset.patternId;
      var lblCommentsCount = $('#lblCommentsCount' + patternId);
      var plhComments = $('#plhComments' + patternId);
      var txtComment = $('#txtComment' + patternId);
      var commentText = txtComment.val();
      
      function commentAddedCallback(apiComment) {
        function renderComment(html) {
          plhComments.append(html);
        }
        lblCommentsCount.html(parseInt(lblCommentsCount.html()) + 1);
        $.get(endpoint + "comments/" + apiComment.id, renderComment);
      }

      if (commentText.trim()) {
        api.addComment(patternId, commentText, commentAddedCallback);
        txtComment.val('');
      }
      return false;
    }

    function keyPressHandler(e) {
      if (e.keyCode === 13) {
        clickHandler(e);
      }
    }

    $('.new-comment .btn-primary').unbind('click').click(clickHandler);
    $('.txt-comment').unbind('keypress').bind('keypress', keyPressHandler);
  }

  function initMoreLinks() {
    function clickHandler(e) {
      var url = e.target.href;
      $(e.target).css("visigility", "hidden");
      $('.btn-more').unbind('click');
      function renderMore(html) {
        $(e.target).remove();
        $('#gallery').append(html);
        initAll();
      }

      $.get(url, renderMore);
      return false;
    }
    
    function handleScroll() {
      if ($(window).scrollTop() > $(document).height() - $(window).height() - 1000) {
        $(".btn-more").click();
      }
    }

    $('.btn-more').unbind('click').click(clickHandler);
    $(window).unbind('scroll').bind('scroll', handleScroll);
  }

  function initChangeAvatarInputs() {
    function changeHandler() {
      $('form').submit();
    }
    $('#fileAvatar').bind('change', changeHandler);
  }
  
  function initDownloadLinks() {
    function getClickHandler(url) {
      return function clickHandler() {
        saveTool.save(url);
        return false;
      };
    }
    
    var links = $('a.download');
    for (var i = 0; i < links.length; i++) {
      $(links[i]).unbind('click').click(getClickHandler(links[i].href));
    }
  }
  
  function initVkShareButtons() {
    var sharePlaceholders = $('.vk-share');
    for (var i = 0; i < sharePlaceholders.length; i++) {
      var url = sharePlaceholders[i].dataset.url;
      var title = sharePlaceholders[i].dataset.title;
      var imageUrl = sharePlaceholders[i].dataset.imageUrl;
      var description = sharePlaceholders[i].dataset.description;
      var html = VK.Share.button({
        url: url,
        title: title,
        description: description,
        image: imageUrl,
        noparse: true
      });
      sharePlaceholders[i].innerHTML = html;
    }
  }

  function initSubmitLinks() {
    function clickHandler(e) {
      $(e.target).closest('form').submit();
    }
    $('.link-submit').click(clickHandler);
  }
}