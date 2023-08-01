#include "config.h"
#include "TestPatterns.h"

static bool isTestPatternMode = true;
static bool isImageChanged = false;
static bool isAnimationMode = false;
static int  speedMultiplier = 0;
static bool isUploading = false;
static bool isNewAnimation = false;
static bool isStayMode = false;
static uint8_t currIndx = 0;

class GifAnimation{
private:
  
public:
  void setup(){
    FastLED.clear(true);
    GifControl_loadAnimation();
  }

  boolean loop(){

    if (SerialBT.available()) {
      char keyPress = (char)SerialBT.read();
      switch(keyPress) {
        case '-':
          ESP.restart();
          break;
        case 'm':
          currentApp = AppState_Menu;
          return false;
        case '0':
          currentApp = AppState_Menu;
          return false;
        case '1':
          currentApp = AppState_Tetris;
          return false;
        case '2':
          currentApp = AppState_Snake;
          return false;
        case '3':
          currentApp = AppState_PixelArt;
          return false;
      }
    }

    GifControl_loadAnimation();

    return true;
  }

  class getDataFromUART {
    WrittenAnimation *theNextAnimation;

    public:
    getDataFromUART(WrittenAnimation *dstStruct) 
      : 
        theNextAnimation(dstStruct)
    {}

    bool operator()(void){
      isUploading = true;
      // isTestPatternMode = false;
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
      isNewAnimation = true;
      // isAnimationMode = animation.framesAmount > 1 ? true : false;
      isStayMode = true;
      isUploading = false;
      
      return true;
    }
  };

private:
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
        isImageChanged = true;
        break;
      case 1:
        animation.framesAmount = 1;
        memcpy(animation.frame[0].frameImage, tp_marioFlowerPattern, tpSize * sizeof(uint32_t));
        animation.frame[0].frameDuration = 1000;
        animation.widthSize = 16;
        animation.hightSize = 16;
        isImageChanged = true;
        break;
      case 2:
        animation.framesAmount = 1;
        memcpy(animation.frame[0].frameImage, tp_marioGhostPattern, tpSize * sizeof(uint32_t));
        animation.frame[0].frameDuration = 1000;
        animation.widthSize = 16;
        animation.hightSize = 16;
        isImageChanged = true;
        break;
      case 3:
        animation.framesAmount = 1;
        memcpy(animation.frame[0].frameImage, tp_pokeballPattern, tpSize * sizeof(uint32_t));
        animation.frame[0].frameDuration = 1000;
        animation.widthSize = 16;
        animation.hightSize = 16;
        isImageChanged = true;
        break;
      default:
        animation.framesAmount = 0;
        isImageChanged = true;
        break;
    }
    currTP = (++currTP) % TP_Amount;
  }

  void GifControl_loadAnimation(void){   
    if(isUploading){
      return;
    }

    if(isTestPatternMode){
      if(0 < savedAmount){
        char path[17] = {0};
        sprintf(path, "/GIF_Lamp/%02d.bin", currIndx);
        File currFile = SD.open(path);
        currIndx = (currIndx + 1) % savedAmount;

        currFile.read((uint8_t *) &animation, sizeof(WrittenAnimation));
        isTestPatternMode = false;
        isImageChanged = true;
      }
      else{
        GifControl_loadTestPattern();
      }
    }
    
    if(isNewAnimation){
      char path[17] = {0};
      sprintf(path, "/GIF_Lamp/%02d.bin", savedAmount);

      Serial.println(path);
      File file = SD.open(path, FILE_WRITE);
      if(!file){
        Serial.println("Error on opening file");
      }
      else{
        file.write((uint8_t *) &animation, sizeof(WrittenAnimation));
        file.close();
        savedAmount = (savedAmount + 1) % 100;
      }

      isNewAnimation = false;
    }
    

    if(isImageChanged){
      uint8_t startCol = (MATRIX_MAX_WIDTH - animation.widthSize) / 2;
      uint8_t startRow = (MATRIX_MAX_HIGHT - animation.hightSize) / 2;
      uint16_t pixelAmount = animation.widthSize * animation.hightSize;
      Serial.println(animation.framesAmount);
      for(int currFrame = 0; currFrame < animation.framesAmount; ++currFrame){
        FastLED.clear();
        uint32_t currPixel = 0;
        for(uint8_t col = startCol; col < startCol + animation.widthSize; ++col){
          for(uint8_t row = startRow; row < startRow + animation.hightSize; ++row){
            uint32_t RGB_24bits = animation.frame[currFrame].frameImage[currPixel];
            struct CRGB correctColorRGB = CRGB(((RGB_24bits >> 16) & 0xFF), ((RGB_24bits >> 8) & 0xFF), (RGB_24bits & 0xFF));
            leds(row, MATRIX_MAX_HIGHT - col) = correctColorRGB;
            currPixel++;
          }
        }
        uint32_t potentiometerValue = analogRead(POTENTIOMETER_PIN);
        speedMultiplier = map(potentiometerValue, 0, 4095, -3, 3);

        uint8_t effectiveMult = (0 == speedMultiplier) ? 1 : (0 < speedMultiplier ? speedMultiplier : -speedMultiplier);
        uint32_t delayTime;
        if(speedMultiplier < 0){
          delayTime = animation.frame[currFrame].frameDuration * effectiveMult;
        }
        else{
          delayTime = animation.frame[currFrame].frameDuration / effectiveMult + 1;
        }

        FastLED.show();
        delay(delayTime);
      }

      if(!isTestPatternMode && !isStayMode){
        char path[17] = {0};
        sprintf(path, "/GIF_Lamp/%02d.bin", currIndx);
        File currFile = SD.open(path);
        currIndx = (currIndx + 1) % savedAmount;

        currFile.read((uint8_t *) &animation, sizeof(WrittenAnimation));
        isImageChanged = true;
      }
    }
  }
};
