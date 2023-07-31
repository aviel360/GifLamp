# GifLamp

this project was created by Daniel Elgarici, Aviel Cohen and Evgeny Lambord.

GifLamp offers a versatile platform to play classic games, display pictures, and showcase animated GIFs. It's a creative and engaging project that combines hardware and software to produce mesmerizing visual experiences.
it is an interactive LED matrix powered by ESP32 which consists of two applications:

1) Phone App: This app provides full control over the LED matrix, allowing users to switch between four modes:
    Serial Input Mode: Enables real-time communication between the WPF app and the LED matrix.
    Tetris Mode: Transforms the LED matrix into a gaming screen for playing Tetris.
    Snake Mode: Turns the LED matrix into a platform for an entertaining game of Snake.
    Pixel Art Mode: Displays GIFs statically using parametric functions, creating captivating pixel art.

2) WPF Application: The WPF application empowers users to upload, edit, and create GIFs. These GIFs can then be sent to the LED matrix for display. The LED matrix can save the GIFs onto an SD card.

____________________________________________________________________________________________________________________________________
The names and versions of the libraries we worked with ESP32

FastLED.h - 3.6.0
LEDMatrix.h - 0.3.0
LEDSprites.h - //
LEDText.h - Update December 2021
FontMatrise.h - Update December 2021
Blue0toothSerial.h - // 
SD.h - // 
SPI.h - // 

____________________________________________________________________________________________________________________________________

In the main directory (the directory containing this file) you can find the installation of the phone application (BluetoothGifLamp.apk) and the installation of the WPF application (GifAppSetup.msi).
You can also find the other project directories:
* GifApp - This directory contains all the files intended for the WPF application.
* MatrixCode - This directory contains the .ino file - this is the ESP32 runtime file.
  In addition, this folder contains header files that contain state classes present in the matrix. GIF animations, pixel art, Snake game,        Tetris game, and the menu.
  as well, there are Bluetooth and SD communication wrappers in the directory and default test patterns which will be shown if there are no      uploaded animations.
  In addition to all this, there will be the configuration file.
* PhoneApp - This directory contains the .aia file with which you can edit the mobile app. This file corresponds to MIT app inventor 
* Presentation - A directory containing the files with which we presented the project. Presentation and poster.
____________________________________________________________________________________________________________________________________
