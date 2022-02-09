SRC=$(wildcard src/*c) $(wildcard src/**/*.c)
OUTDIR=bin/
OUTFILE=dtsodc.com
CMP=gcc
all: clear_c build test

clear_c:
	clear
clear_bin:
	@echo "\e[36m-------------[clear_bin]---------------\e[0m"
	touch $(OUTDIR)_.com
	rm $(OUTDIR)*.com

clang: CMP=clang
clang: all

CMPARGS=-O -static
build: 
	@echo "\e[36m-------------[build]---------------\e[0m"
	$(CMP) $(CMPARGS) $(SRC) -o $(OUTDIR)$(OUTFILE)
test:
	@echo "\e[36m-------------[test]----------------\e[0m"
	$(OUTDIR)$(OUTFILE)
