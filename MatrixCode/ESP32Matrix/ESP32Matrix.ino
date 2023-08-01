#include <FastLED.h>
#include <LEDMatrix.h>
#include <LEDSprites.h>
#include <LEDText.h>
#include <FontMatrise.h>
#include "BluetoothSerial.h"
#include "SD.h"
#include "SPI.h"

#include "config.h"
#include "SD_CardModule.h"

#define SCK  18
#define MISO  19
#define MOSI  23
#define CS  5

SPIClass hSPI = SPIClass(HSPI);

// LEDs Panel Initialization
cLEDMatrix<-MATRIX_WIDTH, MATRIX_HEIGHT, VERTICAL_ZIGZAG_MATRIX, HORIZONTAL_BLOCKS_AMOUNT, -VERTICAL_BLOCKS_AMOUNT, VERTICAL_ZIGZAG_BLOCKS> leds;
SD_CardMemory sdModule = SD_CardMemory();


// Bluetooth input
BluetoothSerial SerialBT;
btnInput currentInput = NONE;


ApplicationState currentApp = AppState_Menu;
SavedAnimationData savedData = {0};
WrittenAnimation animation = {0};
uint8_t savedAmount = 0;

#include "Tetris.h"
#include "Menu.h"
#include "Snake.h"
#include "Pixel.h"
#include "Animation.h"

static GifAnimation::getDataFromUART basic_UartDataRead(&animation);
static uint8_t currBrt = MATRIX_START_BRIGHTNESS;

void setup()
{
  Serial.begin(921600);
  Serial.onReceive(basic_UartDataRead);
  SerialBT.begin("GIF_Lamp");

  pinMode(CS, OUTPUT);
  digitalWrite(CS, HIGH);
  
  hSPI.begin(SCK, MISO, MOSI, CS);
  if(!SD.begin(CS, hSPI)){
    Serial.println("Card Mount Failed");
    Serial.println("Card Type: " + String(SD.cardType()));
  } else{
    savedAmount = sdModule.countFiles(SD, "/GIF_Lamp");
    if(0 < savedAmount){
      currentApp = AppState_GifAnimations;
    }
  }

  FastLED.addLeds<CHIPSET, LED_PIN, COLOR_ORDER>(leds[0], leds.Size());
  FastLED.setMaxPowerInVoltsAndMilliamps(5, 20000);
  FastLED.clear(true);
  FastLED.show();
}

void loop()
{
  checkBrt();

  switch (currentApp) {
    case AppState_Menu : 
      runMenu();
      break;
    case AppState_Tetris : 
      runTetris();
      break;
    case AppState_Snake : 
      runSnake();
      break;
    case AppState_GifAnimations : 
      runAnimation();
      break;
    case AppState_PixelArt : 
      runPixel();
      break;
  }
}

void runAnimation(){
  bool isRunning = true;
  GifAnimation animation = GifAnimation();
  animation.setup();
  while (isRunning) {
    checkBrt();
    isRunning = animation.loop();
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

void runPixel(){
  bool isRunning = true;
  Pixel pixel = Pixel();
  pixel.setup();
  while (isRunning) {
    isRunning = pixel.loop();
  }
}

void checkBrt(){
  uint32_t newLuxAmount = analogRead(ABS_PIN);
  uint8_t expectedNewBrt = map(newLuxAmount, 4095, 0, 0, 255);
  uint8_t brtDiff = (expectedNewBrt - currBrt) > 0 ? (expectedNewBrt - currBrt) : (currBrt - expectedNewBrt);

  if(brtDiff > BRT_HYSTERESIS){
    currBrt = expectedNewBrt;
    FastLED.setBrightness(currBrt);
    delay(100);
    FastLED.show();
  }
}
