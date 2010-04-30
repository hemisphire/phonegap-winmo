PhoneGap Windows Mobile
=====================================================
Allows developers to create Windows Mobile applications using HTML, 
CSS and JavaScript, and bridge into device functionality like 
geolocation, SMS, device information, accelerometer, etc. via
a JavaScript API.

Pre-requisites
-----------------------------------------------------
You should have Microsoft Visual Studio 2005, Windows Mobile 6 SDK and the .NET 2.0 framework installed. 

Create a PhoneGap project with Visual Studio 
-----------------------------------------------------
Double click the solution file (PhoneGap.sln) and Visual Studio will open with the project opened.  Select Build and Build Solution, then you can choose
to deploy (under Build) or debug (under Debug).

Accelerometer
-----------------------------------------------------
The accelerometer managed code in AccelerometerCommand.cs is currently commented out as it will only work on supported HTC devices like the Diamond.