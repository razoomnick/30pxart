function ArrayCompressor() {
  this.compress = function(array) {
    var uniqueAndResultSize = getUniqueAndResultSize(array);
    console.log('original: ' + uniqueAndResultSize.size + ', compressed:' + uniqueAndResultSize.resultSize);
    var result = pack(uniqueAndResultSize);
    return result;
  };

  this.decompress = function(array) {
    var result = unpack(array);
    return result;
  };

  function getUniqueAndResultSize(array) {
    var unique = {};
    var resultSize = 1;
    var lastValue;
    for (var i = 0; i < array.length; i++) {
      if (!unique[array[i]]) {
        unique[array[i]] = [];
        resultSize += 2;
      }
      if (array[i] !== lastValue) {
        unique[array[i]].push(i);
        lastValue = array[i];
        resultSize++;
      }
    }
    return { unique: unique, size: array.length, resultSize: resultSize };
  }

  function pack(uniqueAndResultSize) {
    var unique = uniqueAndResultSize.unique;
    var buffer = new ArrayBuffer(uniqueAndResultSize.resultSize * 4);
    var result = new Uint32Array(buffer);
    result[0] = uniqueAndResultSize.size;
    var pos = 1;
    for (var v in unique) {
      result[pos] = v;
      pos++;
      result[pos] = unique[v].length;
      pos++;
      for (var i = 0; i < unique[v].length; i++) {
        result[pos] = unique[v][i];
        pos++;
      }
    }
    return result;
  }

  function unpack(array) {
    var size = array[0];
    var buffer = new ArrayBuffer(size * 4);
    var result = new Uint32Array(buffer);
    var currentValue = 0;
    var itemsLeft = 0;
    var positionsBuffer = new ArrayBuffer(size);
    var positions = new Uint8Array(positionsBuffer);
    for (var i = 1; i < array.length; i++) {
      if (itemsLeft === 0) {
        currentValue = array[i];
        i++;
        itemsLeft = array[i];
      } else {
        result[array[i]] = currentValue;
        positions[array[i]] = 1;
        itemsLeft--;
      }
    }
    for (i = 0; i < result.length; i++) {
      if (positions[i] === 1) {
        currentValue = result[i];
      } else {
        result[i] = currentValue;
      }
    }
    return result;
  }
}