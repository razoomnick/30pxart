function Api(endpoint) {

  this.savePattern = function(pattern, success, fail) {
    putToApiAsync('patterns', pattern, success, fail);
  };

  this.previewPattern = function(pattern, success, fail) {
    postToApiAsync('patterns/base64', pattern, success, fail);
  };

  this.like = function(patternId, success, fail) {
    putToApiAsync('patterns/' + patternId + '/likes', null, success, fail);
  };
  
  this.dislike = function(patternId, success, fail) {
    deleteFromApiAsync('patterns/' + patternId + '/likes', success, fail);
  };

  this.follow = function(publisherId, success, fail) {
    putToApiAsync('users/' + publisherId + '/followers', null, success, fail);
  };
  
  this.unfollow = function(publisherId, success, fail) {
    deleteFromApiAsync('users/' + publisherId + '/followers', success, fail);
  };
  
  this.addComment = function (patternId, text, success, fail) {
    var apiComment = { text: text };
    putToApiAsync('patterns/' + patternId + '/comments', apiComment, success, fail);
  };

  function putToApiAsync(url, object, success, fail) {
    var json = object ? JSON.stringify(object) : '';
    $.ajax({
      type: 'PUT',
      url: endpoint + '/' + url,
      async: true,
      timeout: 100000,
      contentType: "application/json",
      dataType: 'json',
      data: json,
      success: success,
      error: fail,
      fail: fail
    });
  }

  function postToApiAsync(url, object, success, fail) {
    var json = object ? JSON.stringify(object) : '';
    $.ajax({
      type: 'POST',
      url: endpoint + '/' + url,
      async: true,
      timeout: 100000,
      contentType: "application/json",
      dataType: 'json',
      data: json,
      success: success,
      error: fail,
      fail: fail
    });
  }

  function deleteFromApiAsync(url, success, fail) {
    $.ajax({
      type: 'DELETE',
      url: endpoint + '/' + url,
      async: true,
      timeout: 100000,
      contentType: "application/json",
      dataType: 'json',
      success: success,
      error: fail,
      fail: fail
    });
  };
}