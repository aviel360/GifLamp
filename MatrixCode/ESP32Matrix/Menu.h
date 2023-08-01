class Menu {
  cLEDText ScrollingMsg;
  unsigned char txtMenu[15];
  ApplicationState menuItem;
  
  public:
  Menu(){
    menuItem = AppState_Tetris;  
  };

  void setup() {
    sprintf((char *)txtMenu, "              ");
    ScrollingMsg.SetFont(MatriseFontData);
    ScrollingMsg.Init(&leds, leds.Width(), ScrollingMsg.FontHeight() + 1, 0, (32 - ScrollingMsg.FontHeight()) / 2/*5*/);
    ScrollingMsg.SetBackgroundMode(BACKGND_ERASE);
    ScrollingMsg.SetScrollDirection(SCROLL_LEFT);
    ScrollingMsg.SetFrameRate(5);
    ScrollingMsg.SetTextColrOptions(COLR_RGB | COLR_SINGLE, 0xff, 0x00, 0xff);
    FastLED.clear();
    
    menuChanged(AppState_Tetris);
  }

  boolean loop() {
    // Rainbow background
    uint32_t ms = millis();
    int32_t yHueDelta32 = ((int32_t)cos16( ms * 27 ) * (350 / leds.Width()));
    int32_t xHueDelta32 = ((int32_t)cos16( ms * 39 ) * (310 / leds.Height()));
    DrawOneFrame( ms / 65536, yHueDelta32 / 32768, xHueDelta32 / 32768);

    //Black bar behind text
    uint8_t blackBarStartRow = (MATRIX_MAX_WIDTH - ScrollingMsg.FontHeight()) / 2;
    uint8_t blackBarWidth = ScrollingMsg.FontHeight();
    for(uint8_t row = 0; row < blackBarWidth + 2; ++row){
      for(uint8_t col = 0; col < MATRIX_MAX_WIDTH; ++col){
        leds(MATRIX_MAX_HIGHT - col, blackBarStartRow + row - 1) = CRGB::Black;
      }
    }
    
    displayMenu();
        
    if(SerialBT.available()){
        char keyPress = (char)SerialBT.read();
        switch(keyPress) {
          case 'a':
            menuItem = (ApplicationState)((AppState_Menu + 1) == menuItem ? (AppState_Amount - 1) : (menuItem - 1));
            menuChanged(menuItem);
            break;
          case 'd':
            menuItem = (ApplicationState)((AppState_GifAnimations == menuItem) ? (AppState_Menu + 1) : (menuItem + 1));
            menuChanged(menuItem);
            break;
          case 'y':
            currentApp = menuItem;
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
          case '4':
            currentApp = AppState_GifAnimations;
            return false;
        }
    }
    return true;
  }

  void DrawOneFrame( byte startHue8, int8_t yHueDelta8, int8_t xHueDelta8)
  {
    byte lineStartHue = startHue8;
    for( byte y = 0; y < leds.Height(); y++) {
      lineStartHue += yHueDelta8;
      byte pixelHue = lineStartHue;      
      for( byte x = 0; x < leds.Width(); x++) {
        pixelHue += xHueDelta8;
        leds(x, y)  = CHSV( pixelHue, 255, 255);
      }
    }
  }

  private:
  void menuChanged(ApplicationState menuItem) {
    switch (menuItem) {
      case AppState_Tetris:
        //Tetris highlighted
        sprintf((char *)txtMenu, "      TETRIS  ");
        break;
      case AppState_Snake:
        //Snake highlighted
        sprintf((char *)txtMenu, "       SNAKE  ");
        break;
      case AppState_GifAnimations:
        //Breakout highlighted
        sprintf((char *)txtMenu, "      GIFS    ");
        break;
      case AppState_PixelArt:
        //Animation highlighted
        sprintf((char *)txtMenu, "   PIXEL ART  ");
        break;
    }
    ScrollingMsg.SetText((unsigned char *)txtMenu, sizeof(txtMenu) - 1);
  }
  
  void displayMenu() {
    if(-1 == ScrollingMsg.UpdateText())
      ScrollingMsg.SetText((unsigned char *)txtMenu, sizeof(txtMenu) - 1);
    else
      FastLED.show();
  }
  
};
