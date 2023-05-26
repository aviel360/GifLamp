import sys
from PIL import Image


if __name__ == "__main__":
	print(f"Arguments count: {len(sys.argv)}")
	orgGifFileName = sys.argv[1]
	dstName = orgGifFileName[0: -4] + '.bin'
	
	orgFullGif = Image.open(orgGifFileName)
	requiredFPS = orgFullGif.info["duration"]
	requiredFPS = requiredFPS #if requiredFPS <= 255 else 100
	print(f'Original Size is {orgFullGif.size}')
	print(f'and the number of frames is {orgFullGif.n_frames}')
	print(f'The FPS is {requiredFPS}')
	
	resizedFullGif = []
	for i in range(orgFullGif.n_frames):
		orgFullGif.seek(i)
		resizedFullGif.append(orgFullGif.resize((16,16)).convert('RGB'))
	
	with open(dstName, 'wb') as dstAnimation:
		dstAnimation.write(requiredFPS.to_bytes(1, 'little'))
		dstAnimation.write(len(resizedFullGif).to_bytes(1, 'little'))
		for im in resizedFullGif:
			for i in range(16):
				for j in range(16):
					r, g, b = im.getpixel((j, i))
					val = (r << 16) | (g << 8) | b
					dstAnimation.write(val.to_bytes(4, 'little'))