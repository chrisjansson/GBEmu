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
