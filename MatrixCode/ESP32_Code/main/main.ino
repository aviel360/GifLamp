
#include <Adafruit_NeoPixel.h>
#include <Adafruit_NeoMatrix.h>
#include <Adafruit_GFX.h>
#include "Gif_Lamp_Control.h"
#include "TestPatterns.h"

///Panel Parameters
#define DEFAULT_GIF_PANEL_WIDTH						   (16u)
#define DEFAULT_GIF_PANEL_HIGHT						   (16u)

#define EFFECTIVE_GIF_PANEL_WIDTH						 (16u)
#define EFFECTIVE_GIF_PANEL_HIGHT						 (16u)

#define DEFAULT_LEDS_AMOUNT_IN_STRIP         (DEFAULT_GIF_PANEL_WIDTH * DEFAULT_GIF_PANEL_HIGHT)
#define EFFECTIVE_LEDS_AMOUNT_IN_STRIP       (EFFECTIVE_GIF_PANEL_HIGHT  * EFFECTIVE_GIF_PANEL_HIGHT)
#define DEFAULT_GIF_PANEL_PIN						     (5u)

///Panel default programming
#define DEFAULT_GIF_PANEL_TEXT_WRAP_MODE			      (false)
#define DEFAULT_GIF_PANEL_BRIGHTNESS				        (40u)
#define DEFAULT_GIF_PANEL_STARTUP_FILL_SCREEN_COLOR	(0u)

static uint16_t standartIndexToReal[EFFECTIVE_GIF_PANEL_HIGHT * EFFECTIVE_GIF_PANEL_WIDTH] = {0};
static bool isTestPatternMode = true;
static bool isImageChanged = false;

Adafruit_NeoPixel GifPanel(DEFAULT_LEDS_AMOUNT_IN_STRIP, DEFAULT_GIF_PANEL_PIN, NEO_GRB + NEO_KHZ800);

static void initIndexArray(void){
  for(uint8_t i = 0; i < EFFECTIVE_GIF_PANEL_HIGHT; ++i){
    for(uint8_t j = 0; j < EFFECTIVE_GIF_PANEL_WIDTH; ++j){
      uint16_t currIndex = (i * EFFECTIVE_GIF_PANEL_HIGHT) + j;
      if(i % 2 == 0){
        standartIndexToReal[currIndex] = (i * DEFAULT_GIF_PANEL_HIGHT) + j;
      }
      else{
        standartIndexToReal[currIndex] = ((i + 1) * DEFAULT_GIF_PANEL_HIGHT) - j - 1;
      }
    }
  }
}

void GifControl_initPanel(void){
  initIndexArray();

	GifPanel.begin();
  GifPanel.setBrightness(DEFAULT_GIF_PANEL_BRIGHTNESS);
	GifPanel.clear();
  GifPanel.fill();
	GifPanel.show();
  
  delay(5);
}

uint32_t nextImageToShow[EFFECTIVE_LEDS_AMOUNT_IN_STRIP] = {0};

void GifControl_showTestPattern(void){
  const uint8_t TP_Amount = 4;
  static uint8_t currTP = 0;

  if(isTestPatternMode){
    switch(currTP){
    case 0:
      memcpy(nextImageToShow, tp_marioPlanePattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      break;
    case 1:
      memcpy(nextImageToShow, tp_marioFlowerPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      break;
    case 2:
      memcpy(nextImageToShow, tp_marioGhostPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      break;
    case 3:
      memcpy(nextImageToShow, tp_pokeballPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      break;
    default:
      break;
    }
    currTP = (++currTP) % TP_Amount;

    isImageChanged = true;
  }

  if(isImageChanged){
    GifPanel.clear();
    for(int i = 0; i < EFFECTIVE_LEDS_AMOUNT_IN_STRIP; ++i){
      // GifPanel.setPixelColor(standartIndexToReal[i], nextImageToShow[i]);
      uint32_t correctColorRGB = GifPanel.gamma32(nextImageToShow[i]);
      GifPanel.setPixelColor(standartIndexToReal[i], correctColorRGB);
    }
    GifPanel.show();
  }

  isImageChanged = false;
}

void writeFrameToBuffer(void){
  if(Serial.readBytes((char *)nextImageToShow, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t)) != EFFECTIVE_LEDS_AMOUNT_IN_STRIP){
    Serial.flush();
  }

  isTestPatternMode = false;
  isImageChanged = true;
}

class getDataFromUART {
  uint32_t* dataDst;
  uint32_t dataLen;
  uint16_t framesAmount;

public:
  getDataFromUART(uint32_t* dstMat, uint32_t expectedLen) 
    : 
      dataDst(dstMat), dataLen(expectedLen * sizeof(uint32_t))
  {}

  bool operator()(void){
    int readLen = Serial.readBytes((char *)dataDst, dataLen);
    if(!dataDst || (readLen != dataLen)){
      Serial.flush();
      Serial.println("Need more bytes %d" + readLen);

      return false;
    }
    else{
      isTestPatternMode = false;
      isImageChanged = true;

      Serial.println("Went Well " + readLen);

      return true;
    }
  }
};

static getDataFromUART basic_UartDataRead(nextImageToShow, EFFECTIVE_LEDS_AMOUNT_IN_STRIP);

void setup() {
  Serial.begin(921600);
  Serial.onReceive(basic_UartDataRead);

  // put your setup code here, to run once:
  GifControl_initPanel();
}

#define FRAME_HOLD_TIME_IN_MILLI_SECS     (1000)
void loop() {
  // put your main code here, to run repeatedly:

  GifControl_showTestPattern();
  delay(FRAME_HOLD_TIME_IN_MILLI_SECS);
}

// void loop(){
//   static uint8_t currIndex = 0;

//   GifPanel.clear();
//   for(int i = 0; i < 10; i++){
//     uint32_t currIndexFade = (currIndex + 256 - i) % 256;
//     uint32_t currColor = GifPanel.Color(255 / (i+1), 255 / (i+1), 255 / (i+1));
//     GifPanel.setPixelColor(standartIndexToReal[currIndexFade], currColor);
//   }
//   GifPanel.show();

//   currIndex = (currIndex + 1) % 256;

//   delay(100);
// }
