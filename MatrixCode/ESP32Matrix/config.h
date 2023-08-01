#ifndef config_h
#define config_h

#define LED_PIN                         22
#define ABS_PIN                         (2)
#define POTENTIOMETER_PIN               (4)

#define COLOR_ORDER                     GRB
#define CHIPSET                         WS2812B
#define MATRIX_WIDTH                    (32)
#define MATRIX_HEIGHT                   (16)
#define VERTICAL_BLOCKS_AMOUNT          (2)
#define HORIZONTAL_BLOCKS_AMOUNT        (1)
#define MATRIX_TYPE                     (HORIZONTAL_ZIGZAG_MATRIX)

#define MATRIX_MAX_WIDTH                (32)
#define MATRIX_MAX_HIGHT                (32)
#define MATRIX_MAX_LEDS_IN_STRIP        (MATRIX_MAX_WIDTH * MATRIX_MAX_HIGHT)

#define MAX_ANIMATION_FRAMES_ACCEPTED   (18)
#define BRT_HYSTERESIS                  (8)
#define MATRIX_START_BRIGHTNESS		      (40u)

#define IS_PLASMA_BACKGROUND_NEEDED   (false)

typedef enum{
  AppState_Menu             = 0,
  AppState_Tetris           = 1,
  AppState_Snake            = 2,
  AppState_PixelArt         = 3,
  AppState_GifAnimations    = 4,
  AppState_Amount
}ApplicationState;

typedef enum{
  NONE, 
  UP, 
  DOWN, 
  LEFT, 
  RIGHT,
  ACTION
}btnInput;

#pragma pack(1)
typedef struct{
  uint32_t frameImage[MATRIX_MAX_LEDS_IN_STRIP]; 
  uint32_t frameDuration;
}frameInfo;

typedef struct{
  uint8_t type;
  uint8_t framesAmount;
  uint8_t widthSize;
  uint8_t hightSize;

  frameInfo frame[MAX_ANIMATION_FRAMES_ACCEPTED]; 
} WrittenAnimation;

typedef struct{
  uint32_t u32_gifsAmount;
} SavedAnimationData;
#pragma pack(0)
#endif
