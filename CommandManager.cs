using System;

namespace PhoneGap {

    class CommandManager {

        private Command[] commands = new Command[ 9 ];

        public CommandManager() {
            commands[ 0 ] = new DeviceCommand();
            commands[ 1 ] = new MediaCommand();
            commands[ 2 ] = new GeolocationCommand();
            commands[ 3 ] = new NotificationCommand();
            commands[ 4 ] = new TelephonyCommand();
            commands[ 5 ] = new SMSCommand();
            commands[ 6 ] = new CameraCommand();
            commands[ 7 ] = new ContactCommand();
            commands[ 8 ] = new AccelerometerCommand();
        }

        public String processInstruction(String instruction) {
            for (int index = 0; index < commands.Length; index++) {
			    Command command = (Command) commands[index]; 
			    if (command.accept(instruction))
			        try {
				        return command.execute(instruction);
			        } catch(Exception e) {
				        
			        }
		    }
		    return null;
        }
    }

}
