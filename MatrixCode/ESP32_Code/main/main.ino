#include <Adafruit_NeoPixel.h>
#include <Adafruit_NeoMatrix.h>
#include <Adafruit_GFX.h>
#include "Gif_Lamp_Control.h"
#include "TestPatterns.h"

///Panel Parameters
#define SINGLE_MATRIX_TILE_WIDTH             (16u)
#define SINGLE_MATRIX_TILE_HIGHT             (16u)

#define HORIZONTAL_TILES_AMOUNT              (2u)
#define VERTICAL_TILES_AMOUNT                (2u)

#define DEFAULT_GIF_PANEL_WIDTH						   (SINGLE_MATRIX_TILE_WIDTH * HORIZONTAL_TILES_AMOUNT)
#define DEFAULT_GIF_PANEL_HIGHT						   (SINGLE_MATRIX_TILE_HIGHT * VERTICAL_TILES_AMOUNT)

#define DEFAULT_LEDS_AMOUNT_IN_STRIP         (DEFAULT_GIF_PANEL_WIDTH * DEFAULT_GIF_PANEL_HIGHT)
#define DEFAULT_GIF_PANEL_PIN						     (5u)

///Panel default programming
#define DEFAULT_GIF_PANEL_TEXT_WRAP_MODE			      (false)
#define DEFAULT_GIF_PANEL_BRIGHTNESS				        (40u)
#define DEFAULT_GIF_PANEL_STARTUP_FILL_SCREEN_COLOR	(0u)

#define MAX_FRAMES_ACCEPTED                         (10u)

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

Adafruit_NeoMatrix GifMatrix(16, 16, 2, 2, DEFAULT_GIF_PANEL_PIN,
    NEO_MATRIX_BOTTOM    + NEO_MATRIX_RIGHT +
    NEO_MATRIX_COLUMNS   + NEO_MATRIX_ZIGZAG + 
    NEO_TILE_TOP         + NEO_TILE_RIGHT +  NEO_TILE_PROGRESSIVE,
    NEO_GRB              + NEO_KHZ800 );

WrittenAnimation animation = {0};

void GifControl_loadTestPattern(void){
  const uint8_t TP_Amount = 4;
  const uint32_t tpSize = 16 * 16;
  static uint8_t currTP = 0;

  switch(currTP){
    case 0:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioPlanePattern, tpSize * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = 16;
      animation.hightSize = 16;
      break;
    case 1:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioFlowerPattern, tpSize * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = 16;
      animation.hightSize = 16;
      break;
    case 2:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_marioGhostPattern, tpSize * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = 16;
      animation.hightSize = 16;
      break;
    case 3:
      animation.framesAmount = 1;
      memcpy(animation.frame[0].frameImage, tp_pokeballPattern, tpSize * sizeof(uint32_t));
      animation.frame[0].frameDuration = 1000;
      animation.widthSize = 16;
      animation.hightSize = 16;
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
    uint8_t startCol = (DEFAULT_GIF_PANEL_WIDTH - animation.widthSize) / 2;
    uint8_t startRow = (DEFAULT_GIF_PANEL_WIDTH - animation.hightSize) / 2;
    uint16_t pixelAmount = animation.widthSize * animation.hightSize;

    for(int currFrame = 0; currFrame < animation.framesAmount; ++currFrame){
      GifMatrix.clear();
      uint32_t currPixel = 0;
      for(uint8_t col = startCol; col < startCol + animation.widthSize; ++col){
        for(uint8_t row = startRow; row < startRow + animation.hightSize; ++row){
          uint32_t RGB_24bits = animation.frame[currFrame].frameImage[currPixel];
          uint16_t correctColorRGB = GifMatrix.Color(((RGB_24bits >> 16) & 0xFF), ((RGB_24bits >> 8) & 0xFF), (RGB_24bits & 0xFF));
          GifMatrix.drawPixel(row, col, correctColorRGB);
          currPixel++;
        }
      }
      GifMatrix.show();

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

void GifControl_initMatrix(void){
  GifMatrix.begin();
  GifMatrix.setTextWrap(false);
  GifMatrix.setBrightness(DEFAULT_GIF_PANEL_BRIGHTNESS);
  GifMatrix.clear();
  GifMatrix.show();

  delay(5);
}

void setup(){
  Serial.begin(921600);
  Serial.onReceive(basic_UartDataRead);
  GifControl_initMatrix();

  animation.type = 1;
}

void loop() {
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
