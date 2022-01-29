SRC=src/*.c
OUTDIR=bin/
OUTFILE=dtsodc.com

all: clear_c clear_bin test 

clear_c:
	clear

clear_bin:
	@echo "\e[36m-----------------[clear_bin]-------------------\e[0m"
	touch $(OUTDIR)_.com
	rm $(OUTDIR)*.com

# using sdlib
STDARGS=-D STDLIB

std_build: 
	@echo "\e[36m-------------[std_build]---------------\e[0m"
	gcc $(STDARGS) $(SRC) -o $(OUTDIR)std_$(OUTFILE)

std_test: std_build
	@echo "\e[36m----------------[std_test]------------------\e[0m"
	$(OUTDIR)std_$(OUTFILE)


# using cosmopolitan
COSMARGS_PRE=-g -O -static -fno-pie -no-pie -mno-red-zone -nostdlib -nostdinc -D COSMOPOLITAN 
COSMARGS_POST=-Wl,--oformat=binary -Wl,--gc-sections -Wl,-z,max-page-size=0x1000 -fuse-ld=bfd -Wl,-T,cosmopolitan/ape.lds \
	-include cosmopolitan/cosmopolitan.h cosmopolitan/crt.o ./cosmopolitan/ape.o cosmopolitan/cosmopolitan.a

build:
	@echo "\e[36m----------------[build]----------------\e[0m"
	gcc $(COSMARGS_PRE) $(SRC) $(COSMARGS_POST) -o $(OUTDIR)$(OUTFILE)

test: build
	@echo "\e[36m-----------------[test]----------------\e[0m"
	$(OUTDIR)$(OUTFILE)