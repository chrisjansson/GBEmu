Performance notes:
MBC bank selection can be computed on set
Display, invalidate tiles on writes instead of every frame...


MBCS:
MBC1
	Does 1MB and 2MB roms contain 64 and 128 banks of data or just 63 and 125? The current assumption is 64 and 128

Does special registers that do not take up 8bits need to be masked?
TAC
IE and IF?
LCDC
P1?

Check that all display registers are implemented properly:
LYC should be reset when written to, even though its read only..

LCD OAM transfer is not implemented
transfer is from $0000-DFFF not $8000-$DFFF as programming manual says

Unimplemented differences between GB and CGB:

IME shoudl be disabled when entering interrupt vector...

If a gb bootstrap rom is not present load the cpu with the correct values,
  see Gameboy CPU Manual p.17
  Test that bootstrapper with and without boot rom produces the same results

Assert memory access when dma transfer is in progress?
