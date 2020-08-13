/*
 * Written by roboticsbytk 
 * Date 11/08/2020
 * Website: roboticsbytk.blogspot.com 
  
 * This program controls the stepper motor and moves by a user defined angle value. The user defines
 * the angle via  Serial communication. Either you can use the Serial Monitor to input the value or the 
 * WPF application created by me.
 * To input an angle you must add a $ sign at the end 
 * e.g  45 degrees   -> 45 $
 * 95 degrees   -> 95 $
 * 
 * The Stepper Motor used in this example was the 28BYJ-48 step motor (In Full step mode).
 * All of the code was written by me. Execept the Serial coomunication part, it was obtained from :
 * Example 2 - Receiving several characters from the Serial Monitor written by Robin2 (https://forum.arduino.cc/index.php?topic=396450.0)


*/

#include <Stepper.h>

// change this to the number of steps on your motor
#define stepsPerRevolution 2048 //FullStep Mode
// create an instance of the stepper class, specifying
// the number of steps of the motor and the pins it's
// attached to
/* IN1 => PIN 8
   IN3 => PIN 10
   IN2 => PIN 9
   IN4 => PIN 11 

   IN1-4 correspond to the 4 pins on the  ULN2003 driver board.
   Take care of the order in which you type the PIN numbers.
   */
Stepper myStepper(stepsPerRevolution, 8,10,9 ,11);


const byte numChars =15; //the length of the array will be 15 because I'm only sending a few characters 
char rc[numChars];// an array to store the received data

boolean newData=false; //status variable to check if data was received and sent. (True when data is received and not sent to motor yet)
int data; //will store user-defined angle in integers
void setup() {

 myStepper.setSpeed(1); //Running the motor at a a very low speed
 Serial.begin(9600); //BAUD Rate is set to 9600
}

void loop() {

receiveit(); //Checks if new data is coming and stores it in the rc array
printit(); //Prints on Serial Monitor and sends data to Stepper Motor
 
}

void receiveit(){
  char readit; //Temporarily stores characters in this variable obtained from the Serial Port
  static int i=0; //updates index value each time a new characters is added to the rc array
        
  while(Serial.available()>0){
    readit=Serial.read(); //gets characters from the Serial Port
    //If no previous data was left unsent
  if(newData==false){
 
    if(readit!='$'){ //checks to see if the string hasn't ended, $ will be the symbol that signifies the end of the string
    rc[i]=readit; //adds character to the end of the array
    i++; //updates the indexh
    if(i>=numChars){ //if array length is equal or greater than the max array length
      i=numChars-1; //Will overwrite at the end of the array
      }
    }

    else{ //if string has ended because there was the $ at the end
     rc[i]='\0'; //ends array and removes the $
     i=0; //Resets the position to the beginning of the array
     newData=true; // set to true to signify Data was received and has to be sent
      
      }
    }
  }
   
  
  }

  void printit(){
 
    if(newData==true){  //if data was received and hasnt been sent yet
      data=atoi(rc);//Converts to Integer
      
        // Serial.print("Data as Number ... ");   
        //Serial.println(data);   //Print the angle number to the Serial Monitor
        
        rotateit(); //Sends to Stepper Motor
        newData = false; //Allows for more characters to be received
 
      }
     
    }

    void rotateit(){
      int steps=data/0.176; //Calculates the number of steps needed to rotate the motor by defined angle
      myStepper.setSpeed(6); //
      //Serial.print(steps); //Prints number of steps to Serial Monitor
      myStepper.step(steps); //Rotates the motor by the number of steps 
      delay(1000); //delays by 5 second
      //Motor stops running until user defines new angle
    
      }
