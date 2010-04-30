var device = {
    init: function() {
		PhoneGap.exec("initialize");
		// For some reason, in WinMo v6.0, we need to delay setting device.available because device.name does not exist yet.
		// A 10ms delay is sufficient for the variable to be visible.
		setTimeout('PhoneGap.available = typeof(device.name) == "string";',10);
    }
};