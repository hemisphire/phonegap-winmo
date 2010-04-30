/**
 * This class provides access to device GPS data.
 * @constructor
 */
function Geolocation() {
    /**
     * The last known GPS position.
     */
    this.lastPosition = null;
    this.lastError = null;
};

/**
 * Asynchronously aquires the current position.
 * @param {Function} successCallback The function to call when the position
 * data is available
 * @param {Function} errorCallback The function to call when there is an error 
 * getting the position data.
 * @param {PositionOptions} options The options for getting the position data
 * such as timeout.
 */
Geolocation.prototype.getCurrentPosition = function(successCallback, errorCallback, options) 
{
    var params = [successCallback];
    params.push(errorCallback);
    PhoneGap.exec("getCurrentPosition", [params]);
}

Geolocation.prototype.start = function() {
    PhoneGap.exec("start_gps");
};

Geolocation.prototype.stop = function() {
    PhoneGap.exec("stop_gps");
};

if (typeof navigator.geolocation == "undefined") navigator.geolocation = new Geolocation();