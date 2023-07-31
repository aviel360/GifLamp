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

______________________
The names and versions of the libraries we worked with ESP32

FastLED.h
LEDMatrix.h
LEDSprites.h
LEDText.h
FontMatrise.h
Blue0toothSerial.h
SD.h
SPI.h
