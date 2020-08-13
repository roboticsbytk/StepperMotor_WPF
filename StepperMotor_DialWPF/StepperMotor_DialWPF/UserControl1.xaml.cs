/*
 * Created by SharpDevelop.
 * User: roboticsbytk
 * Date: 11/8/2020
 * Website: roboticsbytk.blogspot.com
 * 
 * This 
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xaml;
using System.ComponentModel;
using System.Windows.Shapes;
using System.IO.Ports; // Important to import, for Serial communication
 
namespace StepperMotor_DialWPF
{
	/// <summary>
	/// UserControl1 serves to create the interface whereby a user can rotate the dial to their
	/// desired position and send the corresponding angle value to the Arduino using Serial Communication
	/// The application will first ask the user to rotate the dial to the position where the real-life stepper motor is pointing.
	/// It then takes in the initial angle of the motor and uses it to move to the user defined orientation.
	/// </summary>
	public partial class UserControl1 : UserControl
	{    
		private RotateTransform rotation2;		
		private Point pointorigin; //stores the centre position of the big circle, used for calculation purposes
		private bool leftbutton; //sets status to true when the left button was pressed and false when lifted
		private double angle=0; //stores the calculated angle value of the ellipse
		private bool initializestatus; //Only becomes true after the initial angle value has been stored
		private double initialangle;
	
	//Creating a new instance for a Serial Port, Baud Rate=9600, 8bits
 	SerialPort port1=new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
	
		public UserControl1()
		{
			InitializeComponent();
			
			//Stores the origin point coordinates of the big circle
			pointorigin.Y=Science.Margin.Top+Science.Height/2;
			pointorigin.X=Science.Margin.Left+Science.Width/2;
			
			//Instructs the user to set dial to initial position
			MessageBox.Show("Set the dial \nto the corresponding position \nof your stepper \nmotor's dial");
			
		 	//sets status to false because the initial angle of the motor hasnt been stored yet
			initializestatus=false;
		}
		
		
		
		void Ellipse2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{  	//Event triggered when the smaller ellipse is clicked onto with the left button
			//sets leftbutton to true
  			leftbutton=true;
  			 
		}
		
		void Science_MouseMove(object sender, MouseEventArgs e)
		{   //Anytime the mouse moves around in the grid region, this function is triggered
			
			//stores the x and y coordinates of the mouse pointer
			Point point2=Mouse.GetPosition(this);
			//To find the inverse tangent, the opposite and adjacent lengths are found
			//Using the centre of the bigger circle and the current position of the mouse pointer
			double adj_length=-point2.X+pointorigin.X;
			double opp_length=-point2.Y+pointorigin.Y;
			
			//If the initial angle hasn't been set yet as in initializestatus=false
			// AND left button was previously pressed, the condition starts
			if(initializestatus==false&&leftbutton==true){
				
				//calculates the inverse tangent angle in degrees and subtracts from 90
				// because the program doesnt not adhere to cartesian coordinate rules currently
				//Also it is set to negative because clockwise is -ve
				angle=Math.Round(-(+90-Math.Atan2(opp_length,adj_length)*180/Math.PI));
				//Uses RotateTransform to rotate the ellipse by calculated angle
				//the RenderTransform origin was set to the origin of the bigger circle (0.4471,4.6444)
				rotation2=new RotateTransform(angle,0.4471,4.6444);
				//This renders the ellipse's new position on the WIndow
				Ellipse2.RenderTransform=rotation2;
				//Displays the position of the dial (in degrees) in the Left Text Box
				Temp.Text= (-rotation2.Angle+90).ToString();
				initialangle=(-rotation2.Angle+90); //Stores the initial angle value (+90 is added to cater for the cartesian coordinate rules)
			}
			//After getting the initial angle, the following bit of code is run instead
			else if(leftbutton){
				//This bit of code is explained above
				angle=Math.Round(-(+90-Math.Atan2(opp_length,adj_length)*180/Math.PI));
				rotation2=new RotateTransform(angle,0.4471,4.6444);				
				Ellipse2.RenderTransform=rotation2;
				Temp.Text= (-rotation2.Angle+90).ToString();
			}
		}
		
		void Science_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
		{	
			if(initializestatus==false){
				initializestatus=true; //After button is no longer pressed, the initial angle is marked at wherever the dial is pointing
				MessageBox.Show("Initial Dial position has been marked!");//Message informs the user that angle was marked
			}
			leftbutton=false; //left button is no longer pressed, status changes
			Temp.Text=(-rotation2.Angle+90).ToString(); //displays current angle in the TextBox
		}
		
		void ButtonSerial_Click(object sender, RoutedEventArgs e)
		{	//Once the button "Send to Arduino" is clicked, 
			//the current angular position of the ellipse is sent via Serial communication
			
			//the temporary variable subtracts the initial angle position, so that it is compensated for
			// when the motor starts moving. 
			//Also at each iteration, the previous position of the motor is stored in initialangle, and is compensated for at each iteration
			var temporary=Math.Round(((-rotation2.Angle+90)-initialangle));
			
			//tries opening and writing to the port
			try{
				port1.Open(); //Opens port
				if(port1.IsOpen){
				 port1.Write(temporary+" $"); //Writes to the port and appends a $ symbol b/c it signifies end of string for the Arduino code
				 
				 //Shows a Message once the angle is sent sucessfully
				 MessageBox.Show("Sent "+temporary+" $ to arduino. Initial angle was :"+initialangle);
				 port1.Close(); //closes the port
				}
			}catch(Exception ex){MessageBox.Show(ex.Message); //if any exception arises, it lets the user know			
			}
			initialangle=0; //resets initial angle to zero
			initialangle=(90-rotation2.Angle); //takes into account the current angular position of the stepper motor
		}
}
}