import sys


if __name__ == "__main__":
	print(f"Arguments count: {len(sys.argv)}")
	fileName = sys.argv[1]
	dstName = sys.argv[2]
	
	with open(fileName, 'r') as src, open(dstName, 'wb') as dstbin:
		sourceData = []
		sourceLines = src.readlines()
		for line in sourceLines:
			sourceData += [int(h, 16).to_bytes(4, 'little') for h in line[:-3].strip().replace('0x', '').split(', ')]
		
		for i in sourceData:
			dstbin.write(i)