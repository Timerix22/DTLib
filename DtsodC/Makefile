SRC_LIB=src/autosize_array/Autoarr.c src/throw.c src/strict_types.c
SRC_COM=src/test.c

all: clear_c std_build test 

clear_c:
	clear

clear:
	@echo "\e[36m-----------------[clear]-------------------\e[0m"
	rm bin/*.com

# using sdlib
STDARGS=-D STDLIB

std_build: std_build_test

std_build_lib:
	@echo "\e[36m-------------[std_build_lib]---------------\e[0m"
	gcc $(STDARGS) $(SRC_LIB) -o bin/dtsodc_test_STD.dll 

std_build_test: 
	@echo "\e[36m-------------[std_build_test]---------------\e[0m"
	gcc $(STDARGS) $(SRC_LIB) $(SRC_COM) -o bin/dtsodc_test_STD.com 

std_test: std_build_test
	@echo "\e[36m----------------[std_test]------------------\e[0m"
	bin/dtsodc_test_STD.com


# using cosmopolitan
COSMARGS_PRE=-g -O -static -fno-pie -no-pie -mno-red-zone -nostdlib -nostdinc -D COSMOPOLITAN 
COSMARGS_POST=-Wl,--oformat=binary -Wl,--gc-sections -Wl,-z,max-page-size=0x1000 -fuse-ld=bfd -Wl,-T,cosmopolitan/ape.lds \
	-include cosmopolitan/cosmopolitan.h cosmopolitan/crt.o ./cosmopolitan/ape-no-modify-self.o cosmopolitan/cosmopolitan.a

build: build_test

build_lib:
	@echo "\e[36m----------------[build_lib]-----------------\e[0m"
	gcc $(COSMARGS_PRE) $(SRC_LIB) $(COSMARGS_POST) -o bin/dtsodc_test.dll

build_test:
	@echo "\e[36m----------------[build_test]----------------\e[0m"
	gcc $(COSMARGS_PRE) $(SRC_LIB) $(SRC_COM) $(COSMARGS_POST) -o bin/dtsodc_test.com

test: build_test
	@echo "\e[36m-----------------[test]----------------\e[0m"
	bin/dtsodc_test.com