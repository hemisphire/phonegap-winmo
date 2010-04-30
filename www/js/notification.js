/**
 * This class provides access to notifications on the device.
 */
function Notification() {
	
}

Notification.prototype.vibrate = function(mills) {
  PhoneGap.exec("vibrate",[mills]);
};

Notification.prototype.beep = function(count, volume) {
  PhoneGap.exec("beep");
};

if (typeof navigator.notification == "undefined") navigator.notification = new Notification();