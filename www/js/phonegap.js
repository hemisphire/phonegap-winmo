var PhoneGap = {
    exec: function(command, params) {
        if (PhoneGap.available || command == "initialize") {
            try {
                var url = "http://gap.exec/" + command;
                if (params) url += "/" + params.join("/");
                window.location.href = url;
            } catch(e) {
                console.log("Command '" + command + "' has not been executed, because of exception: " + e);
                alert("Error executing command '" + command + "'.");
            }
        }
    }
};