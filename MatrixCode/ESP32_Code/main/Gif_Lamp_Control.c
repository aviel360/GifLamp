// #include <Adafruit_NeoPixel.h>
// #include <Adafruit_NeoMatrix.h>
// #include "Gif_Lamp_Control.h"
// #include "TestPatterns.h"

// ///Panel Parameters
// #define DEFAULT_GIF_PANEL_WIDTH						(16u)
// #define DEFAULT_GIF_PANEL_HIGHT						(16u)
// #define LEDS_AMOUNT_IN_STRIP              (DEFAULT_GIF_PANEL_WIDTH * DEFAULT_GIF_PANEL_HIGHT)
// #define DEFAULT_GIF_PANEL_PIN						  (5u)

// ///Panel default programming
// #define DEFAULT_GIF_PANEL_TEXT_WRAP_MODE			      (false)
// #define DEFAULT_GIF_PANEL_BRIGHTNESS				        (40u)
// #define DEFAULT_GIF_PANEL_STARTUP_FILL_SCREEN_COLOR	(0u)

// const static uint16_t standartIndexToReal[DEFAULT_GIF_PANEL_WIDTH * DEFAULT_GIF_PANEL_HIGHT] = 
// {
// 	0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,
// 31,30,29,28,27,26,25,24,23,22,21,20,19,18,17,16,
// 32,33,34,35,36,37,38,39,40,41,42,43,44,45,46,47,
// 63,62,61,60,59,58,57,56,55,54,53,52,51,50,49,48,
// 64,65,66,67,68,69,70,71,72,73,74,75,76,77,78,79,
// 95,94,93,92,91,90,89,88,87,86,85,84,83,82,81,80,
// 96,97,98,99,100,101,102,103,104,105,106,107,108,109,110,111,
// 127,126,125,124,123,122,121,120,119,118,117,116,115,114,113,112,
// 128,129,130,131,132,133,134,135,136,137,138,139,140,141,142,143,
// 159,158,157,156,155,154,153,152,151,150,149,148,147,146,145,144,
// 160,161,162,163,164,165,166,167,168,169,170,171,172,173,174,175,
// 191,190,189,188,187,186,185,184,183,182,181,180,179,178,177,176,
// 192,193,194,195,196,197,198,199,200,201,202,203,204,205,206,207,
// 223,222,221,220,219,218,217,216,215,214,213,212,211,210,209,208,
// 224,225,226,227,228,229,230,231,232,233,234,235,236,237,238,239,
// 255,254,253,252,251,250,249,248,247,246,245,244,243,242,241,240
// };

// Adafruit_NeoPixel GifPanel(LEDS_AMOUNT_IN_STRIP, DEFAULT_GIF_PANEL_PIN, NEO_GRB + NEO_KHZ800);

// void GifControl_initPanel(void){
// 	GifPanel.begin();
// 	// GifPanel.setTextWrap(DEFAULT_GIF_PANEL_TEXT_WRAP_MODE);
// 	GifPanel.clear();
// 	GifPanel.show();
// }

// void GifControl_showTestPattern(void){
// 	const uint32_t tpMatrix[DEFAULT_GIF_PANEL_WIDTH][DEFAULT_GIF_PANEL_HIGHT] = POKEBALL_PATTERN_16_16_MATRIX;
	
// 	for(int r = 0; r < DEFAULT_GIF_PANEL_HIGHT; ++r){
// 		for(int c = 0; c < DEFAULT_GIF_PANEL_WIDTH; ++c){
//       GifPanel.setPixelColor(standartIndexToReal[r*DEFAULT_GIF_PANEL_HIGHT + c], tpMatrix[r][c]);
// 		}
// 	}
// 	GifPanel.show();
// }



