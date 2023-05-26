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

#define MAX_FRAMES_ACCEPTED                         (60u)

static uint16_t standartIndexToReal[DEFAULT_LEDS_AMOUNT_IN_STRIP] = {0};
static bool isTestPatternMode = true;
static bool isImageChanged = false;
static bool isAnimationMode = false;

#define FRAME_HOLD_TIME_IN_MILLI_SECS     (1000)

#pragma pack(1)
typedef struct{
  uint32_t frameImage[DEFAULT_LEDS_AMOUNT_IN_STRIP]; 
  uint32_t frameDuration;
}frameInfo;

typedef struct{
  uint8_t type;
  uint8_t framesAmount;
  uint8_t widthSize;
  uint8_t hightSize;

  frameInfo frame[MAX_FRAMES_ACCEPTED]; 
} WrittenAnimation;
#pragma pack(0)

Adafruit_NeoPixel GifPanel(DEFAULT_LEDS_AMOUNT_IN_STRIP, DEFAULT_GIF_PANEL_PIN, NEO_GRB + NEO_KHZ800);

static void initIndexArray(uint8_t requestedHight, uint8_t requestedWidth){
  uint8_t rowOffset = (DEFAULT_GIF_PANEL_WIDTH - requestedWidth) / 2;
  uint8_t colOffset = (DEFAULT_GIF_PANEL_HIGHT - requestedHight) / 2;

  for(uint8_t i = 0; i < requestedHight; ++i){
    for(uint8_t j = 0; j < requestedWidth; ++j){
      uint16_t currIndex = (i * requestedHight) + j;
      if(i % 2 == 0){
        standartIndexToReal[currIndex] = colOffset + ((i + rowOffset) * DEFAULT_GIF_PANEL_HIGHT) + j;
      }
      else{
        standartIndexToReal[currIndex] = ((i + 1 + rowOffset) * DEFAULT_GIF_PANEL_HIGHT) - j - 1 - colOffset;
      }
    }
  }
}

WrittenAnimation animation = {0};

void GifControl_loadTestPattern(void){
  const uint8_t TP_Amount = 4;
  static uint8_t currTP = 0;

  switch(currTP){
    case 0:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioPlanePattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = EFFECTIVE_GIF_PANEL_WIDTH;
      animation.hightSize = EFFECTIVE_GIF_PANEL_HIGHT;
      break;
    case 1:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioFlowerPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = EFFECTIVE_GIF_PANEL_WIDTH;
      animation.hightSize = EFFECTIVE_GIF_PANEL_HIGHT;
      break;
    case 2:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioGhostPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = EFFECTIVE_GIF_PANEL_WIDTH;
      animation.hightSize = EFFECTIVE_GIF_PANEL_HIGHT;
      break;
    case 3:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_pokeballPattern, EFFECTIVE_LEDS_AMOUNT_IN_STRIP * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = EFFECTIVE_GIF_PANEL_WIDTH;
      animation.hightSize = EFFECTIVE_GIF_PANEL_HIGHT;
      break;
    default:
      animation.framesAmount = 0;
      break;
  }
  currTP = (++currTP) % TP_Amount;

  isImageChanged = true;
}

void GifControl_loadAnimation(void){
  if(isTestPatternMode){
    GifControl_loadTestPattern();
  }

  if(isImageChanged){
    initIndexArray(animation.widthSize, animation.hightSize);
    uint16_t pixelAmount = animation.widthSize * animation.hightSize;

    for(int currFrame = 0; currFrame < animation.framesAmount; ++currFrame){
      GifPanel.clear();
      for(int currPixel = 0; currPixel < pixelAmount; ++currPixel){
        // GifPanel.setPixelColor(standartIndexToReal[i], nextImageToShow[i]);
        uint32_t correctColorRGB = GifPanel.gamma32(animation.frame[currFrame].frameImage[currPixel]);
        GifPanel.setPixelColor(standartIndexToReal[currPixel], correctColorRGB);
      }
      GifPanel.show();

      delay(animation.frame[currFrame].frameDuration);
    }
  }

  isImageChanged = isAnimationMode ? true : false;
}

class getDataFromUART {
  WrittenAnimation *theNextAnimation;

public:
  getDataFromUART(WrittenAnimation *dstStruct) 
    : 
      theNextAnimation(dstStruct)
  {}

  bool operator()(void){
    Serial.readBytes(&theNextAnimation->type, sizeof(uint8_t));
    Serial.readBytes(&theNextAnimation->framesAmount, sizeof(uint8_t));
    Serial.readBytes(&theNextAnimation->widthSize, sizeof(uint8_t));
    Serial.readBytes(&theNextAnimation->hightSize, sizeof(uint8_t));
    uint16_t animationSize = (theNextAnimation->widthSize) * (theNextAnimation->hightSize) * sizeof(uint32_t);

    for(int currFrame = 0; currFrame < theNextAnimation->framesAmount; ++currFrame){
      Serial.readBytes((uint8_t *)(&(theNextAnimation->frame[currFrame].frameImage)), animationSize);
      Serial.readBytes((uint8_t *)(&(theNextAnimation->frame[currFrame].frameDuration)), sizeof(uint32_t));
    }

    isTestPatternMode = false;
    isImageChanged = true;
    isAnimationMode = animation.framesAmount > 1 ? true : false;

    return true;
  }
};

static getDataFromUART basic_UartDataRead(&animation);

void GifControl_initPanel(void){
	GifPanel.begin();
  GifPanel.setBrightness(DEFAULT_GIF_PANEL_BRIGHTNESS);
	GifPanel.clear();
  GifPanel.fill();
	GifPanel.show();
  
  delay(5);
}

void setup() {
  Serial.begin(921600);
  Serial.onReceive(basic_UartDataRead);

  // put your setup code here, to run once:
  GifControl_initPanel();
  animation.type = 1;
}

void loop() {
  // put your main code here, to run repeatedly:
  GifControl_loadAnimation();
}

// void writeFrameToBuffer(void){
//   Serial.readBytes(&animation.type, sizeof(uint8_t));
//   Serial.readBytes(&animation.framesAmount, sizeof(uint8_t));

//   int expectedSize = animation.framesAmount * EFFECTIVE_LEDS_AMOUNT_IN_STRIP;

//   if(expectedSize != Serial.readBytes((uint8_t *)animation.nextImageToShow, expectedSize)){
//     Serial.flush();
//     animation.framesAmount = 0;
//   }

//   isTestPatternMode = false;
//   isImageChanged = true;
// }
