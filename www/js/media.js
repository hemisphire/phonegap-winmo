/**
 * This class provides access to the device media, interfaces to both sound and video
 * @constructor
 */
function Media() {
};

Media.prototype.record = function() {
}

Media.prototype.play = function(src) {
  PhoneGap.exec("media",[src]);
}

Media.prototype.pause = function() {
}

Media.prototype.stop = function() {
}

if (typeof navigator.media == "undefined") navigator.media = new Media();