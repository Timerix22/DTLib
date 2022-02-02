SRC=$(wildcard src/*c) $(wildcard src/**/*.c)
OUTDIR=bin/
OUTFILE=dtsodc.com
CMP=gcc
all: clear_c build test
std: clear_c std_build std_test

clear_c:
	clear
clear_bin:
	@echo "\e[36m-----------------[clear_bin]-------------------\e[0m"
	touch $(OUTDIR)_.com
	rm $(OUTDIR)*.com

# using sdlib
STDARGS=-D STDLIB -O
std_build: 
	@echo "\e[36m-------------[std_build]---------------\e[0m"
	$(CMP) $(STDARGS) $(SRC) -o $(OUTDIR)std_$(OUTFILE)
std_test:
	@echo "\e[36m----------------[std_test]------------------\e[0m"
	$(OUTDIR)std_$(OUTFILE)

# using cosmopolitan
COSMARGS_PRE=-g -O -static -fno-pie -no-pie -mno-red-zone -nostdlib -nostdinc -D COSMOPOLITAN 
COSMARGS_POST=-Wl,--oformat=binary -Wl,--gc-sections -Wl,-z,max-page-size=0x1000 -fuse-ld=bfd -Wl,-T,cosmopolitan/ape.lds \
	-include cosmopolitan/cosmopolitan.h cosmopolitan/crt.o ./cosmopolitan/ape.o cosmopolitan/cosmopolitan.a
build:
	@echo "\e[36m----------------[build]----------------\e[0m"
	$(CMP) $(COSMARGS_PRE) $(SRC) $(COSMARGS_POST) -o $(OUTDIR)$(OUTFILE)
test:
	@echo "\e[36m-----------------[test]----------------\e[0m"
	$(OUTDIR)$(OUTFILE)
