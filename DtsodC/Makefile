SRC=$(wildcard src/*c) $(wildcard src/**/*.c)
OUTDIR=bin/
OUTFILE=$(OUTDIR)dtsodc.com
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

CMPARGS= -Wall $(SRC) -o $(OUTFILE)
build: 
	@echo "\e[36m-------------[build]---------------\e[0m"
	$(CMP) -O2 $(CMPARGS)
build_dbg: 
	@echo "\e[36m-------------[build_dbg]---------------\e[0m"
	$(CMP) -O0 $(CMPARGS).dbg
test:
	@echo "\e[36m-------------[test]----------------\e[0m"
	$(OUTFILE)
valgrind: clear_c build_dbg
	@echo "\e[36m-----------[valgrind]--------------\e[0m"
	valgrind -s --read-var-info=yes --fullpath-after=DtsodC/ $(OUTFILE).dbg