DEFAULT_WIDTH	=	16
DEFAULT_HIGHT	=	16

WIDTH = 10
HIGHT = 10

rowOffset = (DEFAULT_WIDTH - WIDTH) // 2
colOffset = (DEFAULT_HIGHT - HIGHT) // 2

for i in range(HIGHT):
	for j in range(WIDTH):
		currIndex = i*HIGHT + j
		if i % 2:
			print(f"<{currIndex:03d}> {rowOffset + (i+1)*(DEFAULT_HIGHT) - j - 1:03d}, ", end='')
		else:
			print(f"<{currIndex:03d}> {colOffset + i*(DEFAULT_HIGHT) + j:03d}, ", end='')
	print()