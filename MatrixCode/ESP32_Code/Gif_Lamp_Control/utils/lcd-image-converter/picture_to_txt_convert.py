# pip install Pillow
from PIL import Image
import sys

def creat_txt_from_pict(pic_path: str):
  # Open the image and resize it to 16x16 pixels
  image = Image.open(pic_path).resize((16, 16))

  # Convert the image to RGB mode
  image = image.convert("RGB")

  # Create a list to store the hexadecimal values
  hex_list = []

  # Iterate over each pixel in the image and convert its RGB values to a hexadecimal string
  for y in range(image.height):
      for x in range(image.width):
          r, g, b = image.getpixel((x, y))
          hex_value = f"{r:02X}{g:02X}{b:02X}"
          hex_list.append(hex_value)

  # Write the hexadecimal values to a text file
  output_file = pic_path.split('.')[0] + '.txt'
  with open(output_file, "w") as f:
      f.write("\n".join(hex_list))

if __name__ == "__main__":
    for arg in sys.argv:
      if arg.endswith(".py"):
        continue
      creat_txt_from_pict(arg)
