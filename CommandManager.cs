using System;

namespace PhoneGap {

    class CommandManager {

        private Command[] commands = new Command[ 10 ];

        public CommandManager() {
            commands[ 0 ] = new AccelerometerCommand();
            commands[ 1 ] = new CameraCommand();
            commands[ 2 ] = new ContactCommand();
            commands[ 3 ] = new DeviceCommand();
            commands[ 4 ] = new FileCommand();
            commands[ 5 ] = new GeolocationCommand();
            commands[ 6 ] = new MediaCommand();
            commands[ 7 ] = new NotificationCommand();
            commands[ 8 ] = new SMSCommand();
            commands[ 9 ] = new TelephonyCommand();
        }

        public String processInstruction(String instruction) {
            for (int index = 0; index < commands.Length; index++) {
			    Command command = (Command) commands[index]; 
			    if (command.accept(instruction))
			        try {
				        return command.execute(instruction);
			        } catch(Exception e) {
                        System.Diagnostics.Debug.WriteLine( e, "bug" );
			        }
		    }
		    return null;
        }
    }

}
