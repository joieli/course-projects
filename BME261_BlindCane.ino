int button = 2;

int echoPinL = 4; 
int trigPinL = 3; //PMW
int motorPinL = 5; //PMW

int echoPinM = 7;
int trigPinM = 6;//PMW
int motorPinM = 9;//PMW

int echoPinH = 8;
int trigPinH = 10; //PMW
int motorPinH = 11;//PMW

//Low, Mid, high thresholds
int distancesL[] = {10, 20, 30};
int distancesM[] = {10, 20, 30};
int distancesH[] = {10, 20, 30};

//variables used for the button debounce
boolean lastButton = LOW;
boolean currentButton = LOW;
boolean buzzerOn = false;
int debounceDelay = 20;

long duration; // variable for the duration of sound wave travel
int distance; // variable for the distance measurement

void setup() {
  pinMode(trigPinL, OUTPUT);
  pinMode(echoPinL, INPUT);
  pinMode(motorPinL, OUTPUT);
  pinMode(trigPinM, OUTPUT);
  pinMode(echoPinM, INPUT);
  pinMode(motorPinM, OUTPUT);
  pinMode(trigPinH, OUTPUT);
  pinMode(echoPinH, INPUT);
  pinMode(motorPinH, OUTPUT);
  pinMode(button, INPUT);
  Serial.begin(9600); // // 
  Serial.println("Distances detected by ultrasonic sensors");
  Serial.println(buzzerOn);
}

void loop() {
  currentButton = debounce(lastButton);
  // put your main code here, to run repeatedly:
  if (lastButton == LOW && currentButton == HIGH)
  {
    buzzerOn = !buzzerOn;
    Serial.println(buzzerOn);
  }

  if(buzzerOn == true){
    detectAndBuzz(trigPinL, echoPinL, motorPinL, distancesL, "Low");
    detectAndBuzz(trigPinM, echoPinM, motorPinM, distancesM, "Mid");
    detectAndBuzz(trigPinH, echoPinH, motorPinH, distancesH, "High");
  }
  else{
    analogWrite(motorPinL, 0); //stop vibration
    analogWrite(motorPinM, 0); //stop vibration
    analogWrite(motorPinH, 0); //stop vibration
  }

}

boolean debounce(boolean last){
  boolean current = digitalRead(button);
  if(last != current){
    delay(debounceDelay);
    current = digitalRead(button);
  }
  return current;
}

void detectAndBuzz(int trigPin, int echoPin, int motorPin, int distances[], String sensorName){  
  // Clears the trigPin condition
  digitalWrite(trigPin, LOW);
  delayMicroseconds(2);
  
  // Sets the trigPin HIGH (ACTIVE) for 10 microseconds
  digitalWrite(trigPin, HIGH);
  delayMicroseconds(1000);
  digitalWrite(trigPin, LOW);
  
  // Reads the echoPin, returns the sound wave travel time in microseconds
  duration = pulseIn(echoPin, HIGH);
  
  // Calculating the distance
  distance = duration * 0.034 / 2; // Speed of sound wave divided by 2 (go and back)
  
  // Displays the distance on the Serial Monitor for testing
  Serial.print("Distance for " + sensorName + ": ");
  Serial.print(distance);
  Serial.println(" cm");

  //Buzz with different intensity when within the 3 buzzing ranges
  if(distance > 0 && distance <= distances[0]){
    analogWrite(motorPin, 240); //vibrate intensly if close
  }
  else if(distance > distances[0] && distance <= distances[1]){
    analogWrite(motorPin, 160); //vibrate moderately if in the middle
  }
  else if(distance > distances[1] && distance <= distances[2]){
    analogWrite(motorPin, 80); // vibrate lightly if far
  }
  else{
    digitalWrite(motorPin, 0); //stop vibration
  }
}
