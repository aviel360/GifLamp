import sys
from PIL import Image

MAX_WIDTH = 16
MAX_HIGHT = 16


if __name__ == "__main__":
	print(f"Arguments count: {len(sys.argv)}")
	orgGifFileName = sys.argv[1]
	dstName = orgGifFileName[0: -4] + '.bin'
	
	orgFullGif = Image.open(orgGifFileName)
	print(f'Original Size is {orgFullGif.size}')
	print(f'and the number of frames is {orgFullGif.n_frames}')
	
	resizeToWidth = MAX_WIDTH if orgFullGif.size[0] > MAX_WIDTH else orgFullGif.size[0]
	resizeToHight = MAX_HIGHT if orgFullGif.size[1] > MAX_HIGHT else orgFullGif.size[1]
	
	resizedFullGif = []
	for i in range(orgFullGif.n_frames):
		orgFullGif.seek(i)
		resizedFullGif.append(orgFullGif.resize((resizeToWidth, resizeToHight)).convert('RGB'))
	
	with open(dstName, 'wb') as dstAnimation:
		dstAnimation.write((1).to_bytes(1, 'little')) #type
		dstAnimation.write(len(resizedFullGif).to_bytes(1, 'little')) #framesAmount
		dstAnimation.write(resizeToWidth.to_bytes(1, 'little')) #widthSize
		dstAnimation.write(resizeToHight.to_bytes(1, 'little')) #higthSize
		for im in resizedFullGif:
			for i in range(resizeToWidth):
				for j in range(resizeToHight):
					r, g, b = im.getpixel((j, i))
					val = (r << 16) | (g << 8) | b
					dstAnimation.write(val.to_bytes(4, 'little'))
			dstAnimation.write(im.info["duration"].to_bytes(4, 'little'))