// TETRIS
#include <FastLED.h>
#include <LEDMatrix.h>
#include <LEDSprites.h>
#include <LEDText.h>
#include <FontMatrise.h>
#include "BluetoothSerial.h"
#include "config.h"

#define APP_MENU ((uint8_t)255)
#define APP_TETRIS ((uint8_t)0)
#define APP_SNAKE ((uint8_t)1)
#define APP_BREAKOUT ((uint8_t)2)
#define APP_PIXEL ((uint8_t)3)

uint8_t currentApp = APP_MENU;

cLEDMatrix<-MATRIX_WIDTH, -MATRIX_HEIGHT, MATRIX_TYPE> leds;
BluetoothSerial SerialBT;

// Bluetooth input
enum btnInput {NONE, UP, DOWN, LEFT, RIGHT};
btnInput currentInput = NONE;

#include "Tetris.h"
#include "Menu.h"
#include "Snake.h"
#include "Breakout.h"
#include "Pixel.h"

void setup()
{
  Serial.begin(921600);
  SerialBT.begin("ESP32Matrix");
  
  FastLED.addLeds<CHIPSET, LED_PIN, COLOR_ORDER>(leds[0], leds.Size());
  FastLED.setMaxPowerInVoltsAndMilliamps(5,1000);
  FastLED.clear(true);
  FastLED.show();
}

void loop()
{
  switch (currentApp) {
    case APP_MENU : runMenu();
    break;
    case APP_TETRIS : runTetris();
    break;
    case APP_SNAKE : runSnake();
    break;
    case APP_BREAKOUT : runBreakout();
    break;
    case APP_PIXEL : runPixel();
    break;
  }
}

void runMenu(){
  bool isRunning = true;
  Menu menu = Menu();
  menu.setup();
  while (isRunning) {
    isRunning = menu.loop();
  }
}

void runTetris(){
  bool isRunning = true;
  Tetris tetris = Tetris();
  tetris.setup();
  while (isRunning) {
    isRunning = tetris.loop();
  }
}

void runSnake(){
  bool isRunning = true;
  Snake snake = Snake();
  snake.setup();
  while (isRunning) {
    isRunning = snake.loop();
  }
}

void runBreakout(){
  bool isRunning = true;
  Breakout breakout = Breakout();
  breakout.setup();
  while (isRunning) {
    isRunning = breakout.loop();
  }
}

void runPixel(){
  bool isRunning = true;
  Pixel pixel = Pixel();
  pixel.setup();
  while (isRunning) {
    isRunning = pixel.loop();
  }
}
