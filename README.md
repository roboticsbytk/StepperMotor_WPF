# StepperMotor_WPF
This project involves creating a WPF application whereby you can rotate/move the smaller ellipse (shown below)and send its corresponding angular position to an Arduino Board.
This is done using Serial Communication. The Arduino receives the Angular value, converts the string to integer. To rotate the stepper motor to a user defined position it calculates the number of steps needed for the stepper motor to move. 


<img src="/Images/dialmovement.gif" class="center" width="400"/>


The image below shows the setup of my project.


<img src="/Images/setupimage.JPG" class="center" width="400"/>



 Detailed explanation is given on [my blog!](http://roboticsbytk.blogspot.com)



References that I found useful/took help from:

-  https://www.makerguides.com/28byj-48-stepper-motor-arduino-tutorial/ 
-  https://forum.arduino.cc/index.php?topic=396450.0 
-  https://blog.jerrynixon.com/2012/12/walkthrough-building-sweet-dial-in-xaml.html 
-  https://www.youtube.com/watch?v=m-EqckhJNvI 
-  https://www.wpf-tutorial.com/usercontrols-and-customcontrols/creating-using-a-usercontrol/ 
